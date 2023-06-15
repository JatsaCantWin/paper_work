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
        {10, 20, 0, 0, 0}
    };

    public GameObject[,] gameMap = new GameObject[2,5];
    
    void Start()
    {
        GenerateRooms();
    }
    
    void GenerateRooms()
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
    }
}