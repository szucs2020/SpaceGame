using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

    void Awake() {

        DontDestroyOnLoad(transform.gameObject);

        //Give the player's slot a reference to the player
        StartCoroutine("Setup");
    }

    //wait a frame to setup stuff because unity sucks
    IEnumerator Setup() {
        yield return new WaitForFixedUpdate();
        print("slot: " + slot);
        if (isLocalPlayer) {
            GameObject.Find("GameLobby").transform.Find((slot + 1).ToString()).GetComponent<PregameMenu>().setPlayer(this);
        }
    }
}
