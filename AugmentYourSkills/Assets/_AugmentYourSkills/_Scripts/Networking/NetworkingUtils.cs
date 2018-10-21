using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

using UnityEngine;

namespace AYS.Networking {
    public class NetworkingUtils { 
        private const int bytesLength = 307000;

        

        public static void sendBytes(NetworkStream networkStream, byte[] data) {
            networkStream.Write(data, 0, data.Length);
        }

        public static void sendString(NetworkStream networkStream, string message) {
            sendBytes(networkStream, Encoding.UTF8.GetBytes(message));
        }

        public static void sendCurrentCaptureScreenshotImage(NetworkStream networkStream) {
            Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
            sendBytes(networkStream, tex.EncodeToJPG());
        }

        public async static void readBytes(NetworkStream networkStream, byte[] data) {
            data = new byte[bytesLength];
            await networkStream.ReadAsync(data, 0, bytesLength);
            return;
        }

        public static void readJPGImage(NetworkStream networkStream, Texture2D tex) {
            byte[] data = new byte[bytesLength];
            readBytes(networkStream, data);

            tex = new Texture2D(1, 1);
            tex.LoadImage(data);

            return;
        }

        public static bool IsConnected(TcpClient tcpClient) {
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
