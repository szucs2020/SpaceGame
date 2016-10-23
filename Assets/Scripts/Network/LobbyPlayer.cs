using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

    [SyncVar(hook = "CallUpdateTeam")]
    private int playerTeam = 0;

    [Command]
    public void CmdChangeTeam() {

        if (playerTeam != 3) {
            playerTeam++;
        } else {
            playerTeam = 0;
        }
    }

    /*
    private void CallUpdateTeam(int pt) {
        GameObject.Find("GameLobby").transform.Find((slot).ToString()).GetComponent<Lobby>().UpdateTeam(pt);
    }
    */

    private void CallUpdateTeam(int pt) {
        GameObject.Find("GameLobby").GetComponent<Lobby>().UpdateTeam(pt, this.slot);
    }

    void Awake() {
        //wait a frame to setup stuff because unity sucks
        StartCoroutine("Setup");
    }

    public override void OnClientReady(bool readyState) {

    }

    /*
    //Give the player's slot a reference to the lobbyplayer
    IEnumerator Setup() {
        yield return new WaitForFixedUpdate();
        if (isLocalPlayer) {
            GameObject.Find("GameLobby").transform.Find((slot).ToString()).GetComponent<Lobby>().setPlayer(this);
        }
    }
    */

    //Give the player's slot a reference to the lobbyplayer
    IEnumerator Setup() {
        yield return new WaitForFixedUpdate();
        if (isLocalPlayer) {
            GameObject.Find("GameLobby").GetComponent<Lobby>().setPlayer(this);
        }
    }
}
