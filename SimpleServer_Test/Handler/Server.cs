using SimpleServer;
using SimpleServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimpleServer_Test.Handler
{
    public class Server : SimpleServerTCP
    {
        
        public Server(string ip, int port) : base(ip,port)
        {

           
        }



        public override void onClientConnected(SocketEntity socketEntity)
        {
          
        }
    }
}
