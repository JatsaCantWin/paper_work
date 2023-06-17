using System;
using System.Collections;
using UnityEngine;

public class RoomActionsOffice : RoomActionsHorizontalMovement
{
    public GameObject responseOK;
    public GameObject responseNO;
    public float stampWaitTime = 0.5f;
    
    private bool _playerVisited = false;
    private StampManager _stampManager;

    public override void Start()
    {
        base.Start();
        _stampManager = GameObject.FindWithTag("Canvas").GetComponent<StampManager>();
    }

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

        GameObject response;
        
        if (_playerVisited)
        {
            response = responseNO;
        }
        else
        {
            response = responseOK;
            _stampManager.getStamp();
            _playerVisited = true;
        }
        
        response.transform.localScale = Vector3.one * 0.01f;

        response.SetActive(true);

        var scaleSpeed = 1.0f / stampWaitTime;

        while (response.transform.localScale.x < 1.0f)
        {
            response.transform.localScale += Vector3.one * Time.deltaTime * scaleSpeed;
            yield return null;
        }

        response.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(stampWaitTime);

        response.transform.localScale = Vector3.one * 0.01f;

        response.SetActive(false);
        
        playerController.canMove = true;
    }

}
