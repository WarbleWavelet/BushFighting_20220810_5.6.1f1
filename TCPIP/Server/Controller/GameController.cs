/****************************************************

	文件：
	作者：WWS
	日期：2022/08/16 12:28:46
	功能：控制游戏开始

*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using GameServer.Servers;
namespace GameServer.Controller
{

 
    class GameController:BaseController
    {
        public GameController()
        {
            requestCode = ReqCode.Game;
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string StartGame(string data, Client client, Server server)
        {
            if (client.IsHouseOwner())//房主才有资格开启游戏
            {
                Room room =  client.Room;
                room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString());
                room.StartTimer();

                return  ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }


        public string QuitBattle(string data, Client client, Server server)
        {
            Room room = client.Room;

            if (room != null)
            {
                room.BroadcastMessage(null, ActionCode.QuitBattle, "r");
                room.Close();
            }
            return null;
        }

        public string Move(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
            {
               room.BroadcastMessage(client, ActionCode.Move, data);
            }
                
            return null;
        }
        public string Shoot(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
            {
                room.BroadcastMessage(client, ActionCode.Shoot, data);
            }
               
            return null;
        }
        public string Attack(string data, Client client, Server server)
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if (room == null)
            {
               return null;
            } 
            room.TakeDamage(damage, client);
            return null;
        }

    }
}
