using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class RegisterRequest : BaseRequest//注册 
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
