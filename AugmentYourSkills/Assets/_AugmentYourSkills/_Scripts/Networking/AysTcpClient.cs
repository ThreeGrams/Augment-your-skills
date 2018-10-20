using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AYS.Networking {
    public class AysTcpClient : TcpClient {
        private StreamReader reader;

        public AysTcpClient(string ipAddr, int port) : base() {
            this.Connect(ipAddr, port);

            reader = new StreamReader(this.GetStream(), Encoding.UTF8);
        }

        public AysTcpClient(IPAddress ipAddr, int port) : base() {
            this.Connect(ipAddr, port);
        }

        public void sendBytes(byte[] data) {
            this.GetStream().Write(data, 0, data.Length);
        }

        public void sendString(string message) {
            sendBytes(Encoding.UTF8.GetBytes(message));
        }

        public void sendImage() {
            
        }
    }
}
