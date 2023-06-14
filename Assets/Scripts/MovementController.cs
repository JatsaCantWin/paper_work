using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public AnimationCurve accelerationCurve;
    
    public IEnumerator MoveCoroutine(float moveTime, Vector3 startPosition, Vector3 targetPosition)
    {
        var elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            var t = elapsedTime / moveTime;

            var speedMultiplier = accelerationCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, speedMultiplier);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
