using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

using AYS.Networking;
using AYS.Camera;

namespace AYS.Networking.Client {
    public class ImageRecieverClient : MonoBehaviour {
        [SerializeField] private ServerConfiguration serverConfig = null;

        private TcpClient tcpClient;
        private StreamReader streamReader;
    
        private bool _isConnected;

        // Start is called before the first frame update
        void Start() {
            tcpClient = new TcpClient();
            tcpClient.Connect(serverConfig.ipAddr, serverConfig.port);
            _isConnected = true;

            streamReader = new StreamReader(tcpClient.GetStream());

            StartCoroutine(SendCameraImagesToServer(1.0f));
        }

        void Update() {
            if (!_isConnected) {
                return; 
            }

            string line = null;

            while ((line = streamReader.ReadLine()) != null) {
                Debug.Log(line.Length);
            }
        }

        IEnumerator SendCameraImagesToServer(float waitTime) {
            DeviceCamera deviceCamera = DeviceCamera.instance;

            if (!deviceCamera._cameraIsAvailable) {
                yield return null;
            }

            WebCamTexture camera = deviceCamera._camera;
            
            while(camera.isPlaying) {
                if (_isConnected) {
                    NetworkingUtils.sendCurrentCameraFrameImage(tcpClient.GetStream(), camera);
                }

                yield return new WaitForSeconds(waitTime);
            }

        }
    }
}