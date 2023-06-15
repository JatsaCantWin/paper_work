using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject roomCorridor;
    public GameObject roomCorridorDoor;
    public GameObject roomOffice;
    
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
        GenerateRooms();
    }

    private void GenerateRooms()
    {
        roomWidth = roomWidth * scale;
        roomHeight = roomHeight * scale;

        for (int row = 0; row < roomArray.GetLength(0); row++)
        {
            for (int col = 0; col < roomArray.GetLength(1); col++)
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
}