using System;
using System.Collections;
using UnityEngine;

public abstract class RoomActions : MonoBehaviour
{
    protected GameObject player;
    protected PlayerController playerController;
    protected MovementController playerMovementController;
    
    protected GameObject mainCamera;
    protected MovementController mainCameraMovementController;

    [NonSerialized]
    public RoomActions roomLeft;
    [NonSerialized]
    public RoomActions roomRight;
    [NonSerialized]
    public RoomActions roomAbove;
    [NonSerialized]
    public RoomActions roomBelow;

    public GameObject leftDoor;
    public GameObject wallBothHoles;
    public GameObject wallLeftHole;
    public GameObject wallRightHole;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerMovementController = player.GetComponent<MovementController>();
        
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCameraMovementController = mainCamera.GetComponent<MovementController>();
    }

    public virtual void Start()
    {
        if (roomLeft == null)
        {
            leftDoor.SetActive(false);
            wallBothHoles.SetActive(false);
            wallRightHole.SetActive(true);
        } 
        else if (roomRight == null)
        {
            wallBothHoles.SetActive(false);
            wallLeftHole.SetActive(true);
        }
    }

    public abstract void ButtonUp();
    public abstract void ButtonDown();
    public abstract void ButtonLeft();
    public abstract void ButtonRight();
    
    protected IEnumerator MoveCameraCoroutine(Vector3 direction, float distance)
    {
        var moveTime = distance / playerController.moveSpeed;
        var startPosition = mainCamera.transform.position;
        var targetPosition = startPosition + direction * distance;

        yield return mainCameraMovementController.MoveCoroutine(moveTime, startPosition, targetPosition);
    }
}