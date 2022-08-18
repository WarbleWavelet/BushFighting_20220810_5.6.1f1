/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:13:55
	功能： 登录

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class LoginRequest : BaseRequest 
{


  
    private LoginPanel loginPanel;
    public override void Awake()
    {
        reqCode = ReqCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }
    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        loginPanel.OnLoginResponse(returnCode);
        if (returnCode == ReturnCode.Success)
        {

            string username = strs[1];
            int totalCount = int.Parse(strs[2]);
            int winCount = int.Parse(strs[3]);
            UserData ud = new UserData(username, totalCount, winCount);
            facade.SetUserData(ud);
        }
    }

}
