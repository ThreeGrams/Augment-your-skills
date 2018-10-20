using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AYS.Server {
	public class Server : MonoBehaviour {
		private const int MAX_CLIENT_CONNECTIONS = 1000;

		private int reliableChannel;
		private int unreliableChannel;

		private int hostId;
		private int webHostId;

		[SerializeField] private ServerConfiguration _serverConfig = null;

		// Start is called before the first frame update
		void Start() {
			NetworkTransport.Init();
			ConnectionConfig connectionConfig = new ConnectionConfig();

			reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
			unreliableChannel = connectionConfig.AddChannel(QosType.Unreliable);

			HostTopology hostTopology = new HostTopology(connectionConfig, MAX_CLIENT_CONNECTIONS);

			hostId = NetworkTransport.AddHost(hostTopology, _serverConfig.port, _serverConfig.ipAddr);
			webHostId = NetworkTransport.AddWebsocketHost(hostTopology, _serverConfig.port, _serverConfig.ipAddr);
		}

		// Update is called once per frame
		void Update() {

		}
	}

}
