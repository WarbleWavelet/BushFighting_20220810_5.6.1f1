/****************************************************

	文件：
	作者：WWS
	日期：2022/8/15 14:45:34
	功能：加入房间

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;




public class JoinRoomRequest : BaseRequest {

    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        reqCode = ReqCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }



    public override void OnResponse(string data)
    {
        string[] strs = data.Split('-'); //"returncode,roletype-id,username,tc,wc|id,username,totalCnt,winCnt"
        string[] strs2 = strs[0].Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs2[0]);
        UserData ud1 = null;
        UserData ud2 = null;
        if (returnCode == ReturnCode.Success)          
        {
            string[] udStrArray = strs[1].Split('|');
            ud1 = new UserData(udStrArray[0]);//一队
            ud2 = new UserData(udStrArray[1]); //另一队

            RoleType roleType = (RoleType)int.Parse(strs2[1]);
            facade.SetCurrentRoleType(roleType);
        }
        roomListPanel.OnJoinResponse(returnCode, ud1, ud2);
    }
}
