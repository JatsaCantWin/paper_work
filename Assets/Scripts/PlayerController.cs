using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveVerticallyDistance = 5f;
    public float moveHorizontallyDistance = 10f;
    public float keyPressSensitivity = 1.25f;
    public float moveSpeed = 10f;
    public float fadeOutDuration = 0.5f;
    public float fadeInDuration = 0.5f;

    private bool _canMove = true;
    private FadeController _fadeController;
    private MovementController _movementController;

    private void Start()
    {
        _fadeController = GetComponent<FadeController>();
        _movementController = GetComponent<MovementController>();
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

        yield return _movementController.MoveCoroutine(moveTime, startPosition, targetPosition);

        if (vertical)
        {
            _fadeController.FadeIn(fadeOutDuration);
            yield return _fadeController.WaitForFadeCoroutine();
        }
        
        _canMove = true;
    }
}
