/*
 * LobbyPlayer.cs
 * Authors: Christian
 * Description: Supplies a player controller for the lobby, that allows users to change their own settings but not 
 * other player's settings.
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkLobbyPlayer {

	private Lobby lob;
	private GameSettings settings;

    [SyncVar(hook = "CallUpdateTeam")]
    private int playerTeam = 0;

	[SyncVar(hook = "CallUpdateName")]
	private string playerName;

    [Command]
    public void CmdChangeTeam() {

        if (playerTeam != 3) {
            playerTeam++;
        } else {
            playerTeam = 0;
        }
    }

	[Command]
	public void CmdChangeName(string pn) {
		playerName = pn;
	}

    private void CallUpdateTeam(int pt) {
        GameObject.Find("GameLobby").GetComponent<Lobby>().UpdateTeam(pt, this.slot);
    }

	private void CallUpdateName(string name){
		GameObject.Find("GameLobby").GetComponent<Lobby>().UpdateName(name, this.slot);
	}

    void Awake() {
        //wait a frame to setup stuff because unity sucks
        StartCoroutine("Setup");
    }

    //Give the player's slot a reference to the lobbyplayer
    IEnumerator Setup() {
        yield return new WaitForFixedUpdate();
		lob = GameObject.Find("GameLobby").GetComponent<Lobby>();
        if (isLocalPlayer) {
			settings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
            lob.setPlayer(this);
			CmdChangeName(settings.getLocalPlayerName());
		} else {
            CallUpdateName(playerName);
        }
    }

    public int GetTeam() {
        return this.playerTeam;
    }

    public string getPlayerName() {
        return playerName;
    }
}
