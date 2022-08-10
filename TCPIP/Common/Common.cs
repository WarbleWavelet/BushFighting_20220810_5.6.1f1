using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class Common
{
    public const string IP = "127.0.0.1";
    public const int Port =88;

       public static Socket Socket_Server_New(string ip, int endPoint, int waitCnt = 0)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, endPoint);
            serverSocket.Bind(ipEndPoint);//绑定ip和端口号
            serverSocket.Listen(waitCnt);//开始监听端口号 


            return serverSocket;
        }


      public  static Socket Socket_Client_New(string ip, int endPoint, int waitCnt = 0)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, endPoint);

            clientSocket.Connect(ipEndPoint);//开始监听端口号 

            return clientSocket;
        }

        public static byte[] String2Bin(string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(data);
        }


        public static string Bin2String(Socket socket)
        {
            byte[] data = new byte[1024];
            int count = socket.Receive(data);

            return System.Text.Encoding.UTF8.GetString(data, 0, count);
        }




    public static string Socket_Get(Socket socket)
    {
        return Bin2String(socket);
    }


    public static void Socket_Send(Socket from, string msg)
    {
        from.Send(Common.String2Bin(msg));
    }


    public static void Socket_Send(string msg, Socket to)
    {
        to.Send(Common.String2Bin(msg));
    }


}

