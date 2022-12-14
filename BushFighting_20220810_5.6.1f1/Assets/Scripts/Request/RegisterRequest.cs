/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:13:00
	功能：注册

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class RegisterRequest : BaseRequest 
{

   
    private RegisterPanel registerPanel;

    public override void Awake()
    {                                
        reqCode = ReqCode.User;
        actionCode = ActionCode.Register;
        registerPanel = GetComponent<RegisterPanel>();
        base.Awake();
    }

    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        registerPanel.OnRegisterResponse(returnCode);

    }
}
