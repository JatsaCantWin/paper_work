using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public GameObject roomCorridor;
    public GameObject roomCorridorDoor;
    public GameObject roomOffice;

    public int numFloors = 5;
    public int roomsPerFloor = 5;
    public int entranceCoordX = 0;
    public int entranceCoordY = 0;
    
    public float spaceBetweenRooms;
    public float spaceBetweenFloors;
    public float scale;
    public float roomWidth;
    public float roomHeight;

    public GameObject[,] gameMap;

    private const int RoomTypeNone = 0;
    private const int RoomTypeEmpty = 10;
    private const int RoomTypeStairs = 20;
    private const int RoomTypeOffice = 30;
    private const int RoomTypeEntrance = 10;

    private const int MaxOfficeRooms = 3;

    private void Awake()
    {
        gameMap = new GameObject[numFloors, roomsPerFloor];
        GenerateMap();
    }

    private void GenerateMap()
    {
        int[,] roomArray = new int[numFloors, roomsPerFloor];

        Debug.Log(roomArray.GetLength(0));
        
        GenerateStairs(roomArray);
        GenerateRemainingRooms(roomArray);
        GenerateOfficeRooms(roomArray);

        for (int i = 0; i < roomArray.GetLength(0); i++)
        {
            String floorString = "Floor " + i + " ";
            for (var j = 0; j < roomArray.GetLength(1); j++)
            {
                floorString += roomArray[i, j] + " ";
            }
            Debug.Log(floorString);
        }

        roomWidth *= scale;
        roomHeight *= scale;

        for (int row = 0; row < numFloors; row++)
        {
            for (int col = 0; col < roomsPerFloor; col++)
            {
                int roomType = roomArray[row, col];
                GameObject roomPrefab = null;

                if (row == 0 && col == 0)
                {
                    // Bottom right corner room is always a corridor room
                    roomType = 10;
                }
                
                switch (roomType)
                {
                    case RoomTypeNone:
                        break;
                    case RoomTypeEmpty:
                        roomPrefab = roomCorridor;
                        break;
                    case RoomTypeStairs:
                        roomPrefab = roomCorridorDoor;
                        break;
                    case RoomTypeOffice:
                        roomPrefab = roomOffice;
                        break;
                    default:
                        Debug.LogWarning("Invalid room type: " + roomType);
                        break;
                }

                if (roomPrefab != null)
                {
                    float posX = col * (roomWidth + spaceBetweenRooms);
                    float posY = row * (roomHeight + spaceBetweenFloors);

                    GameObject roomInstance = Instantiate(roomPrefab, transform);
                    roomInstance.transform.localPosition = new Vector3(posX, posY, 0f);
                    roomInstance.transform.localScale = new Vector3(scale, scale, 1f);

                    gameMap[row, col] = roomInstance;
                }
            }
        }
        
        ConnectRooms();
    }

    private void GenerateStairs(int[,] roomArray)
    {
        var prevRandomCol = -1;
        var randomCol = -1;

        for (var row = 0; row < numFloors-1; row+=2)
        {
            while (randomCol == prevRandomCol)
                randomCol = Random.Range(0, roomsPerFloor);

            prevRandomCol = randomCol;

            roomArray[row, randomCol] = RoomTypeStairs;
            roomArray[row+1, randomCol] = RoomTypeStairs;
            if (row != 0)
            {
                roomArray[row - 1, randomCol] = RoomTypeStairs;
            }
        }

        // If the map height is uneven, add a third set of stairs to the top floor
        if (roomArray.GetLength(0) % 2 == 1)
        {
            roomArray[roomArray.GetLength(0) - 1, randomCol] = RoomTypeStairs;
        }
    }

    void GenerateRemainingRooms(int[,] roomArray)
    {
        for (int row = 0; row < numFloors; row++)
        {
            for (int col = 0; col < roomsPerFloor; col++)
            {
                if (roomArray[row, col] == 0)
                {
                    int roomType = 10;
                    roomArray[row, col] = roomType;
                }
            }
        }
    }

    void GenerateOfficeRooms(int[,] roomArray)
    {
        var officeRoomCount = 0;

        while (officeRoomCount < MaxOfficeRooms)
        {
            var randomRow = Random.Range(0, numFloors);
            var randomCol = Random.Range(0, roomsPerFloor);

            if (!CanBeOffice(roomArray, randomRow, randomCol))
            {
                continue;
            }

            roomArray[randomRow, randomCol] = RoomTypeOffice;
            officeRoomCount++;

        }
    }

    private bool CanBeOffice(int[,] roomArray, int roomX, int roomY)
    {
        var roomId = roomArray[roomX, roomY];

        if ((roomX == entranceCoordX) && (roomY == entranceCoordY))
            return false;

        if (roomId != RoomTypeEmpty)
            return false;
        
        return true;
    }
    
    private void ConnectRooms()
    {
        for (int row = 0; row < gameMap.GetLength(0); row++)
        {
            for (int col = 0; col < gameMap.GetLength(1); col++)
            {
                if (gameMap[row, col] != null)
                {
                    var roomActions = gameMap[row, col].GetComponent<RoomActions>();

                    if (col < gameMap.GetLength(1) - 1 && gameMap[row, col + 1] != null)
                    {
                        var roomActionsLeft = gameMap[row, col + 1].GetComponent<RoomActions>();
                        roomActions.roomLeft = roomActionsLeft;
                    }

                    if (col > 0 && gameMap[row, col - 1] != null)
                    {
                        var roomActionsRight = gameMap[row, col - 1].GetComponent<RoomActions>();
                        roomActions.roomRight = roomActionsRight;
                    }

                    if (row > 0 && gameMap[row - 1, col] != null)
                    {
                        var roomActionsAbove = gameMap[row - 1, col].GetComponent<RoomActions>();
                        roomActions.roomAbove = roomActionsAbove;
                    }

                    if (row < gameMap.GetLength(0) - 1 && gameMap[row + 1, col] != null)
                    {
                        var roomActionsBelow = gameMap[row + 1, col].GetComponent<RoomActions>();
                        roomActions.roomBelow = roomActionsBelow;
                    }
                }
            }
        }
    }
}
