using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Chat : NetworkBehaviour {
    private Text chatText;
    private InputField chatInput;
    private Player player;
    [SyncVar]
    string message;

	// Use this for initialization
	void Start () {
        chatText = GameObject.Find("ChatText").GetComponent<Text>();
        chatInput = GameObject.Find("ChatInput").GetComponent<InputField>();
        player = transform.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        string tempMessage = chatInput.text;
        if (!string.IsNullOrEmpty(tempMessage.Trim()) && Input.GetKeyDown("return")){
            print("update");
            //PrintMessage(player.playerName + ": " + tempMessage + "\n");
            message = player.playerName + ": " + tempMessage + "\n";
            RpcPrintMessage(message);
            chatInput.text = "";
        }
	}

    //void PrintMessage(string message) {
    //    print("Printmessage");
    //    CmdPrintMessage(message);
    //}

    //[Command]
    //void CmdPrintMessage(string message) {
    //    print("CmdPrintmessage");
    //    RpcPrintMessage(message);
    //}

    [ClientRpc]
    void RpcPrintMessage(string message)
    {
        print("RPC");
        chatText.text += message;
    }
}
