/****************************************************

	文件：
	作者：WWS
	日期：2022/8/15 16:29:1
	功能：更新房间

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;


public class UpdateRoomRequest : BaseRequest 
{
    private RoomPanel roomPanel;
    public override void Awake()
    {
        reqCode = ReqCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        UserData udHome = null;//主队
        UserData udAway = null;//客队
        string[] udStrArray = data.Split('|');//"returncode,roletype-id,username,tc,wc|id,username,tc,wc"
        udHome = new UserData(udStrArray[0]);
        if (udStrArray.Length > 1)
        {
            udAway = new UserData(udStrArray[1]);
        }
        roomPanel.SetAllPlayerResAsync(udHome, udAway);
    }
}
