using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Network : NetworkBehaviour {

	private int reliableChannelID;
	private int maxConnections = 10;
	int socketId;
	int socketPort = 8888;

	// Use this for initialization
	void Start () {
		
		NetworkTransport.Init();
		ConnectionConfig config = new ConnectionConfig();
		reliableChannelID = config.AddChannel(QosType.Reliable);
		HostTopology top = new HostTopology(config, maxConnections);

		socketId = NetworkTransport.AddHost(top, socketPort);
		Debug.Log("Socket Opened with ID: " + socketId);
		Connect();

	}

	public void Connect(){

		int connectionID;

		byte error;
		connectionID = NetworkTransport.Connect(socketId, "localhost", socketPort, 0, out error);
		Debug.Log("Connected to server. ConnectionId: " + connectionID);

	}

	public void SendSocketMessage() {
		byte error;
		byte[] buffer = new byte[1024];
		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, "HelloServer");

		int bufferSize = 1024;

		NetworkTransport.Send(hostId, connectionId, myReliableChannelId, buffer, bufferSize, out error);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
