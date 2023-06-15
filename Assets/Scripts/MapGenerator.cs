using UnityEngine;

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

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
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
                }
            }
        }
    }

    void GenerateCorridorDoorRooms(int[,] roomArray)
    {
        for (int row = 0; row < numFloors; row++)
        {
            int randomCol = Random.Range(0, roomsPerFloor);
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
        for (int row = 0; row < numFloors; row++)
        {
            int randomCol = Random.Range(0, roomsPerFloor);
            int roomType = 30;
            roomArray[row, randomCol] = roomType;
        }
    }
}
