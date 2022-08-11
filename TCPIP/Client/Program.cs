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
            //Test12_Async();
            //Test21_SubPack_Int();
           Test21_SubPack_String();
        }

        /// <summary>
        /// 粘包
        /// </summary>
        static void Test21_SubPack_Int()
        {

            Socket clientSocket = Common.Socket_Client_New(Common.IP, Common.Port);
            Console.WriteLine("收到" + Common.Socket_Get(clientSocket)); //收
            for (int i = 0; i <100 ; i++)
            {
                Common.Socket_Send(clientSocket, i);
                Common.Socket_Send(clientSocket, " ");
            }

            Console.ReadKey();
            clientSocket.Close();
        }            
        
        /// <summary>
        /// 粘包
        /// </summary>
        static void Test21_SubPack_String() 
        {

            Socket clientSocket = Common.Socket_Client_New(Common.IP, Common.Port);
            Console.WriteLine("收到" + Common.Socket_Get(clientSocket)); //收
            string s = "fhwduikfhjksfghuisdfhu的撒谎的十多个哈斯需不需ZHJ报电子版巴萨这个点哈斯必须总不能V型没ZB一可师傅好是127";
            string str="";
            for (int i = 0; i <100 ; i++)
            {
                str += s;
            }
            Common.Socket_Send(clientSocket, str);



            Console.ReadKey();
            clientSocket.Close();
        }

        static void Test21_StickPack()//Stick黏
        {

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
