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
		//path = pathFinder.FindShortestPath ();
		controller = this.GetComponent<Controller2D> ();

		//target = path [0];
		//path.RemoveAt (0);

		AI = transform.GetComponent<Player> ();

		player = GameObject.Find ("Player");
		path = pathFinder.FindShortestPath (player.transform.position);
		target = path [0];
		path.RemoveAt (0);

		//Movement
		buttonPressedJumped = false;
	}

	double timedelta = 0;
	// Update is called once per frame
	void Update () {

		//getClosestNodeToPlayer ();

		timedelta += Time.deltaTime;
		if (timedelta < 2) {
			return;
		}

		//if (AI.currentPlatform == player.GetComponent<Player> ().currentPlatform) {

		//	float variablePos = Random.Range(-5f, 5f);

		//	if (player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x < 30f) {
		//		path.Clear ();
		//		target = player.GetComponent<Node> ();
		//		Move (player.transform.position + new Vector3 (variablePos + 20f, 0, 0));
		//	} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x < 30f) {
		//		path.Clear ();
		//		target = player.GetComponent<Node> ();
		//		Move (player.transform.position - new Vector3 (variablePos + 20f, 0, 0));
		//	}
		//} else if (target != null) {
		//	MoveToPlayersPlatform ();
		//} else {
		//	print ("Set Movement Axis");
		//	ReCalcPath ();
		//	AI.setMovementAxis (new Vector2 (0, 0));
		//}
	}


	/*MoveToPlayersPlatform should be broken up into another function which can be called from
	 * the else of line 119 so code isn't duplicated
	 * I guess just separate the firs if
	 */
	private void MoveToPlayersPlatform () {

		AI.setbuttonPressedJump (false);

		if(Mathf.Abs(target.transform.position.x - transform.position.x) < .5f) {

			if (path.Count == 0) {
				target = null;
				return;
			}

			target = path [0];
			path.RemoveAt (0);
		}

		Move (target.transform.position);
	}

	void Move(Vector3 target) {
		if(target.y > transform.position.y + 2 /*&& Mathf.Abs(target.x - transform.position.x) < 15f*/) {
			AI.setbuttonPressedJump (true);
		}

		if (target.x < transform.position.x) {
			AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}
	}

	void ReCalcPath() {
		Node closestNode = null;

		/*If path is empty if Player can't be found find closest nodes to AI and Player
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

			Vector3 playerPos = player.transform.position - new Vector3 (0, player.transform.position.y / 2, 0);
			for (int i = 0; i < pathFinder.nodes.Count; i++) {
				dist = (playerPos - pathFinder.nodes[i].transform.position).sqrMagnitude;
				if (dist < smallestDist) {
					smallestDist = dist;
					closestToPlayer = pathFinder.nodes [i];
				}
			} pathFinder.target = closestToPlayer;

			path = pathFinder.FindShortestPath (player.transform.position);
			target = path [0];
			path.RemoveAt (0);
		}
	}

























	/*************
 * 
 * 
 * Previous Implementation
 * 
 */

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

	private void getClosestNodeToPlayer() {
		Node closestNode = null;

		/*If path is empty if Player can't be found find closest nodes to AI and Player
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

			Vector3 playerPos = player.transform.position - new Vector3 (0, player.transform.position.y / 2, 0);
			for (int i = 0; i < pathFinder.nodes.Count; i++) {
				dist = (playerPos - pathFinder.nodes[i].transform.position).sqrMagnitude;
				if (dist < smallestDist) {
					smallestDist = dist;
					closestToPlayer = pathFinder.nodes [i];
				}
			} pathFinder.target = closestToPlayer;

			path = pathFinder.FindShortestPath (player.transform.position);
			target = path [0];
			path.RemoveAt (0);
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
			closestNode.setColour (Color.magenta);
			closestNode.setInPath (false);
			path.RemoveAt (path.Count - 1);
		}
	}
}