using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Protocol;

namespace Test_Server
{
    public class Program
    {
        static byte[] dataBuffer = new byte[102400];


        static void Main(string[] args)
        {
            Console.WriteLine("服务端：刘备");

            // Test01_Sync();
            //Test11_StartServerAsync();
            // Test12_StartServerAsync();
            Test21_StartServerAsync();

            Console.ReadKey();
        }



        #region Msg统一数据长度
        public static Msg msg = new Msg();
        /// <summary>
        /// 采用Msg
        /// </summary>
        static void Test21_StartServerAsync()
        {
            Socket serverSocket = Common.Socket_Server_New(Common.IP, Common.Port);

            serverSocket.BeginAccept(Test21_AcceptCallBack, serverSocket); //开始监听


        }

        static void Test21_AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            Common.Socket_Send("三弟务必守好徐州城", clientSocket);


            Socket_BeginReceive(msg, clientSocket, Test21_ReceiveCallBack);
            serverSocket.BeginAccept(Test21_AcceptCallBack, serverSocket); //开始监听

        }



        static void Test21_ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count <= 0)//正常关闭
                {
                    clientSocket.Close();
                    return;
                }


                msg.Read(count);

                Socket_BeginReceive(msg, clientSocket, Test21_ReceiveCallBack);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)//出现异常，关闭连接
                {
                    clientSocket.Close();
                }
            }
            finally
            {


            }
        }

        /// <summary>
        /// 容易敲
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="to"></param>
        /// <param name="ar"></param>
        static void Socket_BeginReceive(Msg msg, Socket to, System.AsyncCallback ar)
        {
            Socket clientSocket = to;//为了命名能复制
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize(), SocketFlags.None, ar, clientSocket);
        }
        #endregion


        #region 异步 2
        static void Test12_StartServerAsync()
        {
            Socket serverSocket = Common.Socket_Server_New(Common.IP, Common.Port);

            serverSocket.BeginAccept(Test12_AcceptCallBack, serverSocket); //开始监听


        }




        static void Test12_AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            Common.Socket_Send("三弟务必守好徐州城", clientSocket);


            clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, Test12_ReceiveCallBack, clientSocket);
            serverSocket.BeginAccept(Test12_AcceptCallBack, serverSocket); //开始监听

        }

        static void Test12_ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {

                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count <= 0)//正常关闭
                {
                    clientSocket.Close();
                    return;
                }
                string msgStr = Encoding.UTF8.GetString(dataBuffer, 0, count);
                Console.WriteLine("从客户端接收到数据：" + msgStr);

                clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, Test12_ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)//出现异常，关闭连接
                {
                    clientSocket.Close();
                }
            }
            finally
            {


            }
        }
        #endregion  

        #region 异步 1
        static void Test11_StartServerAsync()
        {
            Socket serverSocket = Common.Socket_Server_New(Common.IP, Common.Port);
            Socket clientSocket = serverSocket.Accept();
            Common.Socket_Send("三弟务必守好徐州城", clientSocket);


            clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, Test11_ReceiveCallBack, clientSocket); //开始监听
        }


        static void Test11_ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            int count = clientSocket.EndReceive(ar);
            string msgStr = Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine("从客户端接收到数据：" + msgStr);

            clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, Test11_ReceiveCallBack, clientSocket);
        }


        #endregion


        #region 同步
        static void Test01_Sync()
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
