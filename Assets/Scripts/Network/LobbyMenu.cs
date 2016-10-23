using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour {

    public CustomNetworkLobby lobbyManager;
    public InputField ipAddress;
    public GameObject menu;
    public GameObject lobby;

    public void onClickHost() {
        lobbyManager.StartHost();
        menu.SetActive(false);
        lobby.SetActive(true);
    }

    public void onClickJoin() {
        lobbyManager.networkAddress = ipAddress.text;
        lobbyManager.StartClient();
        menu.SetActive(false);
        lobby.SetActive(true);
    }

}
