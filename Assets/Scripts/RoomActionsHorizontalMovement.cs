using System.Collections;
using UnityEngine;

public abstract class RoomActionsHorizontalMovement : RoomActions
{
    public float moveLeftDistance;
    public float moveRightDistance;

    public override void ButtonLeft()
    {
        if (!playerController.canMove)
            return;    
        
        StartCoroutine(MoveCoroutine(new Vector3(-1, 0f, 0f), moveLeftDistance));
    }

    public override void ButtonRight()
    {
        if (!playerController.canMove)
            return;    
        
        StartCoroutine(MoveCoroutine(new Vector3( 1, 0f, 0f), moveRightDistance));
    }

    private IEnumerator MoveCoroutine(Vector3 direction, float distance)
    {
        playerController.canMove = false;
        
        var moveTime = distance / playerController.moveSpeed;
        var startPosition = player.transform.position;
        var targetPosition = startPosition + direction * distance;

        StartCoroutine(playerMovementController.MoveCoroutine(moveTime, startPosition, targetPosition));
        playerController.OrientPlayer(direction);
        playerController.playerX -= (int) direction.x;
        
        yield return new WaitForSeconds(playerController.cameraMovementDelay);
        yield return MoveCameraCoroutine(direction, distance);
        yield return new WaitForSeconds(playerController.cameraMovementDelay);

        playerController.canMove = true;
    }
    
    private IEnumerator MoveCameraCoroutine(Vector3 direction, float distance)
    {
        var moveTime = distance / playerController.moveSpeed;
        var startPosition = mainCamera.transform.position;
        var targetPosition = startPosition + direction * distance;

        yield return mainCameraMovementController.MoveCoroutine(moveTime, startPosition, targetPosition);
    }
}
