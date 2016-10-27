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

    private bool hostFlag = false;
    private NetworkClient localClient;

    public void HostGame() {
        localClient = StartHost();
    }

    public void JoinGame(String ipAddress) {
        networkAddress = ipAddress;
        localClient = StartClient();
    }

    public void CloseConnection() {
        Shutdown();
        SceneManager.LoadScene(5);
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
        Player p = gamePlayer.GetComponent<Player>();
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();
        p.playerSlot = lp.slot;
        return true;
    }

    //cleverly getting around the fact that this is called twice every time someone joins
    public override void OnServerConnect(NetworkConnection conn) {
        if (hostFlag) {
            this.minPlayers++;
        } else {
            hostFlag = true;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        this.minPlayers--;
    }

    void OnLevelWasLoaded(int level) {
        print("level: " + level);
        if (level == 1) {
            GameObject.Find("GameSettings").GetComponent<GameController>().SpawnAllAI();
        }
    }
}
