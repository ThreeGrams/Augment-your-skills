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

        private int lastRoomId = 0;
        private Dictionary<int, Room> roomPool;

        // Start is called before the first frame update
        void Start(){
            _server = new TcpListener(IPAddress.Any, serverConfig.port);
            _server.Start();
            _isRunning = true;

            roomPool = new Dictionary<int, Room>();
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
            Debug.Log("New user joined channel");

            User user = new User((TcpClient)obj);

            if (lastRoomId == 0 || roomPool[lastRoomId].isFull()) {
                Debug.Log("Creating new room");
                Room newRoom = new Room();
                roomPool.Add(++lastRoomId, newRoom);
            }

            roomPool[lastRoomId].assign(user);

            Room lastRoom = roomPool[lastRoomId];
       
            TcpClient tcpClient = user.GetClient();
            while (true) {
                if (!lastRoom.isReady()) {
                    continue;
                }

                if (!NetworkingUtils.IsConnected(tcpClient)){
                    break;
                }

                NetworkStream networkStream = tcpClient.GetStream();
                byte[] data = new byte[0];
                
                NetworkingUtils.readBytes(networkStream, data);

                networkStream = lastRoom.assignedUsers[user.getId() % 2].GetClient().GetStream();
                NetworkingUtils.sendBytes(networkStream, data);
            }
        }

        public class User {
            private Room assignedRoom;
            private TcpClient client;
            private int ID;

            public User(TcpClient client) {
                this.client = client;
            }     

            public TcpClient GetClient() {
                return client;
            } 

            public void setId(int Id) {
                this.ID = Id;
            }   

            public int getId() {
                return ID;
            }  

            public void setAssignedRoom(Room room) {
                this.assignedRoom = room;
            }

            public Room getAssignedRoom() {
                return assignedRoom;
            }
        }

        public class Room {
            public User[] assignedUsers = new User[2];
            private bool ready;

            public void assign(User user) {
                if (assignedUsers[0] == null) {
                    assignedUsers[0] = user;
                    user.setId(0);
                } else {
                    assignedUsers[1] = user;
                    user.setId(1);
                    ready = true;
                }
                user.setAssignedRoom(this);
            }
            
            public bool isFull() {
                return assignedUsers[1] != null;
            } 

            public bool isReady() {
                return ready;
            }
        }
    }
}