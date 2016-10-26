using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;

public class CustomNetworkLobby : NetworkLobbyManager {

    private bool hostFlag = false;

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
        Player p = gamePlayer.GetComponent<Player>();
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();
        p.playerSlot = lp.slot;
        return true;
    }

    public override void OnServerConnect(NetworkConnection conn) {
        if (hostFlag) {
            this.minPlayers++;
        } else {
            hostFlag = true;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        print("disconnect");
        this.minPlayers--;
    }

     void OnLevelWasLoaded(int level) {
        if (level == 1) {
            GameObject.Find("GameSettings").GetComponent<GameController>().SpawnAI();
        }
    }
}
