using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyMenu : MonoBehaviour {

    public CustomNetworkLobby lobbyManager;
    public InputField ipAddress;
    public GameObject menu;
    public GameObject lobby;

    public void onClickHost() {
        lobbyManager.HostGame();
        menu.SetActive(false);
        lobby.SetActive(true);
    }

    public void onClickJoin() {
        lobbyManager.JoinGame(ipAddress.text);
        menu.SetActive(false);
        lobby.SetActive(true);
    }
}
