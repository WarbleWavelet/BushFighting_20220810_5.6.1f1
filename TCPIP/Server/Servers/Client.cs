using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using GameServer.Model;
using GameServer.DAO;
using Protocol;
namespace GameServer.Servers
{
    class Client
    {


        #region 字属构造
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
        /// <summary>Client构造时传进来</summary> 
        private MySqlConnection mysqlConn;
        private Room room;
        private User user;
        private Result result;                      
        private ResultDAO resultDAO = new ResultDAO();

        public int HP
        {
            get;set;
        }

        /// <summary>Client构造时传进来</summary> 
        public MySqlConnection MySQLConn
        {
            get { return mysqlConn; }
        }

        public Client() { }
        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mysqlConn = ConnHelper.Connect();
        }

        public Room Room
        {
            set { room = value; }
            get { return room; }
        }
        #endregion




        #region 生命
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
               return;
            }
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

         /// <summary>
         /// 关闭Client
         /// </summary>
        private void Close()
        {
            ConnHelper.CloseConnection(mysqlConn);
            if (clientSocket != null)
            {
                  clientSocket.Close();
            }
              
            if (room != null) //有房退房，房主散房
            {
                room.QuitRoom(this);
            }
            server.RemoveClient(this);
        }


        #endregion



        #region Msg
        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                clientSocket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }
        }

        /// <summary>
        /// 收消息的回调
        /// </summary>
        /// <param name="ar"></param>
          private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                {
                     return;
                } 
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }


        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="ReqCode">请求码</param>
        /// <param name="actionCode">动作码</param>
        /// <param name="data"></param>
        private void OnProcessMessage(ReqCode ReqCode,ActionCode actionCode,string data)
        {
            server.HandleRequest(ReqCode, actionCode, data, this);
        }

        #endregion  







        #region Room
        public bool IsHouseOwner()
        {
            return room.IsHouseOwner(this);
        }
        #endregion


        #region UpdateResult
        public void UpdateResult(bool isVictory)
        {
            UpdateResultToDB(isVictory);
            UpdateResultToClient();
        }


        private void UpdateResultToDB(bool isVictory)
        {
            result.TotalCount++;
            if (isVictory)
            {
                result.WinCount++;
            }
            resultDAO.UpdateOrAddResult(mysqlConn, result);
        }


        private void UpdateResultToClient()
        {
            Send(ActionCode.UpdateResult, string.Format("{0},{1}", result.TotalCount, result.WinCount));
        }
        #endregion

      


        #region 说明
        #region 战斗
        public bool TakeDamage(int damage)
        {
            HP -= damage;
            HP = Math.Max(HP, 0);
            if (HP <= 0) return true;
            return false;
        }
        public bool IsDie()
        {
            return HP <= 0;
        }
        #endregion



        #region user
        public void SetUserData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }

        /// <summary>
        ///  user.Id可做房间Id 
        /// </summary>
        /// <returns></returns>
        public string GetUserData()
        {
            return user.Id + "," + user.Username + "," + result.TotalCount + "," + result.WinCount;
        }


        /// <summary>
        /// 获取GetUserId
        /// </summary>
        /// <returns></returns>
        public int GetUserId()
        {
            return user.Id;
        }
        #endregion
        #endregion

     

    }
}
