using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

    private GameSettings settings;
    private int[] playerLives;

	void Start () {
        settings = GetComponent<GameSettings>();
	}
	
	void Update () {
	    
	}

    public void StartGame() {
        
        if (settings.gameType == GameSettings.GameType.Survival) {

            //Setup player lives
            playerLives = new int[settings.NumberOfAIPlayers + NetworkManager.singleton.numPlayers];
            for (int i = 0; i < playerLives.Length; i++) {
                playerLives[i] = settings.numLives;
            }

        } else if (settings.gameType == GameSettings.GameType.Time) {
            //set up time settings
        }
    }

    public void AttemptSpawnPlayer(NetworkConnection connectionToClient, short playerControllerID, int playerSlot) {
        
        playerLives[playerSlot]--;
        print("Lives left: " + playerLives[playerSlot]);

        if (playerLives[playerSlot] > 0) {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            GameObject newPlayer = (GameObject)Instantiate(NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation);
            newPlayer.GetComponent<Player>().playerSlot = playerSlot;
            NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, playerControllerID);
        }
    }
}
