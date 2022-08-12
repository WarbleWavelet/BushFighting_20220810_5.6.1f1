using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using GameServer.Servers;
namespace GameServer.Controller
{
    abstract class BaseController
    {
        protected ReqCode requestCode = ReqCode.None;

        public ReqCode RequestCode {
            get
            {
                return requestCode;
            }
        }

        public virtual string DefaultHandle(string data,Client client,Server server) { return null; }
    }
}
