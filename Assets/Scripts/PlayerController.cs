using System;
using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float keyPressSensitivity = 1.25f;
    
    public float moveSpeed = 10f;
    public float cameraMovementDelay = 0.08f;
    public float stairMovementFadeDuration = 0.5f;

    public bool canMove = true;
    public int playerX = 0;
    public int playerY = 0;

    private GameObject _gameMap;
    private MapGenerator _mapGenerator;

    private void Start()
    {
        _gameMap = GameObject.FindWithTag("GameMap");
        _mapGenerator = _gameMap.GetComponent<MapGenerator>();
    }

    private void Update()
    {
        ProcessInput();
    }
    
    private void ProcessInput()
    {
        var currentRoomActions = _mapGenerator.gameMap[playerY, playerX].GetComponent<RoomActions>();
        
        float horizontalMovement = Mathf.RoundToInt(Input.GetAxis("Horizontal") * keyPressSensitivity); // Will equal -1 when left and 1 when right
        switch (horizontalMovement)
        {
            case -1:
                currentRoomActions.ButtonLeft();
                break;
            case 1:
                currentRoomActions.ButtonRight();
                break;
        }
        
        float verticalMovement   = Mathf.RoundToInt(Input.GetAxis("Vertical") * keyPressSensitivity);   // Will equal -1 when down and 1 when up
        switch (verticalMovement)
        {
            case -1:
                currentRoomActions.ButtonUp();
                break;
            case 1:
                currentRoomActions.ButtonDown();
                break;
        }
    }
    
    public void OrientPlayer(Vector3 direction)
    {
        var currentRotation = transform.eulerAngles;
        var rotationAngle = 90 - (direction.x * 90);

        transform.eulerAngles = new Vector3(currentRotation.x, rotationAngle, currentRotation.z);
    }
}
