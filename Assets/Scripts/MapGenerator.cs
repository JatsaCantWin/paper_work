using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject roomCorridor;
    public GameObject roomCorridorDoor;
    public GameObject roomOffice;

    public int numFloors;
    public int roomsPerFloor;

    public float spaceBetweenRooms;
    public float spaceBetweenFloors;
    public float scale;
    public float roomWidth;
    public float roomHeight;

    void Start()
    {
        GenerateRooms();
    }

    void GenerateRooms()
    {
        roomWidth = roomWidth * scale;
        roomHeight = roomHeight * scale;

        // Randomly generate room layout
        int[,] roomArray = GenerateRandomRoomLayout();

        for (int row = 0; row < numFloors; row++)
        {
            bool officePlaced = false;

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
                        officePlaced = true;
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

            // Ensure there is at least one roomOffice on each floor (excluding the bottom floor and rightmost room)
            if (!officePlaced && row != numFloors - 1 && !IsRightmostRoom(roomArray, row))
            {
                // Randomly select a column to place the roomOffice
                int randomCol = Random.Range(0, roomsPerFloor);
                roomArray[row, randomCol] = 30;

                float posX = randomCol * (roomWidth + spaceBetweenRooms);
                float posY = row * (roomHeight + spaceBetweenFloors);

                GameObject roomInstance = Instantiate(roomOffice, transform);
                roomInstance.transform.localPosition = new Vector3(posX, posY, 0f);
                roomInstance.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }

    bool IsRightmostRoom(int[,] roomArray, int row)
    {
        for (int col = roomsPerFloor - 1; col >= 0; col--)
        {
            if (roomArray[row, col] != 0)
                return false;
        }

        return true;
    }

    int[,] GenerateRandomRoomLayout()
    {
        int[,] roomLayout = new int[numFloors, roomsPerFloor];

        // Generate random room layout
        for (int row = 0; row < numFloors; row++)
        {
            for (int col = 0; col < roomsPerFloor; col++)
            {
                if (row == 0)
                {
                    // Ensure the first row has corridor rooms
                    roomLayout[row, col] = (col % 2 == 0) ? 10 : 20;
                }
                else
                {
                    // Generate random room type for subsequent rows
                    int roomType = Random.Range(0, 3) * 10;
                    roomLayout[row, col] = roomType;
                }
            }
        }

        // Ensure there is at least one corridor door on each floor
        for (int row = 1; row < numFloors; row++)
        {
            bool corridorDoorExists = false;

            for (int col = 0; col < roomsPerFloor; col++)
            {
                if (roomLayout[row, col] == 20)
                {
                    corridorDoorExists = true;
                    break;
                }
            }

            if (!corridorDoorExists)
            {
                // Randomly select a column to place a corridor door
                int randomCol = Random.Range(0, roomsPerFloor);
                roomLayout[row, randomCol] = 20;
            }
        }

        // Ensure there are no 'none' rooms below or above another room
        for (int row = 1; row < numFloors; row++)
        {
            for (int col = 0; col < roomsPerFloor; col++)
            {
                if (roomLayout[row, col] == 0)
                {
                    // Check if there are adjacent rooms in the row above or below
                    bool hasAdjacentRooms = false;

                    if (row > 0 && roomLayout[row - 1, col] != 0)
                        hasAdjacentRooms = true;

                    if (row < numFloors - 1 && roomLayout[row + 1, col] != 0)
                        hasAdjacentRooms = true;

                    if (hasAdjacentRooms)
                    {
                        // Generate a random room type other than 'none'
                        int roomType = Random.Range(1, 4) * 10;
                        roomLayout[row, col] = roomType;
                    }
                }
            }
        }

        return roomLayout;
    }
}
