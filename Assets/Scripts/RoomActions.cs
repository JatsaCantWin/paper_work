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

    protected GameObject clock;
    protected TimeManager timeManager;
    
    public float timeCostUp;
    public float timeCostDown;
    public float timeCostLeft;
    public float timeCostRight;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerMovementController = player.GetComponent<MovementController>();
        
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCameraMovementController = mainCamera.GetComponent<MovementController>();

        clock = GameObject.FindWithTag("Clock");
        timeManager = clock.GetComponent<TimeManager>();
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

    public virtual void ButtonUp()
    {
        timeManager.AddTime(timeCostUp);
    }

    public virtual void ButtonDown()
    {
        timeManager.AddTime(timeCostDown);
    }

    public virtual void ButtonLeft()
    {
        timeManager.AddTime(timeCostLeft);
    }

    public virtual void ButtonRight()
    {
        timeManager.AddTime(timeCostRight);
    }
    
    protected IEnumerator MoveCameraCoroutine(Vector3 direction, float distance)
    {
        var moveTime = distance / playerController.moveSpeed;
        var startPosition = mainCamera.transform.position;
        var targetPosition = startPosition + direction * distance;

        yield return mainCameraMovementController.MoveCoroutine(moveTime, startPosition, targetPosition);
    }
}