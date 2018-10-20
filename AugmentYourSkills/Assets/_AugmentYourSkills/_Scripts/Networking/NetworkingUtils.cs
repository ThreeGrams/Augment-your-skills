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

        public static byte[] Color32ArrayToByteArray(Color32[] colors) {
            if (colors == null || colors.Length == 0) {
                return null;
            }

            int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
            int length = lengthOfColor32 * colors.Length;
            byte[] bytes = new byte[length];

            GCHandle handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                Marshal.Copy(ptr, bytes, 0, length);
            }
            finally
            {
                if (handle != default(GCHandle))
                    handle.Free();
            }

            return bytes;
        }

        public static Color32[] ByteArrayToColor32Array(byte[] data) {
            if (data == null || data.Length == 0) {
                return null;
            }
            
            Color32[] colorArray = new Color32[data.Length/4];
            for(int i = 0; i < data.Length; i+=4)
            {
                var color = new Color32(data[i + 0], data[i + 1], data[i + 2],data[i + 3]);
                colorArray[i/4] = color;
            }

            return colorArray;
        }

        public static void sendBytes(NetworkStream networkStream, byte[] data) {
            networkStream.Write(data, 0, data.Length);
        }

        public static void sendString(NetworkStream networkStream, string message) {
            sendBytes(networkStream, Encoding.UTF8.GetBytes(message));
        }

        public static void sendCurrentCameraFrameImage(NetworkStream networkStream, WebCamTexture camera) {
            Color32[] pixelData;
            pixelData = new Color32[camera.width * camera.height];

            camera.GetPixels32(pixelData);
            sendBytes(networkStream, NetworkingUtils.Color32ArrayToByteArray(pixelData));
        }

        public static byte[] readBytes(NetworkStream networkStream) {
            byte[] data = new byte[bytesLength];
            networkStream.Read(data, 0, bytesLength);
            return data;
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
