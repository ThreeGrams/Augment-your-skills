using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using AYS.Networking;

namespace AYS.Networking.Server {
    public class Server : MonoBehaviour {
        [SerializeField] private ServerConfiguration serverConfig = null;

        private TcpListener _server;
        private bool _isRunning;

        
        // Start is called before the first frame update
        void Start(){
            _server = new TcpListener(IPAddress.Any, serverConfig.port);
            _server.Start();
            _isRunning = true;
        }

        // Update is called once per frame
        void Update() {
            if(!_isRunning) {
                return;
            }

            TcpClient newClient = _server.AcceptTcpClient();

            Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
            t.Start(newClient);
        }

        public void HandleClient(object obj) {
            TcpClient client = (TcpClient)obj;
    
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
    
            string sData = null;
    
            while (true) {
                if (!IsConnected(client)){
                    break;
                }

                sData = sReader.ReadLine();
    
                Debug.Log("Prijal jsem" + sData);
    
                //sWriter.WriteLine(" Kapesnicek: " + sData);
                //sWriter.Flush();


            }
        }

        public bool IsConnected(TcpClient tcpClient) {
            try {
                if (tcpClient != null && tcpClient.Client != null && tcpClient.Client.Connected) {
                    // Detect if client disconnected
                    if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            // Client disconnected
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch {
                return false;
            }
        }
    }
}