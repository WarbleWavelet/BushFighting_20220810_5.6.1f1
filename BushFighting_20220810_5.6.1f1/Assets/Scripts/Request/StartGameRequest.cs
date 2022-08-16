/****************************************************

	文件：
	作者：WWS
	日期：2022/08/16 12:31:23
	功能： 请求开始游戏

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;



public class StartGameRequest : BaseRequest
{
    private RoomPanel roomPanel;
    public override void Awake()
    {
        reqCode = ReqCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        roomPanel.OnStartResponse(returnCode);
    }

}
