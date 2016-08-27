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

		path.Print();
		target = path.Dequeue ();

		player = transform.GetComponent<Player> ();

		//Movement
		buttonPressedJumped = false;
	}

	double timedelta = 0;
	// Update is called once per frame
	void Update () {
		timedelta += Time.deltaTime;
		if (timedelta < 2) {
			return;
		}

		if (target != null) {
			Fly ();
		} else {
			player.setMovementAxis (new Vector2 (0, 0));
		}
	}

	private void Walk() {
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

	bool inAir = false;
	double deltaTime = 0;

	bool step1 = true;
	bool step2 = false;
	bool step3 = false;
	bool step4 = false;

	private void FlyerHelper() {

		deltaTime += Time.deltaTime;

		if (step1) {

			Debug.Log ("Step 1");

			player.setbuttonPressedJump (true);
			//player.setbuttonReleasedJump (false);

			if (deltaTime > 0.3f) {
				step1 = false;
				step2 = true;
			}
		} else if (step2) {

			Debug.Log ("Step 2");

			player.setbuttonPressedJump (false);
			player.setbuttonHeldJump (true);

			//player.setbuttonReleasedJump (true);
			//step2 = false;
			//step3 = true;
		} else if (step3) {

			Debug.Log ("Step 3");

			player.setbuttonHeldJump (true);
			//player.setbuttonReleasedJump (false);

			//step3 = false;
			//step4 = true;
		} else if (step4) {

			//Debug.Log ("Step 4");
			player.setbuttonPressedJump(false);
			player.setbuttonHeldJump (true);
			//player.setbuttonReleasedJump (false);
		}
	}

	bool isFlying = false;

	private void Fly() {


		if(Mathf.Abs(target.transform.position.x - transform.position.x) < .25f && path.Length() != 0) {
			target = path.Dequeue ();

			if (target == null) {
				return;
			}
			//Debug.Log (target.transform.position + " " + Mathf.Abs(target.transform.position.x - transform.position.x));
		} /*else if(path.Length() == 0 && Mathf.Abs(target.transform.position.x - transform.position.x) < 1f && Mathf.Abs(target.transform.position.y - transform.position.y) < 2f) {
			return;
		}*/

		if(target.transform.position.y > transform.position.y + 10) {

			isFlying = true;
		}

		if (isFlying) {

			if (!(transform.position.y - target.transform.position.y > 2f)) {
				Debug.Log (transform.position.y - target.transform.position.y);
				FlyerHelper ();
			} else {
				isFlying = false;
				player.setbuttonHeldJump (false);
				player.setbuttonReleasedJump (true);
			}
		}
		//Debug.Log (Mathf.Abs(target.transform.position.x - transform.position.x));
		if (target.transform.position.x < transform.position.x) {
			player.setMovementAxis (new Vector2 (-1, 1));
		} else {
			player.setMovementAxis (new Vector2 (1, 1));
		}


	}
}
