using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using AYS.Server;

public class Client : MonoBehaviour {
    private const int MAX_CONNECTION = 100;

    [SerializeField] ServerConfiguration serverConfig = null;

    private int hostId;

    private int reliableChannel;
    private int unreliableChannel;

    private int connectionId;

    private float connectionTime;
    private bool isConnected = false;
    private byte error;

    private void Start() {
        Connect();       
    }

    private void Update() {
        Send("Send nudes");
    }

    private void Send(string message) {
        if (!isConnected) {
            return;
        }    

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);

        NetworkTransport.Send(hostId, connectionId, reliableChannel, buffer, 1024, out error);

        if ((NetworkError) error != NetworkError.Ok) {
            Debug.Log("Error: " + (NetworkError)error);
        }
    }

    private void Connect() {
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);
        
        NetworkTransport.Init();

        hostId = NetworkTransport.AddHost(topo, serverConfig.port);

        connectionId = NetworkTransport.Connect(hostId, serverConfig.ipAddr, serverConfig.port, 0, out error);

        if ((NetworkError) error != NetworkError.Ok) {
            Debug.Log("Error: " + (NetworkError)error);
        }

        connectionTime = Time.time;
        isConnected = true;
    }
}
