using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

using AYS.Networking;
using AYS.Camera;

namespace AYS.Networking.Client {
    public class ImageRecieverClient : MonoBehaviour {
        [SerializeField] private ServerConfiguration serverConfig = null;
        [SerializeField] private RawImage background = null;

        private TcpClient tcpClient;
        private StreamReader streamReader;

        // Start is called before the first frame update
        void Start() {
            tcpClient = new TcpClient();
            tcpClient.ConnectAsync(serverConfig.ipAddr, serverConfig.port);
        }

        void Update() {
            if (!tcpClient.Connected) {
                return; 
            }

            Texture2D tex = new Texture2D(2,2);
            NetworkingUtils.readJPGImage(tcpClient.GetStream(), tex);
            background.texture = tex;
        }

        private void OnApplicationQuit() {
            tcpClient.Close();
        }
    }
}