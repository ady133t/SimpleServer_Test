using SimpleServer.Handler;
using SimpleServer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleServer
{
    public abstract class SimpleServerTCP
    {
        private TcpListener tcpListener = null;
        public List<SocketEntity> entityList = new List<SocketEntity>();

        public SimpleServerTCP(string ip, int port)
        {

            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Convert.ToInt16(port));

            this.tcpListener = new TcpListener(localEndPoint);

   
            //test// push local branch 12

        }


        public void Start<T>() where T : new()
        {
            Thread thdListener = new Thread(() => { //thread 1 only for accepting socket

            this.tcpListener.Start();

            while (true)
            {

                Socket clientSocket = tcpListener.AcceptSocket();

                if (clientSocket.Connected)
                {
                        SocketEntity socketEntity = null;

                    lock (entityList)
                    {
                        socketEntity = new SocketEntity(){ ID = Guid.NewGuid().ToString("N"), socket = clientSocket};

                        //listener.setEntity(socketEntity);
                        entityList.Add(socketEntity);
                            //adding the object to the list

                            onClientConnected(socketEntity);
                    }
                        // ParameterizedThreadStart a = new ParameterizedThreadStart(handlerThread(Socket socket, SimpleServerListenerTCP listener));
                        object listenerObj = (object)new T();
                        SimpleServerListenerTCP listener = (SimpleServerListenerTCP)listenerObj;
                        listener.setEntity(socketEntity);


                        Thread handlerTh = new Thread(()=> handlerThread(listener)); //each socket will have their own handler thread
                        handlerTh.Name = socketEntity.ID;
                        handlerTh.Start();




                    }

                   
                }


            });

            thdListener.Start();
            

        }

        public abstract void onClientConnected(SocketEntity socketEntity);
  



        public void handlerThread(SimpleServerListenerTCP listener)
        {
            

            Thread thisThread = Thread.CurrentThread;
            NetworkStream networkStream = new NetworkStream(listener.socketEntity.socket);
            int thisRead = 0;



            listener.onConnected();

           // BackgroundWorker bw = new BackgroundWorker();
           // bw.DoWork += bw_DoWork;
           // object[] param = new object[]{listener, thisThread};
           // bw.RunWorkerAsync(param);


            Thread heartbeatThread = new Thread(() =>
            { // start new thread to listen for heatbeat

              
                while (true)
                {

                    if (!IsConnected(listener.socketEntity.socket)) // the hearbeat 
                    {

                        lock (entityList)
                        {
                            listener.onDisconnected();
                            entityList.Remove(listener.socketEntity);
                            thisThread.Abort();
                           
                        }

                        return;

                    }

                    Thread.Sleep(100);
                
                }

            });

            heartbeatThread.IsBackground = true;
            heartbeatThread.Start();



            bool foundProtocol = false;
            

            while (true) //listener for receiving data
            {

                int blockSize = 1024;
                Byte[] dataByte = new Byte[blockSize];
               

                try
                {


                    thisRead = networkStream.Read(dataByte, 0, blockSize);

                    if (thisRead > 0)
                    {


                        if (!foundProtocol)
                        {



                            for (int dataBytePos = 0; dataBytePos < dataByte.Length; dataBytePos++)
                            {
                                byte[] bufferCursor = new byte[Protocol.Length];
                                int pos = 0;

                                for (int cursor = dataBytePos; (cursor < Protocol.Length + dataBytePos) && (cursor + Protocol.Length - 1 < dataByte.Length); cursor++)
                                    bufferCursor[pos++] = dataByte[cursor];

                                if (bufferCursor.SequenceEqual(Protocol.MsgHeader))
                                {
                                    Console.WriteLine("Found MessageHeader");
                                }

                                else if(bufferCursor.SequenceEqual(Protocol.FileHeader))
                                {
                                    Console.WriteLine("Found FileHeader");
                                }


                            }
                        }

                        else
                        {

                    

                        }
                    }


               
                }

                catch(Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                  
                    //lock (entityList)
                    //{

                    //    //listener.onDisconnected();
                    //    //entityList.Remove(listener.socketEntity);
                    //    thisThread.Abort();
                        
                    //}

                    return;

                }


            }

        }


        private void bw_DoWork(object  sender, DoWorkEventArgs e)
        {

            SimpleServerListenerTCP listener = (SimpleServerListenerTCP)((object[])e.Argument)[0];
            Thread handlerThread = (Thread)((object[])e.Argument)[1];

            int row = 0;
            while (handlerThread.IsAlive)
            {
              

                if (!IsConnected(listener.socketEntity.socket)) // the hearbeat 
                {

                    lock (entityList)
                    {

                        entityList.Remove(listener.socketEntity);
                        handlerThread.Abort();
                        listener.onDisconnected();
                    }

                    return;

                }

                Thread.Sleep(100);
                Console.WriteLine("child thread is running.." + row++);
            }
        }

        private bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

    }
}
