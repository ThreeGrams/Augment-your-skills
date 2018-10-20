using AYS.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace AYS.Server {
	public class Server : MonoBehaviour {
		private const int MAX_CLIENT_CONNECTIONS = 1000;

		private int _reliableChannel;
		private int _unreliableChannel;

		private int _hostId;
		private int _webHostId;

		private bool _isInitialized = false;

		private List<ClientOnServer> _clients = new List<ClientOnServer>();

		[SerializeField] private ServerConfiguration _serverConfig = null;

		// Start is called before the first frame update
		void Start() {
			NetworkTransport.Init();
			ConnectionConfig connectionConfig = new ConnectionConfig();

			_reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
			_unreliableChannel = connectionConfig.AddChannel(QosType.Unreliable);

			HostTopology hostTopology = new HostTopology(connectionConfig, MAX_CLIENT_CONNECTIONS);

			_hostId = NetworkTransport.AddHost(hostTopology, _serverConfig.port, _serverConfig.ipAddr
				);
			_webHostId = NetworkTransport.AddWebsocketHost(hostTopology, _serverConfig.port, _serverConfig.ipAddr);

			_isInitialized = true;
			Debug.Log("Server Initialization complete!");
		}

		// Update is called once per frame
		void Update() {
			if (!_isInitialized) {
				return;
			}

			int recievedHostId,
				connectionId,
				channelId,
				dataSize,
				bufferSize = 1024;
			byte error;
			byte[] recieveBuffer = new byte[bufferSize];
			NetworkEventType recieveData = NetworkTransport.Receive(out recievedHostId, out connectionId, 
																	out channelId, recieveBuffer, 
																	bufferSize, out dataSize, out error);

			switch(recieveData) {
				case NetworkEventType.Nothing:
					break;
				case NetworkEventType.ConnectEvent:
					onConnectEvent(connectionId);
					break;
				case NetworkEventType.DataEvent:
					string data = Encoding.UTF8.GetString(recieveBuffer, 0, dataSize);
					Debug.LogFormat("Player {0} sent: {1}!", connectionId, data);
					break;
				case NetworkEventType.DisconnectEvent:
					Debug.LogFormat("Player {0} has disconnected!", connectionId);
					break;
				default:
					return;
			}
				
		}

		private void onConnectEvent(int connectionId) {
			Debug.LogFormat("Player {0} has connected!", connectionId);
			ClientOnServer client = new ClientOnServer();
			client.connectionId = connectionId;
			client.playerName = "Name-uh";
			_clients.Add(client);
		}
	}

}
