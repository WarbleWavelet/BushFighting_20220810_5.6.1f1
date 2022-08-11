using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("客户端：张飞");
           // Test01_Sync();
           // Test11_Async();
            Test12_Async();

        }





        static void Test12_Async()
        {
            Socket clientSocket = Common.Socket_Client_New(Common.IP, Common.Port);


            Console.WriteLine("收到" + Common.Socket_Get(clientSocket)); //收

            while (true)
            {
                string data = Console.ReadLine();
                if (data == "c") //按c，客户端自行关闭
                {
                    clientSocket.Close();
                    return;
                }
                Common.Socket_Send(clientSocket, data);
            }


            Console.ReadKey();
            clientSocket.Close();
        }

        static void Test11_Async()
        {
            Socket clientSocket = Common.Socket_Client_New(Common.IP, Common.Port);


            Console.WriteLine("收到" + Common.Socket_Get(clientSocket)); //收

            while(true)
            { 
                string data = Console.ReadLine(); 
                Console.Write(data); 
                Common.Socket_Send(clientSocket, data); 
            }


            Console.ReadKey();
            clientSocket.Close();
        }


        static void Test01_Sync()
        {
            Socket clientSocket = Common.Socket_Client_New(Common.IP, Common.Port);


            Console.WriteLine("收到" + Common.Socket_Get(clientSocket)); //收


            string data = Console.ReadLine(); //需要拆出来写
            Common.Socket_Send(clientSocket, data);          //发

            clientSocket.Close();
        }

    }
}
