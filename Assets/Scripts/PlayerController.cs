using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveVerticallyDistance = 5f; // distance the player moves vertically with a single button press
    public float moveHorizontallyDistance = 10f; // distance the player moves horizontally with a single button press
    public float moveSpeed = 10f; // speed of the player's movement
    public float fadeOutDuration = 0.5f; // duration for fading out
    public float fadeInDuration = 0.5f; // duration for fading in
    public AnimationCurve accelerationCurve; // curve for acceleration and deceleration

    private bool canMove = true; // flag to indicate if the player can currently move
    private bool isMoving = false; // flag to indicate if the player is currently moving
    private Vector3 targetPosition; // target position for the player's movement
    private Vector3 startPosition; // starting position for the player's movement
    private SpriteRenderer spriteRenderer; // reference to the sprite renderer component

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Check if the player can move and the arrow key is pressed
        if (canMove && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayer(Vector3.left, moveHorizontallyDistance, false);
        }
        else if (canMove && Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayer(Vector3.right, moveHorizontallyDistance, false);
        }
        else if (canMove && Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePlayer(Vector3.up, moveVerticallyDistance, true);
        }
        else if (canMove && Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayer(Vector3.down, moveVerticallyDistance, true);
        }
    }

    void MovePlayer(Vector3 direction, float distance, bool shouldFade)
    {
        // Set the target position based on the current position and the move distance
        startPosition = transform.position;
        targetPosition = startPosition + direction * distance;

        // Calculate the time needed to cover the distance
        float moveTime = distance / moveSpeed;

        // Disable further movement until the player reaches the target position
        canMove = false;
        isMoving = true;

        StartCoroutine(MoveAndFade(shouldFade, moveTime));
    }

    IEnumerator MoveAndFade(bool shouldFade, float moveTime)
    {
        if (shouldFade)
        {
            // Fade out
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

            // Apply acceleration and deceleration using the curve
            float speedMultiplier = accelerationCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, speedMultiplier);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the player's position to the target position
        transform.position = targetPosition;

        if (shouldFade)
        {
            // Fade in
            float timer = 0f;
            while (timer < fadeInDuration)
            {
                float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        // Reset the flags for movement
        canMove = true;
        isMoving = false;
    }
}
