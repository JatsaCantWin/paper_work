using System;
using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerController : MonoBehaviour
{
    public float moveVerticallyDistance = 5f;
    public float moveHorizontallyDistance = 10f;
    public float keyPressSensitivity = 1.25f;
    public float moveSpeed = 10f;
    public float cameraMovementDelay = 0.08f;
    public float fadeOutDuration = 0.5f;
    public float fadeInDuration = 0.5f;

    private bool _canMove = true;
    private FadeController _fadeController;
    private MovementController _movementController;
    private GameObject _mainCamera;
    private MovementController _mainCameraMovementController;
    
    private void Start()
    {
        _fadeController = GetComponent<FadeController>();
        _movementController = GetComponent<MovementController>();
        _mainCamera = GameObject.FindWithTag("MainCamera");
        _mainCameraMovementController = _mainCamera.GetComponent<MovementController>();
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (!_canMove)
            return;
        
        float horizontalMovement = Mathf.RoundToInt(Input.GetAxis("Horizontal") * keyPressSensitivity); // Will equal -1 when left and 1 when right
        if (horizontalMovement != 0)
        {
            StartCoroutine(MoveCoroutine(new Vector3(horizontalMovement, 0f, 0f), moveHorizontallyDistance, false));
        }
        
        float verticalMovement   = Mathf.RoundToInt(Input.GetAxis("Vertical") * keyPressSensitivity);   // Will equal -1 when down and 1 when up
        if (verticalMovement != 0)
        {
            StartCoroutine(MoveCoroutine(new Vector3(0f, verticalMovement, 0f), moveVerticallyDistance, true));
        }
    }

    private void OrientPlayer(Vector3 direction)
    {
        var currentRotation = transform.eulerAngles;
        var rotationAngle = 90 - (direction.x * 90);

        transform.eulerAngles = new Vector3(currentRotation.x, rotationAngle, currentRotation.z);
    }

    private IEnumerator MoveCoroutine(Vector3 direction, float distance, bool vertical)
    {
        _canMove = false;
        
        if (vertical)
        {
            _fadeController.FadeOut(fadeInDuration);
            yield return _fadeController.WaitForFadeCoroutine();
        }

        var moveTime = distance / moveSpeed;
        var startPosition = transform.position;
        var targetPosition = startPosition + direction * distance;

        StartCoroutine(_movementController.MoveCoroutine(moveTime, startPosition, targetPosition));

        if (!vertical)
        {
            OrientPlayer(direction);
            yield return new WaitForSeconds(cameraMovementDelay);
        }

        yield return MoveCameraCoroutine(direction, distance);
        
        if (vertical)
        {
            _fadeController.FadeIn(fadeOutDuration);
            yield return _fadeController.WaitForFadeCoroutine();
        }
        
        if (!vertical)
        {
            yield return new WaitForSeconds(cameraMovementDelay);
        }
        
        _canMove = true;
    }

    private IEnumerator MoveCameraCoroutine(Vector3 direction, float distance)
    {
        var moveTime = distance / moveSpeed;
        var startPosition = _mainCamera.transform.position;
        var targetPosition = startPosition + direction * distance;

        yield return _mainCameraMovementController.MoveCoroutine(moveTime, startPosition, targetPosition);
    }
}
