using System.Collections;
using UnityEngine;

public abstract class RoomActions : MonoBehaviour
{
    protected GameObject player;
    protected PlayerController playerController;
    protected MovementController playerMovementController;
    
    protected GameObject mainCamera;
    protected MovementController mainCameraMovementController;

    public RoomActions roomLeft;
    public RoomActions roomRight;
    public RoomActions roomAbove;
    public RoomActions roomBelow;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerMovementController = player.GetComponent<MovementController>();
        
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCameraMovementController = mainCamera.GetComponent<MovementController>();
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