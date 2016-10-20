using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;

public class CustomNetworkLobby : NetworkLobbyManager {

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
        Player p = gamePlayer.GetComponent<Player>();
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();
        p.playerSlot = lp.slot;
        return true;
    }


}
