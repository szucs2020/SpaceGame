using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
	AStar pathFinder;
	Queue path;
	Node target;
	Player player;
	Controller2D controller;

	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();
		path = pathFinder.FindShortestPath ();
		controller = this.GetComponent<Controller2D> ();

		target = path.Dequeue ();

		player = transform.GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	private void Move() {
		player.setMovementAxis (new Vector2(-1, 1));
	}
}
