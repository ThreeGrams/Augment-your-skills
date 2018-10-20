using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

using AYS.Networking;

namespace AYS.Networking.Client {
    public class Client : MonoBehaviour {
        [SerializeField] private ServerConfiguration serverConfig = null;

        private AysTcpClient tcpClient;
    
        private StreamReader _sReader;
        private StreamWriter _sWriter;
    
        private bool _isConnected;

        // Start is called before the first frame update
        void Start() {
            tcpClient = new AysTcpClient(serverConfig.ipAddr, serverConfig.port);   
            _isConnected = true;
        }

        // Update is called once per frame
        void Update() {
            if (!_isConnected) {
                return;
            }

            tcpClient.sendString("Křemen");
        }
    }
}