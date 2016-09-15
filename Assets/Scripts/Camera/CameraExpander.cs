using System;
using UnityEngine;

public class CameraExpander : MonoBehaviour {

    public Vector3 offset = new Vector3(0f, 0f, -1f);
    private GameObject[] players;

    private void LateUpdate(){

        players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null && players.Length > 0) {
            transform.position = new Vector3(players[0].transform.position.x, players[0].transform.position.y, -10);
        }
    }
}
