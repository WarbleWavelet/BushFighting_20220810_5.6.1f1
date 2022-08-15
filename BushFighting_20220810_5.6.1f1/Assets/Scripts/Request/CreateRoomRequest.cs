/****************************************************
	文件：CreateRoomRequest.cs
	作者：WWS
	邮箱: 
	日期：2022/08/14 18:10   	
	功能：创建房间请求
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using System;

public class CreateRoomRequest :BaseRequest 
{

    private RoomPanel roomPanel;

    public override void Awake()
    {
        reqCode = ReqCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public void SetPanel( BasePanel panel)
    {
        roomPanel = panel as RoomPanel;
    }

    public override void SendRequest()
    {
        base.SendRequest("r");  //随便的r
    }

    /// <summary>
    /// 不属于主线程
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        RoleType roleType = (RoleType)int.Parse(strs[1]);
        facade.SetCurrentRoleType(roleType);
        if (returnCode == ReturnCode.Success)
        {
            roomPanel.SetLocalPlayerResAsync();
        }
    }
}
