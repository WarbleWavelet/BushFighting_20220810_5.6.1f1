using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using System.Threading;

namespace GameServer.Servers
{

    /// <summary>
    /// 房间状态
    /// </summary>
    enum RoomState
    {
        WaitingJoin,//等待队友
        WaitingBattle,//加载战斗中
        Battle,//战斗中
        End
    }
    class Room
    {


        #region 字属 构造
        private const int MAX_HP = 200;
        private List<Client> clientLst = new List<Client>();
        private RoomState state = RoomState.WaitingJoin;
        private Server server;

        public Room(Server server)
        {
            this.server = server;
        }
        #endregion


        /// <summary>
        /// 等待人员
        /// </summary>
        /// <returns></returns>
        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }

        public void AddClient(Client client)
        {
            client.HP = MAX_HP;
            clientLst.Add(client);
            client.Room = this;
            if (clientLst.Count>= 2)
            {
                state = RoomState.WaitingBattle;
            }
        }
        public void RemoveClient(Client client)
        {
            client.Room = null;
            clientLst.Remove(client);

            if (clientLst.Count >= 2)
            {
                state = RoomState.WaitingBattle;
            }
            else
            {
                state = RoomState.WaitingJoin;
            }
        }

        /// <summary>
        /// 房主
        /// </summary>
        /// <returns></returns>
        public string GetHouseOwnerData()
        {
            return clientLst[0].GetUserData();
        }
        

        /// <summary>
        /// 房间Id 房主Id 
        /// </summary>
        /// <returns>-1 不存在</returns>
        public int GetId()
        {
            if (clientLst.Count > 0)
            {
                return clientLst[0].GetUserId();
            }
            return -1;
        }


        /// <summary>
        /// 客户端战绩<para />
        /// "returncode,roletype-id,username,tc,wc|id,username,tc,wc"<para />
        /// </summary>
        /// <returns></returns>
        public String GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Client client in clientLst)
            {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        ///  广播消息给其他client
        /// </summary>
        /// <param name="excludeClient">排除掉的</param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        public void BroadcastMessage(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach(Client client in clientLst)
            {
                if (client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }


        public bool IsHouseOwner(Client client)
        {
            return client == clientLst[0];
        }


        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="client"></param>
        public void QuitRoom(Client client)
        {
            if (client.IsHouseOwner())//房主嘉会解散房间
            {
                Close();
            }
            else
            {
                clientLst.Remove(client);
            }
               
        }


        /// <summary>
        /// 解散房间
        /// </summary>
        public void Close()
        {
            foreach(Client client in clientLst) //赶人
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }




        #region 战斗
  public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }


        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessage(null, ActionCode.StartPlay, "r");
        }


        public void TakeDamage(int damage,Client excludeClient)
        {
            bool isDie = false;
            foreach (Client client in clientLst)
            {
                if (client != excludeClient)
                {
                    if (client.TakeDamage(damage))
                    {
                        isDie = true;
                    }
                }
            }
            if (isDie == false)
            {
                 return;
            }
            foreach (Client client in clientLst)  
            {
                if (client.IsDie()) //如果其中一个角色死亡，要结束游戏
                {
                    client.UpdateResult(false);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                }
                else
                {
                    client.UpdateResult(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
            }
            Close();
        }
        #endregion
      
    }
}
