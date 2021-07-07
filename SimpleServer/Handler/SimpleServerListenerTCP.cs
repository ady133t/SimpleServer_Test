using SimpleServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    public abstract class SimpleServerListenerTCP
    {

        public SocketEntity socketEntity;

        public abstract void onConnected();

        public abstract void onDisconnected();

        public abstract void onMsgReceived(byte[] bytes, int offset, int size);

        public abstract void onFileReceived(byte[] bytes, int offset, int size);



        public void setEntity(SocketEntity socketEntity)
        {
            this.socketEntity = socketEntity;

        }

    }
}
