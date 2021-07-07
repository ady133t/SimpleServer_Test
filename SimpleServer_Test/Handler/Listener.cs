using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleServer_Test.Handler
{
    class Listener : SimpleServer.SimpleServerListenerTCP
    {
        public override void onConnected()
        {

            Console.WriteLine(this.socketEntity.ID +  " Connected");
            //Console.WriteLine(Thread.CurrentThread.Name);
 

            //base.onConnected();
        }

        public override void onMsgReceived(byte[] bytes, int offset, int size)
        {
           Console.WriteLine(this.socketEntity.ID + " send : " + Encoding.UTF8.GetString(bytes).Trim('\0') + " : length=" + bytes.Length + " : Thread Name = " + Thread.CurrentThread.Name );
        }

        public override void onFileReceived(byte[] bytes, int offset, int size)
        {
            throw new NotImplementedException();
        }

        public override void onDisconnected()
        {
            Console.WriteLine(this.socketEntity.ID + " Disconnected");

        }
    }
}
