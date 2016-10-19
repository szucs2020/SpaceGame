using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

    void Awake() {

        DontDestroyOnLoad(transform.gameObject);

        //Give the player's slot a reference to the player
        StartCoroutine("Setup");
    }

    public override void OnClientReady(bool readyState) {

    }

    //wait a frame to setup stuff because unity sucks
    IEnumerator Setup() {
        yield return new WaitForFixedUpdate();
        if (isLocalPlayer) {
            GameObject.Find("GameLobby").transform.Find((slot + 1).ToString()).GetComponent<Lobby>().setPlayer(this);
        }
    }
}
