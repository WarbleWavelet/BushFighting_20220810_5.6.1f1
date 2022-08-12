using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class CreateRoomRequest :BaseRequest {

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
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        RoleType roleType = (RoleType)int.Parse(strs[1]);
        facade.SetCurrentRoleType(roleType);
        if (returnCode == ReturnCode.Success)
        {
            roomPanel.SetLocalPlayerResSync();
        }
    }
}
