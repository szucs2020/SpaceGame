using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
	AStar pathFinder;
	Queue path;
	Node target;
	Player player;
	Controller2D controller;

	//Movement
	bool buttonPressedJumped;

	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();
		path = pathFinder.FindShortestPath ();
		controller = this.GetComponent<Controller2D> ();

		Debug.Log (path.Length());
		path.Print();
		target = path.Dequeue ();

		player = transform.GetComponent<Player> ();

		//Movement
		buttonPressedJumped = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			Move ();
		} else {
			player.setMovementAxis (new Vector2 (0, 0));
		}
	}

	private void Move() {
		player.setbuttonPressedJump (false);

		if(Mathf.Abs(target.transform.position.x - transform.position.x) < .25f) {
			target = path.Dequeue ();

			if (target == null) {
				return;
			}
			Debug.Log (target.transform.position + " " + Mathf.Abs(target.transform.position.x - transform.position.x));
		}

		if(target.transform.position.y > transform.position.y + 5 && Mathf.Abs(target.transform.position.x - transform.position.x) < 6f) {
			player.setbuttonPressedJump (true);
		}
		
		//Debug.Log (Mathf.Abs(target.transform.position.x - transform.position.x));
		if (target.transform.position.x < transform.position.x) {
			player.setMovementAxis (new Vector2 (-1, 1));
		} else {
			player.setMovementAxis (new Vector2 (1, 1));
		}

	}
}
