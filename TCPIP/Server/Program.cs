using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    class Program
    {
        static byte[] dataBuffer = new byte[102400];

        static void Main(string[] args)
        {
            Console.WriteLine("服务端：刘备");

           // Test_Sync();
            Test_StartServerAsync();

            Console.ReadKey();
        }



        #region 异步
  static void Test_StartServerAsync()
        {
            Socket serverSocket = Common.Socket_Server_New(Common.IP, Common.Port);
            Socket clientSocket = serverSocket.Accept();
            Common.Socket_Send("三弟务必守好徐州城", clientSocket);


            clientSocket.BeginReceive(dataBuffer, 0,1024, SocketFlags.None, ReceiveCallBack, clientSocket); //开始监听
        }


        static void ReceiveCallBack(IAsyncResult ar)
        {

   
            Socket clientSocket = ar.AsyncState as Socket;
            int count = clientSocket.EndReceive(ar);
            string msgStr= Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine("从客户端接收到数据："+ msgStr);

            clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);
        }


        #endregion  
      

        #region 同步
  static void Test_Sync()
        {
            Socket serverSocket = Common.Socket_Server_New(Common.IP, Common.Port);
            Socket clientSocket = serverSocket.Accept();
            Common.Socket_Send("天王盖地虎", clientSocket);
            //
            Console.WriteLine(Common.Socket_Get(clientSocket));


            Console.ReadKey();
            clientSocket.Close();
            serverSocket.Close();
        }
        #endregion  


    }


}
