using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameSettings : NetworkBehaviour {

    //possible game types
    public enum GameType {
        Survival=1,
        Time=2
    }

    //settings
    public GameType gameType;
    public int numLives;
    public int time;
    public int NumberOfAIPlayers;

    void Start () {

        // prevent the scene from destroying this object
        DontDestroyOnLoad(transform.gameObject);
        gameType = GameType.Survival;
        numLives = 3;
    }

    void OnLevelWasLoaded(int level) {

        //gameplay scene
        if (level == 1) {
            GetComponent<GameController>().StartGame();
        }
    }
}
