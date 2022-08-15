/****************************************************

   文件：
   作者：WWS
   日期：2022/8/15 11:48:12
   功能：请求房间列表

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;



public class ListRoomRequest : BaseRequest 
{

    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        reqCode = ReqCode.Room;
        actionCode = ActionCode.ListRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }


   /// <summary>
   /// 
   /// </summary>
   /// <param name="data">房间列表信息</param>
    public override void OnResponse(string data)
    {
        List<UserData> userLst = new List<UserData>();
        if (data != "0")
        {
            string[] udArray = data.Split('|'); //看服务器的
            foreach (string ud in udArray)
            {
                string[] strs = ud.Split(',');

                #region 另一种写法
                /**  get set不能这样写
                UserData _userData = new UserData
                {
                    Id = int.Parse(strs[0]),
                    Username = strs[1],
                    TotalCount = int.Parse(strs[2]),
                    WinCount = int.Parse(strs[3])
                };
                //*/
                #endregion

                int id = int.Parse(strs[0]);
                string username = strs[1];
                int totalCnt = int.Parse(strs[2]);
                int winCnt = int.Parse(strs[3]);
                userLst.Add(new UserData( id, username, totalCnt, winCnt));
            }
        }
        
        roomListPanel.LoadLobbyAsync(userLst);
    }
}
