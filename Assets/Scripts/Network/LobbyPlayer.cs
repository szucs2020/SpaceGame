using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

    void Awake() {
        //wait a frame to setup stuff because unity sucks
        StartCoroutine("Setup");
    }

    public override void OnClientReady(bool readyState) {

    }

    //Give the player's slot a reference to the lobbyplayer
    IEnumerator Setup() {
        yield return new WaitForFixedUpdate();
        if (isLocalPlayer) {
            GameObject.Find("GameLobby").transform.Find((slot).ToString()).GetComponent<Lobby>().setPlayer(this);
        }
    }
}
