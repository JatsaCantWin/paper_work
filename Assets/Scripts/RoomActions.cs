using UnityEngine;

public abstract class RoomActions : MonoBehaviour
{
    protected GameObject player;
    protected PlayerController playerController;
    protected MovementController playerMovementController;
    protected GameObject mainCamera;
    protected MovementController mainCameraMovementController;

    
    private void Start()
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
}