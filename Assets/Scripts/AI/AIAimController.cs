using UnityEngine;
using System.Collections;

public class AIAimController : MonoBehaviour {
	Player AI;
	Transform Spawns;
	Transform player;
	SyncFlip AISync;
	BoxCollider2D playerCollider;

	// Use this for initialization
	void Start () {
		AI = transform.GetComponent<Player> ();
		Spawns = transform.Find ("spawn");

		player = GameObject.Find ("Player(Clone)").transform;
		AISync = transform.GetComponent<SyncFlip> ();
		playerCollider = player.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
