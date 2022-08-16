/****************************************************

	文件：
	作者：WWS
	日期：2022/8/15 17:7:13
	功能：管理房间

*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using GameServer.Servers;



namespace GameServer.Controller
{

  
    class RoomController:BaseController
    {
        public RoomController()
        {
            requestCode = ReqCode.Room;
        }


        #region 房间的增删查改
        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string CreateRoom(string data, Client client, Server server)
        {
            server.CreateRoom(client);
            return ((int)ReturnCode.Success).ToString()+","+ ((int)RoleType.Home).ToString();
        }


        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string QuitRoom(string data, Client client, Server server)
        {
            bool isHouseOwner = client.IsHouseOwner();
            Room room = client.Room;
            if (isHouseOwner)
            {
                room.BroadcastMessage(client, ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                client.Room.RemoveClient(client);
                room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomData());
                return ((int)ReturnCode.Success).ToString();
            }
        }


       /// <summary>
       /// 大厅展示房间
       /// </summary>
       /// <param name="data"></param>
       /// <param name="client"></param>
       /// <param name="server"></param>
       /// <returns></returns>
        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Room room in server.GetRoomList())
            {
                if (room.IsWaitingJoin())
                {
                    sb.Append(room.GetHouseOwnerData()+"|"); //房主+1
                }
            }

            if (sb.Length == 0)//非等待
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1); //去掉多出来的"|"                       
            }
            return sb.ToString();
        }


        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string JoinRoom(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            Room room = server.GetRoomById(id);
            if(room == null)
            {
                return ((int)ReturnCode.NotFound).ToString();
            }
            else if (room.IsWaitingJoin() == false)//非等待
            {
                return ((int)ReturnCode.Fail).ToString(); //满员了
            }
            else//成功
            {
                room.AddClient(client);
                string roomData = room.GetRoomData();//"returncode,roletype-id,username,tc,wc|id,username,tc,wc"
                room.BroadcastMessage(client, ActionCode.UpdateRoom, roomData);
                return ((int)ReturnCode.Success).ToString() + "," + ((int)RoleType.Away).ToString()+ "-" + roomData;
            }
        }
        #endregion
       



    }
}
