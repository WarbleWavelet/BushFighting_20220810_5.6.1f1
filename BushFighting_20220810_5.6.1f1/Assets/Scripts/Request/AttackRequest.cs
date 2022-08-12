using System;
using System.Collections.Generic;
using Protocol;
using UnityEngine;


public class AttackRequest:BaseRequest
{
    public override void Awake()
    {
        reqCode = ReqCode.Game;
        actionCode = ActionCode.Attack ;
        base.Awake();
    }
    public void SendRequest(int damage)
    {
        base.SendRequest(damage.ToString());
    }
}

