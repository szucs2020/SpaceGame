/*
 * CustomNetworkLobby.cs
 * Authors: Christian
 * Description: This script extends the functionality of the network lobby manager class, and manages the client instances
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;

public class CustomNetworkLobby : NetworkLobbyManager {

	public float timeout;
	public GameObject menu;
	public GameObject load;
	public GameObject lobby;

	private bool sneakyFlag = false;
    private NetworkClient localClient;
	private bool connecting = false;
	private float timeLeft;
	private bool isHost = false;

    public void HostGame() {
        localClient = StartHost();
		isHost = true;
    }

	void Update(){
		if (connecting){

			if (localClient != null && localClient.isConnected){
				
				connecting = false;
				load.SetActive(false);
				lobby.SetActive(true);

			} else {
				timeLeft -= Time.deltaTime;
				if (timeLeft <= 0.0f){
					connecting = false;
					localClient.Shutdown();
					load.SetActive(false);
					menu.SetActive(true);
				}
			}
		}
	}

    public void JoinGame(String ipAddress) {
		
        networkAddress = ipAddress;
        localClient = StartClient();

		timeLeft = timeout;
		connecting = true;
    }

    public void CloseConnection() {
        Shutdown();
		SceneManager.LoadScene("Menu");
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
        Player p = gamePlayer.GetComponent<Player>();
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();
        p.playerSlot = lp.slot;
        p.playerName = lp.getPlayerName();
        return true;
    }

    //cleverly getting around the fact that this is called twice every time someone joins
    public override void OnServerConnect(NetworkConnection conn) {
        if (sneakyFlag) {
            this.minPlayers++;
        } else {
            sneakyFlag = true;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        this.minPlayers--;
    }

	public override void OnStopServer(){
		if (!isHost){
			this.StopClient();
			Destroy(this);
		}
	}

    void OnLevelWasLoaded(int level) {
        if (level == SceneManager.GetSceneByName("Main").buildIndex) {
            GameObject.Find("GameSettings").GetComponent<GameController>().SpawnAllAI();
        }
    }

    public bool getIsHost() {
        return isHost;
    }
}
