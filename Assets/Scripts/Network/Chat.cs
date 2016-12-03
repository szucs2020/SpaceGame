using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Chat : NetworkBehaviour {
    private GameObject chatArea;
    private Text chatText;
    private InputField chatInput;
    private Player player;
    private GameObject chatPanel;
    private List<GameObject> convo;

	// Use this for initialization
	void Start () {
        chatArea = GameObject.Find("ChatArea");
        chatPanel = (GameObject)Resources.Load("ChatPanel");
        chatInput = GameObject.Find("ChatInput").GetComponent<InputField>();
        convo = new List<GameObject>();
        player = transform.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        string message = chatInput.text;
        if (!string.IsNullOrEmpty(message.Trim()) && Input.GetKeyDown("return")){
            GameObject chatBubble = Instantiate(chatPanel);
            chatBubble.transform.SetParent(chatArea.transform, false);
            chatText = chatBubble.transform.GetChild(0).GetComponent<Text>();
            chat(message);
            chatInput.text = "";
            convo.Add(chatBubble);
        }

        int i = 1;
        foreach (GameObject chatBubble in convo) {
            chatBubble.transform.position = chatInput.transform.position + new Vector3(0, 27*i, 0);
            i++;
        }
	}

    public void chat(string message) {
        CmdChat(message);
    }

    [Command]
    void CmdChat(string message) {
        RpcChat(message);
    }

    [ClientRpc]
    void RpcChat(string message) {
        chatText.text += "  " + player.playerName + ": ";
        chatText.text += message;
        chatText.text += "\n";
    }
}
