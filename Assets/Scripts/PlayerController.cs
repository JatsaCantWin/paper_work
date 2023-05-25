using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveVerticallyDistance = 5f;
    public float moveHorizontallyDistance = 10f;
    public float keyPressSensitivity = 1.25f;
    public float moveSpeed = 10f;
    public float fadeOutDuration = 0.5f;
    public float fadeInDuration = 0.5f;
    public AnimationCurve accelerationCurve;

    private bool canMove = true;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ProcessInput()
    {
        float horizontalMovement = Mathf.RoundToInt(Input.GetAxis("Horizontal") * keyPressSensitivity); // Will equal -1 when left and 1 when right
        float verticalMovement   = Mathf.RoundToInt(Input.GetAxis("Vertical") * keyPressSensitivity);   // Will equal -1 when down and 1 when up
        
        if (canMove)
        {
            if (horizontalMovement != 0)
            {
                MovePlayer(new Vector3(horizontalMovement, 0f, 0f), moveHorizontallyDistance, false);
            }

            if (verticalMovement != 0)
            {
                MovePlayer(new Vector3(0f, verticalMovement, 0f), moveVerticallyDistance, true);
            }
        }
    }

    private void Update()
    {
        ProcessInput();
    }

    void MovePlayer(Vector3 direction, float distance, bool shouldFade)
    {
        startPosition = transform.position;
        targetPosition = startPosition + direction * distance;

        float moveTime = distance / moveSpeed;

        canMove = false;
        isMoving = true;

        StartCoroutine(MoveAndFade(shouldFade, moveTime));
    }

    IEnumerator MoveAndFade(bool shouldFade, float moveTime)
    {
        if (shouldFade)
        {
            float timer = 0f;
            while (timer < fadeOutDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            float t = elapsedTime / moveTime;

            float speedMultiplier = accelerationCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, speedMultiplier);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        if (shouldFade)
        {
            float timer = 0f;
            while (timer < fadeInDuration)
            {
                float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        canMove = true;
        isMoving = false;
    }
}
