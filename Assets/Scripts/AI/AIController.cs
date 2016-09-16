using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
	AStar pathFinder;
	List<Node> path;
	Node target;
	Player AI;
	GameObject player;
	Controller2D controller;

	//Movement
	bool buttonPressedJumped;

	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();
		path = pathFinder.FindShortestPath ();
		controller = this.GetComponent<Controller2D> ();

		target = path [0];
		path.RemoveAt (0);

		AI = transform.GetComponent<Player> ();

		player = GameObject.Find ("Player");

		//Movement
		buttonPressedJumped = false;
	}

	private void getClosestNodeToPlayer() {
		Node closestNode = null;

		/*If path is empty of Player can't be found find closest nodes to AI and Player
		 * and run AStar again*/
		if (path.Count == 0 && player != null) {
			Node closestToAI = null;
			Node closestToPlayer = null;
			float dist = float.MaxValue;
			float smallestDist = float.MaxValue;

			for (int i = 0; i < pathFinder.nodes.Count; i++) {
				dist = (AI.transform.position - pathFinder.nodes[i].transform.position).sqrMagnitude;
				if (dist < smallestDist) {
					smallestDist = dist;
					closestToAI = pathFinder.nodes [i];
				}
			} pathFinder.startNode = closestToAI;

			dist = float.MaxValue;
			smallestDist = float.MaxValue;

			for (int i = 0; i < pathFinder.nodes.Count; i++) {
				dist = (player.transform.position - pathFinder.nodes[i].transform.position).sqrMagnitude;
				if (dist < smallestDist) {
					smallestDist = dist;
					closestToPlayer = pathFinder.nodes [i];
				}
			} pathFinder.target = closestToPlayer;

			path = pathFinder.FindShortestPath ();
			target = path [0];
			return;
		}

		float distToTarget = (player.transform.position - path [path.Count - 1].transform.position).sqrMagnitude;

		for (int i = 0; i < path [path.Count - 1].neighbour.Count; i++) {
			float dist = (player.transform.position - path [path.Count - 1].neighbour [i].transform.position).sqrMagnitude;

			Vector3 nodeToNeighbour = path [path.Count - 1].neighbour [i].transform.position - path [path.Count - 1].transform.position;
			Vector3 nodeToPlayer = player.transform.position - path [path.Count - 1].transform.position;

			float nodeToNeighDirX = (nodeToNeighbour / nodeToPlayer.magnitude).x;
			float nodeToPlayDirX = (nodeToPlayer / nodeToPlayer.magnitude).x;
			float nodeToNeighDirY = (nodeToNeighbour / nodeToPlayer.magnitude).y;
			float nodeToPlayDirY = (nodeToPlayer / nodeToPlayer.magnitude).y;

			/*This is mostly used if the AI is jumping up or down a platform
			 * because the neighbouring node on the platform above/below will
			 * always be further then the neighbour right beside it so instead
			 * if the neighbouring node is in the same direction as the player
			 * then it will instead choose that node*/
			if ((nodeToNeighDirX > 0 && nodeToPlayDirX > 0) || (nodeToNeighDirX < 0 && nodeToPlayDirX < 0)) {

				if ((nodeToNeighDirY > 0 && nodeToPlayDirY > 0) || (nodeToNeighDirY < 0 && nodeToPlayDirY < 0)) {
					dist = 0;
				}
			}

			if(dist < distToTarget ) {
				distToTarget = dist;
				closestNode = path [path.Count - 1].neighbour [i];
			}
		}

		if (closestNode != null && !closestNode.getInPath ()) {
			closestNode.setInPath (true);
			path [path.Count - 1].setColour (Color.yellow);
			path.Add (closestNode);
			path [path.Count - 1].setColour (Color.red);
		} else if (closestNode != null && closestNode.getInPath ()) {
			closestNode.setColour (Color.red);
			closestNode.setInPath (false);
			path.RemoveAt (path.Count - 1);
		}
	}

	double timedelta = 0;
	// Update is called once per frame
	void Update () {

		getClosestNodeToPlayer ();

		timedelta += Time.deltaTime;
		if (timedelta < 2) {
			return;
		}

		if (target != null) {
			Walk ();
		} else {
			AI.setMovementAxis (new Vector2 (0, 0));
		}
	}

	private void Walk() {

		AI.setbuttonPressedJump (false);

		if(Mathf.Abs(target.transform.position.x - transform.position.x) < .5f) {

			try {
				target = path [0];
				path.RemoveAt (0);
			} catch {

			}


			if (target == null) {
				return;
			}
		}

		if(target.transform.position.y > transform.position.y + 2 && Mathf.Abs(target.transform.position.x - transform.position.x) < 6f) {
			AI.setbuttonPressedJump (true);
		}

		if (target.transform.position.x < transform.position.x) {
			AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}

	}

	bool inAir = false;
	double deltaTime = 0;

	bool step1 = true;
	bool step2 = false;

	private void FlyerHelper() {

		deltaTime += Time.deltaTime;

		if (step1) {

			AI.setbuttonPressedJump (true);

			if (deltaTime > 0.3f) {
				step1 = false;
				step2 = true;
			}
		} else if (step2) {

			AI.setbuttonPressedJump (false);
			AI.setbuttonHeldJump (true);
		}
	}

	bool isFlying = false;

	private void Fly() {

		if (Mathf.Abs (target.transform.position.x - transform.position.x) < 1f && path.Count == 0) {
			AI.moveSpeed = 1;
		} else {
			AI.moveSpeed = 30;
		}

		if(Mathf.Abs(target.transform.position.x - transform.position.x) < .25f && path.Count != 0) {
			target = path [0];
			path.RemoveAt (0);
			step1 = true;
			step2 = false;
			AI.setbuttonPressedJump (false);
			deltaTime = 0;

			if (target == null) {
				return;
			}
		}

		if(target.transform.position.y > transform.position.y + 10 || target.transform.position.x > transform.position.x + 10) {

			isFlying = true;
		}

		if (isFlying) {

			if (!(transform.position.y - target.transform.position.y > 3f) || target.transform.position.x > transform.position.x + 10) {
				FlyerHelper ();
			} else {
				isFlying = false;
				AI.setbuttonHeldJump (false);
				AI.setbuttonReleasedJump (true);
			}
		}

		if (target.transform.position.x < transform.position.x) {
			AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}
	}

	private void Hover(Node target) {

		AI.moveSpeed = 10;


		if (transform.position.y < target.transform.position.y + 10f) {
			FlyerHelper ();
		} else if (transform.position.y > target.transform.position.y + 10.05f) {
			AI.setbuttonHeldJump (false);
			AI.setbuttonReleasedJump (true);
			step1 = true;
		}

		if (target.transform.position.x < transform.position.x) {
			AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}
	}
}
