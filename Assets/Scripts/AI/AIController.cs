using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * If the AI.currentPlatform != Player.currentPlatform then 
 *		check if the AI.currentPlatform is a neighbour of Player.currentPlatform
 *			if it is then stay on that platform if you're within a certain distance
 *			if not continue
 */

public class AIController : MonoBehaviour {
	AStar pathFinder;
	List<Node> path;
	Node target;
	AIPlayer AI;
	GameObject player;
	Player playerComponent;
	Controller2D controller;

	MoveBehaviour MoveTheAI;

	Status status;

	//Blackboard (Memory Storage)
	GameObject blackBoard;
	GameObject blackBoardInstance;
	Blackboard memory;

	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();
		controller = this.GetComponent<Controller2D> ();

		AI = transform.GetComponent<AIPlayer> ();

		player = GameObject.Find ("Player(Clone)");
		path = pathFinder.FindShortestPath (player.transform.position);
		target = path [0];
		path.RemoveAt (0);

		//Movement
		playerComponent = player.GetComponent<Player> ();

		//Create Blackboard (Memory Storage)
		blackBoard = new GameObject ();
		//blackBoardInstance = (GameObject)Instantiate (blackBoard, new Vector3(0f, 0f, 0f), Quaternion.identity);
		blackBoard.name = "Blackboard";
		blackBoard.AddComponent<Blackboard> ();
		memory = blackBoard.GetComponent<Blackboard> ();
		memory.setTarget (target);

		MoveTheAI = new MoveBehaviour (transform, target, AI, player);
	}

	private double timedelta = 0;
	// Update is called once per frame
	void Update () {
        timedelta += Time.deltaTime;
		if (timedelta < 2.5f) {
			return;
		}

		if (AI.currentPlatform == playerComponent.currentPlatform) {
			AI.setbuttonPressedJump (false);
			AI.setbuttonReleasedJump (true);

            float variablePos = Random.Range(-5f, 5f);

			if (player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x < 30f) {
				path.Clear ();
				Move (player.transform.position + new Vector3 (variablePos + 20f, 0, 0));
			} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x < 30f) {
				path.Clear ();
				Move (player.transform.position - new Vector3 (variablePos + 20f, 0, 0));
			}
		} else if (AI.currentPlatform != playerComponent.currentPlatform) {
			bool onNeighbourPlatform = false;

			foreach (Transform i in AI.currentPlatform.GetComponent<Platform>().neighbours) {
				if (i == playerComponent.currentPlatform) {
					print (i.name);
					onNeighbourPlatform = true;
					break;
				}
			}

			if (onNeighbourPlatform == true) {
				AI.setMovementAxis (new Vector2 (0, 0));
				AI.setbuttonPressedJump (false);
				AI.setbuttonReleasedJump (true);
				print ("onNeighbour");
			} else {
				if (target != null && target.transform.parent == AI.currentPlatform) {
					AI.setbuttonPressedJump (false);
					AI.setbuttonReleasedJump (true);
					WalkOnPlatform ();
				} else if (target != null && target.transform.parent != AI.currentPlatform) {
					if (AI.currentPlatform.position.y > target.transform.parent.position.y) {
						//Target Platform is below Current Platform
						if (Mathf.Abs (AI.currentPlatform.position.x - target.transform.position.x) < 50f) {
							//Can probably fall onto the platform
							AI.setbuttonPressedJump (false);
							AI.setbuttonReleasedJump (true);
							Move (target.transform.position);
						} else {
							//Has to jump onto platform
							Jump (target.transform.parent.position);
						}
					} else if(AI.currentPlatform.position.y < target.transform.parent.position.y) {
						//Target Platform is above Current Platform
						Jump (target.transform.parent.position);
					} else {
						//Platform is level but there is a gap
						Jump (target.transform.parent.position);
					}
				} else {
					//ReCalcPath ();
					AI.setMovementAxis (new Vector2 (0, 0));
				}
			}
		}

		/*if (path.Count != 0) {
			target = path [0];
			path.RemoveAt (0);
		}*/

		/*MoveTheAI.onInitialize ();
		status = MoveTheAI.tick ();

		if (status == Status.BH_SUCCESS) {
			if (path.Count == 0) {
				target = null;
				AI.setMovementAxis (new Vector2(0, 0));
				return;
			}

			target = path [0];
			path.RemoveAt (0);
			memory.setTarget (target);
		}*/
	}

	private void WalkOnPlatform () {
		if(Mathf.Abs(target.transform.position.x - transform.position.x) < 0.5f) {

			if (path.Count == 0) {
				target = null;
				return;
			}

			target = path [0];
			path.RemoveAt (0);
		}

		Move (target.transform.position);
	}

	private void JumpToPlayersPlatform () {
		if(Mathf.Abs(target.transform.position.x - transform.position.x) < 0.5f) {

			if (path.Count == 0) {
				target = null;
				return;
			}

			target = path [0];
			path.RemoveAt (0);
		}

		Jump (target.transform.position);
	}

	void Move(Vector3 target) {

		if (target.x < transform.position.x) {
            AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}
	}

	void Jump(Vector3 target) {
		JumpingHelper ();
		Move (target);
	}

	private float amountOfTimePassed = 0f;
	private bool firstStep = true;
	private bool secondStep = false;
	private bool thirdStep = false;
	private bool finalStep = false;
	public void JumpingHelper() {
		amountOfTimePassed += Time.deltaTime;

		if (firstStep) {
			//Debug.Log ("1 " + amountOfTimePassed);
			AI.setbuttonPressedJump (true);
			AI.setbuttonReleasedJump (false);

			firstStep = false;
			secondStep = true;
		} else if (secondStep) {
			//Debug.Log ("2 " + amountOfTimePassed);
			if (amountOfTimePassed > 0.5f) {
				AI.setbuttonPressedJump (false);
				AI.setbuttonReleasedJump (true);
				//AI.setbuttonHeldJump (true);
				secondStep = false;
				thirdStep = true;
			}
		} else if (thirdStep) {
			//Debug.Log ("3 " + amountOfTimePassed);
			AI.setbuttonPressedJump (true);
			AI.setbuttonReleasedJump (false);
			thirdStep = false;
		} else {
			if(amountOfTimePassed > 0.9f) {
				//Debug.Log ("4 " + amountOfTimePassed);
				AI.setbuttonPressedJump (false);
				AI.setbuttonReleasedJump (true);
				finalStep = false;
				firstStep = true;
				amountOfTimePassed = 0f;
			}
		}
	}

	void ReCalcPath() {

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