using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    //settings
    public int NumberOfAIPlayers;

	void Start () {

        // prevent the scene from destroying this object
        DontDestroyOnLoad(transform.gameObject);
    }
}
