/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:15:15
	功能：攻击

*****************************************************/

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

