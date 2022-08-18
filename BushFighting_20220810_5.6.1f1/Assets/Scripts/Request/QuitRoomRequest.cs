/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:13:26
	功能： 退出房间

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class QuitRoomRequest : BaseRequest
{

   

    private RoomPanel roomPanel;
    public override void Awake()
    {
        reqCode = ReqCode.Room;
        actionCode = ActionCode.QuitRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r"); //随便的r
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        if (returnCode == ReturnCode.Success)
        {
            roomPanel.OnExitResponse();
        }
    }
}
