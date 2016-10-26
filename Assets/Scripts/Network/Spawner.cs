using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

    [SerializeField]
    GameObject PlayerPrefab;

    [SerializeField]
    GameObject AIPrefab;

    [SerializeField]
    GameObject PlayerSpawnPoint;

    [SerializeField]
    GameObject AISpawnPoint;

    //components
    private GameSettings settings;
    private CustomNetworkLobby manager;

    void Start() {

        print("start AI");

        if (!isServer) {
            return;
        }

        settings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<GameSettings>();
        manager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<CustomNetworkLobby>();
        SpawnAI();
    }

    // Update is called once per frame
    void SpawnAI () {
        for (int i = 0; i < settings.NumberOfAIPlayers; i++) {
            print("Spawning AI");
            GameObject AI = (GameObject)GameObject.Instantiate(AIPrefab, manager.GetStartPosition().position, Quaternion.identity);
            NetworkServer.Spawn(AI);
        }
    }
}