using System.Collections;
using UnityEngine;

public class RoomActionsStairs : RoomActionsHorizontalMovement
{
    public float moveUpwardDistance;
    public float moveDownwardDistance;

    private FadeController _fadeController;
    
    private void Start()
    {
        _fadeController = player.GetComponent<FadeController>();
    }
    
    public override void ButtonUp()
    {
        if (!playerController.canMove)
            return;    
        
        if (playerController.currentRoom.roomAbove is null)
            return;

        StartCoroutine(MoveCoroutine(new Vector3(0f, -1f, 0f), moveUpwardDistance));
    }

    public override void ButtonDown()
    {
        if (!playerController.canMove)
            return;    
        
        if (playerController.currentRoom.roomBelow is null)
            return;
        
        StartCoroutine(MoveCoroutine(new Vector3( 0f, 1f, 0f), moveDownwardDistance));
    }
    
    private IEnumerator MoveCoroutine(Vector3 direction, float distance)
    {
        playerController.canMove = false;
    
        _fadeController.FadeOut(playerController.stairMovementFadeDuration);
        yield return _fadeController.WaitForFadeCoroutine();

        var moveTime = distance / playerController.moveSpeed;
        var startPosition = player.transform.position;
        var targetPosition = startPosition + direction * distance;

        playerController.currentRoom = direction.y > 0 ? roomBelow : roomAbove;
        
        StartCoroutine(playerMovementController.MoveCoroutine(moveTime, startPosition, targetPosition));
        yield return MoveCameraCoroutine(direction, distance);
        
        _fadeController.FadeIn(playerController.stairMovementFadeDuration);
        yield return _fadeController.WaitForFadeCoroutine();

        playerController.canMove = true;
    }
}

