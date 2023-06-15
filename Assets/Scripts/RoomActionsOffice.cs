using System.Collections;
using UnityEngine;

public class RoomActionsOffice : RoomActionsHorizontalMovement
{
    public float stampWaitTime = 0.5f;
    private bool _playerVisited = false;

    public override void ButtonDown(){}

    public override void ButtonUp()
    {
        if (!playerController.canMove)
            return;

        StartCoroutine(AddStamp());
    }
    
    private IEnumerator AddStamp()
    {
        playerController.canMove = false;

        if (_playerVisited)
        {
            Debug.Log("PlayerAlreadyVisited");
        }
        else
        {
            Debug.Log("PlayerVisited");
            _playerVisited = true;
        }

        yield return new WaitForSeconds(stampWaitTime);

        playerController.canMove = true;
    } 
}
