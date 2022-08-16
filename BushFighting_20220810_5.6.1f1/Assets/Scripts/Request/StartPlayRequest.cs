using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class StartPlayRequest : BaseRequest {

    private bool isStartPlaying = false;

    public override void Awake()
    {
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }

    private void Update()
    {
        if (isStartPlaying)
        {
            return;
            facade.StartPlaying();
            isStartPlaying = false;
        }
    }

    public override void OnResponse(string data)
    {
        isStartPlaying = true;
    }
}
