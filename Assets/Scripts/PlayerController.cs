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
    public RoomActions currentRoom;

    private GameObject _gameMap;
    private MapGenerator _mapGenerator;

    private void Start()
    {
        _gameMap = GameObject.FindWithTag("GameMap");
        _mapGenerator = _gameMap.GetComponent<MapGenerator>();
        currentRoom = _mapGenerator.gameMap[0, 0].GetComponent<RoomActions>();
    }

    private void Update()
    {
        ProcessInput();
    }
    
    private void ProcessInput()
    {
        if (!canMove)
        {
            return;
        }

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        if (horizontalMovement == -1)
        {
            currentRoom.ButtonLeft();
        }
        else if (horizontalMovement == 1)
        {
            currentRoom.ButtonRight();
        }

        float verticalMovement = Input.GetAxisRaw("Vertical");
        if (verticalMovement == -1)
        {
            currentRoom.ButtonUp();
        }
        else if (verticalMovement == 1)
        {
            currentRoom.ButtonDown();
        }
    }
    
    public void OrientPlayer(Vector3 direction)
    {
        var currentRotation = transform.eulerAngles;
        var rotationAngle = 90 - (direction.x * 90);

        transform.eulerAngles = new Vector3(currentRotation.x, rotationAngle, currentRotation.z);
    }
}
