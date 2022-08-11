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


    /// <summary>
    /// 大小统一
    /// </summary>
    /// <param name="cnt"></param>
    /// <returns></returns>
    public static byte[] String2Bin(int cnt=10000)
    {
        byte[] data = BitConverter.GetBytes(cnt);//字符串

        return data;
    }


    public static string Bin2StringAsync(Socket socket)
    {
        byte[] data = new byte[1024];
        int count = socket.Receive(data);

        return System.Text.Encoding.UTF8.GetString(data, 0, count);
    }


    public static string Socket_Get(Socket socket)
    {
        return Bin2String(socket);
    }

    /// <summary>
    /// from发送msg到...
    /// </summary>
    /// <param name="from"></param>
    /// <param name="msg"></param>
    public static void Socket_Send<T>(Socket from, T msg)
    {
        
        from.Send(Common.String2Bin(msg.ToString()));
    }

    /// <summary>
    /// ...发送msg到to
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="to"></param>
    public static void Socket_Send(string msg, Socket to)
    {
        to.Send(Common.String2Bin(msg));
    }


}

