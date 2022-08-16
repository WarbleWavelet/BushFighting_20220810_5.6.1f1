using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using GameServer.Controller;
using Protocol;
namespace GameServer.Servers
{
    class Server
    {

        #region 字属构造
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private List<Room> roomList = new List<Room>();
        private ControllerManager controllerManager;

        public Server() {}
        public Server(string ipStr,int port)
        {
            controllerManager = new ControllerManager(this);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }
        #endregion

        #region 生命
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        private void AcceptCallBack(IAsyncResult ar  )
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket,this);
            client.Start();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }
        #endregion  


        public void SendResponse(Client client,ActionCode actionCode,string data)
        {
            client.Send(actionCode, data);
        }    


        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }




        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public void HandleRequest(ReqCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }

        #region Room
        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
        }
        
        /// <summary>
        /// 移除房间
        /// </summary>
        /// <param name="room"></param>
        public void RemoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }

        public List<Room> GetRoomList()
        {
            return roomList;
        }

      /// <summary>
       /// 得到该Id的Room
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public Room GetRoomById(int id)
        {
            foreach(Room room in roomList)
            {
                if (room.GetId() == id)
                {
                    return room;
                }
            }
            return null;
        }
        #endregion  
      
    }
}
