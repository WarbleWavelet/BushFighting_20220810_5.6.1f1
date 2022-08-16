using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using System.Reflection;
using GameServer.Servers;
namespace GameServer.Controller
{

    class ControllerManager
    {
        private Dictionary<ReqCode, BaseController> controllerDic = new Dictionary<ReqCode, BaseController>();
        private Server server;

        public ControllerManager(Server server) {
            this.server = server;
            InitController();
        }

        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDic.Add(defaultController.RequestCode, defaultController);
            controllerDic.Add(ReqCode.User, new UserController());
            controllerDic.Add(ReqCode.Room, new RoomController());
            controllerDic.Add(ReqCode.Game, new GameController());
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
            BaseController controller;
            bool isGet = controllerDic.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("无法得到[" + requestCode + "]所对应的Controller,无法处理请求");
                return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[警告]在Controller["+controller.GetType()+"]中没有对应的处理方法:["+methodName+"]");
                return;
            }
            object[] parameters = new object[] { data,client,server };
            object o = mi.Invoke(controller, parameters);
            if(o==null||string.IsNullOrEmpty( o as string))
            {
                return;
            }
            server.SendResponse(client, actionCode, o as string);
        }

    }
}
