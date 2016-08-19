using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
	AStar pathFinder;
	Queue path;
	Node target;
	Player player;


	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();
		path = pathFinder.FindShortestPath ();

		target = path.Dequeue ();

		player = transform.GetComponent<Player> ();

		Debug.Log (target.name);

	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	private void Move() {
		player.setMovementAxis (new Vector2(-1, 1));
	}
}
