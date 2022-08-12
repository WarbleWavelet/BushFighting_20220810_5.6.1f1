using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class QuitRoomRequest : BaseRequest {
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
        base.SendRequest("r");
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
