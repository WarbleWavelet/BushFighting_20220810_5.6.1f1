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
        static void Main(string[] args)
        {
            Console.WriteLine("服务端");


            Socket serverSocket =  Common.Socket_Server_New(Common.IP, Common.Port);
            Socket clientSocket = serverSocket.Accept();
            Common.Socket_Send("天王盖地虎", clientSocket);
            //
            Console.WriteLine(Common.Socket_Get(clientSocket));


            Console.ReadKey();
            clientSocket.Close();
            serverSocket.Close();
        }




 






    }


}
