using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
	private AStar pathFinder;
	private List<Node> path;
	public Node target;
	private Player AI;
	private GameObject player;
	private Player playerComponent;
	private Controller2D controller;

	private float playerHeight = 0f;
	private float AIHeight = 0f;
	private bool hasPath = true;

	private Platform savedPlatform = null;

	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();
		controller = this.GetComponent<Controller2D> ();

		AI = transform.GetComponent<Player> ();
		AI.setIsAI (true);

		player = GameObject.Find ("Player(Clone)");

		//Movement
		playerComponent = player.GetComponent<Player> ();
		playerHeight = player.GetComponent<BoxCollider2D> ().bounds.size.y;
		AIHeight = transform.GetComponent<BoxCollider2D> ().bounds.size.y;

		path = pathFinder.FindShortestPath (playerComponent);
		target = path [0];
		path.RemoveAt (0);
	}

	/*
	 * 
	 * 
	 * 
	 * 
	 * PUT IF AI AND PLAYER ARE ON SAME PLATFORM AND AI ON THE EDGE
	 * 
	 * 
	 * 
	 */

	private double timedelta = 0;
	void Update () {
		if (player == null) {
			return;
		}
        timedelta += Time.deltaTime;
		if (timedelta < 2.5f) {
			return;
		}

		if (savedPlatform == null) {
			savedPlatform = AI.currentPlatform.GetComponent<Platform> ();
		} else if (savedPlatform.transform != AI.currentPlatform) {
			savedPlatform = AI.currentPlatform.GetComponent<Platform> ();
		}

		if (AI.currentPlatform == playerComponent.currentPlatform) {
			path.Clear ();
			hasPath = false;
			target = null;

			AI.setbuttonPressedJump (false);
			AI.setbuttonReleasedJump (false);

			if (Mathf.Abs (transform.position.x - AI.currentPlatform.GetComponent<Platform>().getRight()) < 10f) {
				print ("near right edge");
			}

            float variablePos = Random.Range(-5f, 5f);

			if (player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x < 50f) {
				Move (player.transform.position + new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
			} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x < 50f) {
				Move (player.transform.position - new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
			}
		} else if (AI.currentPlatform != playerComponent.currentPlatform) {
			bool onNeighbourPlatform = false;

			foreach (Transform neighbourPlatform in AI.currentPlatform.GetComponent<Platform>().neighbours) {
				if (neighbourPlatform == playerComponent.currentPlatform) {
					onNeighbourPlatform = true;
					break;
				}
			}

			//On a neighbouring platform to the Player
			if (onNeighbourPlatform == true) {
				path.Clear ();
				hasPath = false;
				target = null;
				AI.setbuttonPressedJump (false);
				AI.setbuttonReleasedJump (false);

				float variablePos = Random.Range(-5f, 5f);

				if (player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x < 30f) { //If within a certain distance stop
					AI.setMovementAxis (new Vector2 (0, 0));
				} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x < 30f) { //If within a certain distance stop
					AI.setMovementAxis (new Vector2 (0, 0));
				} else if (player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x > 30f) { //If not within a certain distance continue
					//nodes[1] represents the second(last) node on platform
					/*
					 * 
					 * Put this stuff if the player is on the same platform
					 * 
					 */
					if (transform.position.x - savedPlatform.getLeft () < 15f) {
						Move (playerComponent.currentPlatform.GetComponent<Platform> ().nodes [1].transform.position, true);
					} else {
						Move (playerComponent.currentPlatform.GetComponent<Platform> ().nodes [1].transform.position, false);
					}
				} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x > 30f) { //If not within a certain distance continue
					//nodes[0] represents the first node on platform
					if (savedPlatform.getRight () - transform.position.x < 15f) {
						Move(playerComponent.currentPlatform.GetComponent<Platform> ().nodes[0].transform.position, true);
					} else {
						Move(playerComponent.currentPlatform.GetComponent<Platform> ().nodes[0].transform.position, false);
					}
				}
			} else { //target represents a node on the platform
				if (!hasPath) {
					path = pathFinder.FindShortestPath (playerComponent);
					target = path [0];
					path.RemoveAt (0);
					hasPath = true;
				}

				if (target != null && target.transform.parent == AI.currentPlatform) {
					AI.setbuttonPressedJump (false);
					AI.setbuttonReleasedJump (false);
					WalkOnPlatform ();
				} else if (target != null && target.transform.parent != AI.currentPlatform) {
					WalkOnPlatform();
					/*if (AI.currentPlatform.position.y > target.transform.parent.position.y) {
						//Target Platform is below Current Platform
						if (Mathf.Abs (AI.currentPlatform.position.x - target.transform.position.x) < 50f) {
							//Can probably fall onto the platform
							AI.setbuttonPressedJump (false);
							AI.setbuttonReleasedJump (true);
							Move (target.transform.position, true);
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
					}*/
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
	}

	private void WalkOnPlatform () {
		if(Mathf.Abs(target.transform.position.x - transform.position.x) < 0.5f && target.transform.parent == AI.currentPlatform) {

			if (path.Count == 0) {
				target = null;
				hasPath = false;
				return;
			}

			target = path [0];
			path.RemoveAt (0);
		}

		Move (target.transform.position, true);
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

	void Move(Vector3 target, bool canJump) {

		if (target.x < transform.position.x) {
            AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}
		//Nodes are 3 units above the ground but I added 4 because the player isn't always touching the ground
		if (canJump && Mathf.Abs (target.x - transform.position.x) < 25f && target.y > transform.position.y - AIHeight + 4) {
			JumpingHelper ();
		} else {
			amountOfTimePassed = 0f;
			firstStep = true;
			secondStep = false;
			thirdStep = false;
			finalStep = false;
			once = false;
		}
	}

	void Jump(Vector3 target) {
		JumpingHelper ();
		Move (target, true); //Added true but this method not used anymore
	}

	private float amountOfTimePassed = 0f;
	private bool firstStep = true;
	private bool secondStep = false;
	private bool thirdStep = false;
	private bool finalStep = false;
	private bool once = false;
	public void JumpingHelper() {
		amountOfTimePassed += Time.deltaTime;

		if (firstStep) {
			//Debug.Log ("1 ");

			if (!once) {
				AI.setbuttonPressedJump (true);
				once = true;
			} else {
				AI.setbuttonPressedJump (false);
			}
			AI.setbuttonReleasedJump (false);

			if (amountOfTimePassed > 0.3f) {
				firstStep = false;
				secondStep = true;
				once = false;
			}
		} else if (secondStep) {
			//Debug.Log ("2 ");

			if (!once) {
				AI.setbuttonReleasedJump (true);
				AI.setbuttonPressedJump (false);
				once = true;
			} else {
				AI.setbuttonReleasedJump (false);
				AI.setbuttonPressedJump (false);
				secondStep = false;
				thirdStep = true;
				once = false;
			}
		} else if (thirdStep) {
			//Debug.Log ("3 ");

			if (!once) {
				AI.setbuttonPressedJump (true);
				once = true;
			} else {
				AI.setbuttonPressedJump (false);
			}

			AI.setbuttonReleasedJump (false);
			if (amountOfTimePassed > 0.7f) {
				thirdStep = false;
				once = false;
				finalStep = true;
			}
		} else {
			AI.setbuttonPressedJump (false);
			AI.setbuttonReleasedJump (true);
			finalStep = false;
			firstStep = true;
			amountOfTimePassed = 0f;
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

			path = pathFinder.FindShortestPath (playerComponent);
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

			path = pathFinder.FindShortestPath (playerComponent);
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