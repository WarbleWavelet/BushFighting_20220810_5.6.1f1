﻿using System;
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
            Console.WriteLine("客户端");

            Socket clientSocket = Common.Socket_Client_New(Common.IP, Common.Port);


            Console.WriteLine("收到" + Common.Socket_Get(clientSocket)); //收


            string data = Console.ReadLine(); //需要拆出来写
            Common.Socket_Send(clientSocket, data);          //发

            clientSocket.Close();
        }

    }
}
