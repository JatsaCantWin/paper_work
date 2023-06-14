using System;
using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerController : MonoBehaviour
{
    public GameObject gameMap;
    public float moveVerticallyDistance = 5f;
    public float keyPressSensitivity = 1.25f;
    public float moveSpeed = 10f;
    public float cameraMovementDelay = 0.08f;

    public bool canMove = true;

    public int playerX = 0;
    public int playerY = 0;

    private void Start()
    {
        gameMap = GameObject.FindWithTag("GameMap");
    }

    private void Update()
    {
        ProcessInput();
    }
    
    private void ProcessInput()
    {
        Debug.Log(playerX + " " + playerY);
        var currentRoomActions = gameMap.GetComponent<MapGenerator>().gameMap[playerX, playerY].GetComponent<RoomActions>();
        
        float horizontalMovement = Mathf.RoundToInt(Input.GetAxis("Horizontal") * keyPressSensitivity); // Will equal -1 when left and 1 when right
        if (horizontalMovement < 0)
        {
            currentRoomActions.ButtonLeft();
        }
        else if (horizontalMovement > 0)
        {
            currentRoomActions.ButtonRight();
        }
        
        float verticalMovement   = Mathf.RoundToInt(Input.GetAxis("Vertical") * keyPressSensitivity);   // Will equal -1 when down and 1 when up
        if (verticalMovement != 0)
        {
            //StartCoroutine(MoveCoroutine(new Vector3(0f, verticalMovement, 0f), moveVerticallyDistance, true));
        }
    }
    
    public void OrientPlayer(Vector3 direction)
    {
        var currentRotation = transform.eulerAngles;
        var rotationAngle = 90 - (direction.x * 90);

        transform.eulerAngles = new Vector3(currentRotation.x, rotationAngle, currentRotation.z);
    }
}
