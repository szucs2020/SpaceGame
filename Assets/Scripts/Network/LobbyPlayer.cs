/*
 * LobbyPlayer.cs
 * Authors: Christian
 * Description: Supplies a player controller for the lobby, that allows users to change their own settings but not 
 * other player's settings.
 */
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

    private void CallUpdateTeam(int pt) {
        GameObject.Find("GameLobby").GetComponent<Lobby>().UpdateTeam(pt, this.slot);
    }

    void Awake() {
        //wait a frame to setup stuff because unity sucks
        StartCoroutine("Setup");
    }

    //Give the player's slot a reference to the lobbyplayer
    IEnumerator Setup() {
        yield return new WaitForFixedUpdate();
        if (isLocalPlayer) {
            GameObject.Find("GameLobby").GetComponent<Lobby>().setPlayer(this);
        }
    }

    public int GetTeam() {
        return this.playerTeam;
    }
}
