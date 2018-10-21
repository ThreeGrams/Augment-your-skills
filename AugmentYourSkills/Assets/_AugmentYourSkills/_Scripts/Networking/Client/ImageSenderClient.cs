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
    public class ImageSenderClient : MonoBehaviour {
        [SerializeField] private ServerConfiguration serverConfig = null;

        private TcpClient tcpClient;

        // Start is called before the first frame update
        void Start() {
            tcpClient = new TcpClient();
            tcpClient.ConnectAsync(serverConfig.ipAddr, serverConfig.port);

            StartCoroutine(SendCameraImagesToServer(1.0f));
        }

        private void OnApplicationQuit() {
            tcpClient.Close();
        }

        IEnumerator SendCameraImagesToServer(float waitTime) {
            DeviceCamera deviceCamera = DeviceCamera.instance;

            if (!deviceCamera._cameraIsAvailable) {
                yield return null;
            }

            WebCamTexture camera = deviceCamera._camera;
            
            while(camera.isPlaying) {
                if (tcpClient.Connected) {
                    NetworkingUtils.sendCurrentCaptureScreenshotImage(tcpClient.GetStream());
                }

                yield return new WaitForSeconds(waitTime);
            }

        }
    }
}