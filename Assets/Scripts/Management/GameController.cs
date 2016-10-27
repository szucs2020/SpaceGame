using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

    private CustomNetworkLobby manager;
    private GameSettings settings;
    private int[] playerLives;

    [SerializeField]
    GameObject AIPrefab;

    void Start () {

        settings = GetComponent<GameSettings>();
        manager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<CustomNetworkLobby>();
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
        print("GAME OVER");
    }

    public void AttemptSpawnPlayer(NetworkConnection connectionToClient, short playerControllerID, int playerSlot) {

        bool respawn = false;

        if (settings.gameType == GameSettings.GameType.Survival) {

            int playersLeft = 0;

            playerLives[playerSlot]--;
            print("Lives left: " + playerLives[playerSlot]);

            //check if there is a winner
            for (int i = 0; i < playerLives.Length; i++) {
                if (playerLives[i] > 0) {
                    playersLeft++;
                }
            }
            if (playersLeft <= 1) {
                EndGame();
            } else if (playerLives[playerSlot] > 0) {
                respawn = true;
            }
        }

        if (respawn == true || settings.gameType == GameSettings.GameType.Time) {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            GameObject newPlayer = (GameObject)Instantiate(NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation);
            newPlayer.GetComponent<Player>().playerSlot = playerSlot;
            NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, playerControllerID);
        }
    }

    // Update is called once per frame
    public void SpawnAI() {
        if (!isServer) {
            return;
        }
        for (int i = 0; i < settings.NumberOfAIPlayers; i++) {
            GameObject AI = (GameObject)GameObject.Instantiate(AIPrefab, manager.GetStartPosition().position, Quaternion.identity);
            NetworkServer.Spawn(AI);
        }
    }
}
