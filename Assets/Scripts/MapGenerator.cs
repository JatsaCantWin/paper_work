using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public GameObject roomCorridor;
    public GameObject roomCorridorDoor;
    public GameObject roomOffice;

    public int numFloors = 5;
    public int roomsPerFloor = 5;

    public float spaceBetweenRooms;
    public float spaceBetweenFloors;
    public float scale;
    public float roomWidth;
    public float roomHeight;
    
    public int[,] roomArray = {
        {10, 20, 20, 30, 30},
        {10, 20, 30, 0, 0}
    };

    public GameObject[,] gameMap = new GameObject[2,5];
    
    private void Awake()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        int[,] roomArray = new int[numFloors, roomsPerFloor];

        GenerateCorridorDoorRooms(roomArray);
        GenerateRemainingRooms(roomArray);
        GenerateOfficeRooms(roomArray);

        roomWidth = roomWidth * scale;
        roomHeight = roomHeight * scale;

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
                    case 0:
                        break;
                    case 10:
                        roomPrefab = roomCorridor;
                        break;
                    case 20:
                        roomPrefab = roomCorridorDoor;
                        break;
                    case 30:
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

    void GenerateCorridorDoorRooms(int[,] roomArray)
    {
        int prevRandomCol = -1;

        // Generate corridorDoor rooms for each row
        for (int row = 0; row < numFloors; row++)
        {
            int randomCol;
            do
            {
                randomCol = Random.Range(0, roomsPerFloor);
            } while (randomCol == prevRandomCol);

            prevRandomCol = randomCol;

            int roomType = 20;
            roomArray[row, randomCol] = roomType;
            EnsureAdjacentRoom(roomArray, row, randomCol, roomType);
        }

        // Ensure the top floor corridorDoor room has an adjacent room below
        int topFloorCol = Random.Range(0, roomsPerFloor);
        roomArray[numFloors - 1, topFloorCol] = 20;
        EnsureAdjacentRoom(roomArray, numFloors - 1, topFloorCol, 20);

        // Ensure the bottom floor corridorDoor room has an adjacent room above
        int bottomFloorCol = Random.Range(0, roomsPerFloor);
        roomArray[0, bottomFloorCol] = 20;
        EnsureAdjacentRoom(roomArray, 0, bottomFloorCol, 20);

        // Ensure corridorDoor rooms in the middle have adjacent corridorDoor rooms above or below
        for (int row = 1; row < numFloors - 1; row++)
        {
            for (int col = 0; col < roomsPerFloor; col++)
            {
                if (roomArray[row, col] == 20)
                {
                    bool hasAdjacentCorridorRoomAbove = (roomArray[row - 1, col] == 20);
                    bool hasAdjacentCorridorRoomBelow = (roomArray[row + 1, col] == 20);

                    if (!hasAdjacentCorridorRoomAbove && !hasAdjacentCorridorRoomBelow)
                    {
                        if (Random.value < 0.5f)
                            roomArray[row - 1, col] = 20; // Assign corridorDoor room above
                        else
                            roomArray[row + 1, col] = 20; // Assign corridorDoor room below
                    }
                    else if (!hasAdjacentCorridorRoomAbove)
                    {
                        roomArray[row - 1, col] = 20; // Assign corridorDoor room above
                    }
                    else if (!hasAdjacentCorridorRoomBelow)
                    {
                        roomArray[row + 1, col] = 20; // Assign corridorDoor room below
                    }
                }
            }
        }

        // Ensure there are no entire columns consisting of CorridorDoor rooms
        for (int col = 0; col < roomsPerFloor; col++)
        {
            int corridorDoorCount = 0;

            for (int row = 0; row < numFloors; row++)
            {
                if (roomArray[row, col] == 20)
                    corridorDoorCount++;

                // If the column already has corridorDoorCount >= numFloors, exit the loop
                if (corridorDoorCount >= numFloors)
                    break;
            }

            if (corridorDoorCount >= numFloors)
            {
                // Clear all rooms in the column
                for (int row = 0; row < numFloors; row++)
                    roomArray[row, col] = 0;

                // Randomly assign a CorridorDoor room in the column
                int randomRow = Random.Range(0, numFloors);
                roomArray[randomRow, col] = 20;
                EnsureAdjacentRoom(roomArray, randomRow, col, 20);
            }
        }

        // Ensure there is at least one CorridorDoor room in each row
        for (int row = 0; row < numFloors; row++)
        {
            if (!ContainsCorridorDoorRoom(roomArray, row))
            {
                int randomCol = Random.Range(0, roomsPerFloor);
                roomArray[row, randomCol] = 20;
                EnsureAdjacentRoom(roomArray, row, randomCol, 20);
            }
        }
    }

    bool ContainsCorridorDoorRoom(int[,] roomArray, int row)
    {
        for (int col = 0; col < roomsPerFloor; col++)
        {
            if (roomArray[row, col] == 20)
                return true;
        }
        return false;
    }

    void EnsureAdjacentRoom(int[,] roomArray, int row, int col, int roomType)
    {
        if (row > 0)
        {
            int roomAbove = roomArray[row - 1, col];
            if (roomAbove == 0)
                roomArray[row - 1, col] = roomType;
        }

        if (row < numFloors - 1)
        {
            int roomBelow = roomArray[row + 1, col];
            if (roomBelow == 0)
                roomArray[row + 1, col] = roomType;
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
        int officeRoomCount = 0;

        while (officeRoomCount < 5)
        {
            int randomRow = Random.Range(0, numFloors);
            int randomCol = Random.Range(0, roomsPerFloor);

            if (roomArray[randomRow, randomCol] != 20 && roomArray[randomRow, randomCol] != 30)
            {
                bool hasAdjacentCorridorRoom = CheckAdjacentCorridorRoom(roomArray, randomRow, randomCol);
                if (!hasAdjacentCorridorRoom)
                {
                    roomArray[randomRow, randomCol] = 30; // Assign office room type
                    officeRoomCount++;
                }
            }
        }
    }

    bool CheckAdjacentCorridorRoom(int[,] roomArray, int row, int col)
    {
        // Check if there is a CorridorDoor room in the adjacent cells (above, below, left, right)
        if (row > 0 && roomArray[row - 1, col] == 20) // Check above
            return true;

        if (row < numFloors - 1 && roomArray[row + 1, col] == 20) // Check below
            return true;

        if (col > 0 && roomArray[row, col - 1] == 20) // Check left
            return true;

        if (col < roomsPerFloor - 1 && roomArray[row, col + 1] == 20) // Check right
            return true;

        return false;
    }

}
