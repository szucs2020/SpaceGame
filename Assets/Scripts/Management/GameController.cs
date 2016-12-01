/*
 * GameController.cs
 * Authors: Christian
 * Description: This script controls the gametypes and respawning of the players
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameController : NetworkBehaviour {

    private CustomNetworkLobby manager;
    private GameSettings settings;
    private int[] playerLives;

    [SerializeField]
    GameObject AIPrefab;

	void Start () {
        settings = GetComponent<GameSettings>();
        manager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkLobby>();
    }

    public void StartGame() {
        
        if (settings.gameType == GameSettings.GameType.Survival) {

            //Setup player lives
            playerLives = new int[settings.NumberOfAIPlayers + NetworkManager.singleton.numPlayers];
            for (int i = 0; i < playerLives.Length; i++) {
                playerLives[i] = settings.numLives;
            }

        } else if (settings.gameType == GameSettings.GameType.Time) {
            GameObject.Find("HUD").transform.Find("Timer").GetComponent<Timer>().setTime(settings.time);
        }
    }

    public void EndGame() {
        manager.CloseConnection();
        Destroy(manager.gameObject);
        Destroy(settings.gameObject);
        SceneManager.LoadScene("EndGame");
    }

    public void AttemptSpawnPlayer(NetworkConnection connectionToClient, short playerControllerID, int playerSlot, string playerName) {

        bool respawn = false;
		bool end = false;

        if (settings.gameType == GameSettings.GameType.Survival) {

            int playersLeft = 0;

            playerLives[playerSlot]--;

            //check if there is a winner
            for (int i = 0; i < playerLives.Length; i++) {
                if (playerLives[i] > 0) {
                    playersLeft++;
                }
            }
            if (playersLeft <= 1) {
				end = true;
                EndGame();
            }

            if (playerLives[playerSlot] > 0) {
                respawn = true;
            }
        }

		if (end == false && (respawn == true || settings.gameType == GameSettings.GameType.Time)) {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            GameObject newPlayer = (GameObject)Instantiate(NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation);
            newPlayer.GetComponent<Player>().playerSlot = playerSlot;
            newPlayer.GetComponent<Player>().playerName = playerName;
            NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, playerControllerID);
        }
    }

    // Update is called once per frame
    public void SpawnAllAI() {
        if (!isServer) {
            return;
        }
        for (int i = 0; i < settings.NumberOfAIPlayers; i++) {
            SpawnAI(0, "hello");
        }
    }

    public void SpawnAI(int slot, string name) {
        Transform t = manager.GetStartPosition();
        GameObject AI = (GameObject)GameObject.Instantiate(AIPrefab, t.position, Quaternion.identity);
        NetworkServer.Spawn(AI);
    }
}
