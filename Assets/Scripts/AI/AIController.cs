﻿/*
 * AIController.cs
 * Authors: Lajos Polya
 * Description: This script cotrolls how the AI moves around the map including jumping
 */
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
	private PathGen pathGen;
	private Health playerHealth;
	private PlayerFinder playerFinder;
	private Health health;

	private float AIHeight = 0f;
	private bool hasPath = false;

	private Platform savedPlatform = null;

	// Movement State
	private States state;

	//Same Platform Movement
	private bool inMotion;
	private Vector3 moveTo;
	private bool mightJump;

	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();

		health = transform.GetComponent<Health> ();
		AI = transform.GetComponent<Player> ();
		AI.setIsAI (true);

		playerFinder = transform.GetComponent<PlayerFinder> ();
		player = playerFinder.getPlayer ().gameObject;

		playerHealth = player.GetComponent<Health> ();
		//Movement
		playerComponent = player.GetComponent<Player> ();
		AIHeight = transform.GetComponent<BoxCollider2D> ().bounds.size.y;

		pathGen = GameObject.Find ("Platforms").GetComponent<PathGen> ();

		path = pathFinder.FindShortestPath (playerComponent);
		if (path != null) {
			target = path [0];
			path.RemoveAt (0);
		}

		// Movement State
		state = States.Follow;

		//Same Platform Movement
		inMotion = false;
		mightJump = true;
	}

	private double timedelta = 0;
	void Update () {
		if (player == null) {
			AI.setMovementAxis (new Vector2 (0, 0));

			player = playerFinder.getPlayer ();

			if (player != null) {
				playerComponent = player.GetComponent<Player> ();
				playerHealth = player.GetComponent<Health> ();
			}
			return;
		}
        timedelta += Time.deltaTime;
		if (timedelta < 2.5f) {
			return;
		}

		if (savedPlatform == null) {
			if (AI.currentPlatform != null) {
				savedPlatform = AI.currentPlatform.GetComponent<Platform> ();
			}
		} else if (AI.currentPlatform != null && savedPlatform.transform != AI.currentPlatform) {
			savedPlatform = AI.currentPlatform.GetComponent<Platform> ();
			hasPath = false;
		}

		if (state == States.SamePlatform) {
			//print ("SamePlatform");
			if (AI.currentPlatform != playerComponent.currentPlatform) {
				state = States.Follow;
			} else if(health.getHealth() < 40f && playerHealth.getHealth() > 50f) {
				state = States.Disregard;
				hasPath = false;
			}

			path.Clear ();
			hasPath = false;
			target = null;

			AI.setbuttonPressedJump (false);
			AI.setbuttonReleasedJump (false);

			if (inMotion == false) {
				int index = (int)Random.Range (0, 4.99f);

				if (index < 2) {
					moveTo = AI.currentPlatform.GetChild (index).position;
				} else {
					float x = player.transform.position.x + Random.Range (-30f, 30f);
					moveTo = new Vector3 (x, player.transform.position.y, 0f);

					if (x > savedPlatform.getRight () || x < savedPlatform.getLeft ()) {
						if (x > savedPlatform.getRight ()) {
							foreach (Transform neighbour in savedPlatform.neighbours) {
								if (savedPlatform.transform.position.x < neighbour.transform.position.x) {
									moveTo = neighbour.GetComponent<Platform> ().nodes [0].position;
									mightJump = true;
									break;
								}
							}
							if (moveTo.x == x) {
								moveTo = AI.currentPlatform.GetComponent<Platform> ().nodes [1].transform.position;
							}
						} else if (x < savedPlatform.getLeft ()) {
							foreach (Transform neighbour in savedPlatform.neighbours) {
								if (savedPlatform.transform.position.x > neighbour.transform.position.x) {
									moveTo = neighbour.GetComponent<Platform> ().nodes [1].position;
									mightJump = true;
									break;
								}
							}
							if (moveTo.x == x) {
								moveTo = AI.currentPlatform.GetComponent<Platform> ().nodes [0].transform.position;
							}
						}
					}

				}
				inMotion = true;
			} else {
				if (Mathf.Abs (transform.position.x - moveTo.x) < 2f) {
					inMotion = false;
					mightJump = false;
				}

				if (mightJump == true && (savedPlatform.getRight () - transform.position.x < 15f || transform.position.x - savedPlatform.getLeft () < 15f)) {
					if (moveTo.x > savedPlatform.getRight () || moveTo.x < savedPlatform.getLeft ()) {
						Move (moveTo, true);
					}
				} else {
					Move (moveTo, false);
				}
			}

			/*float variablePos = Random.Range(-5f, 5f);

			if (transform.position.x - player.transform.position.x < 40f || jumpingToNextPlatform == true) {
				if ((player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x < 50f) || jumpingToNextPlatform == true) {
					if (jumpingToNextPlatform == false) {
						Move (player.transform.position + new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
					}

					if (savedPlatform.getRight () - transform.position.x < 15f) {
						jumpingToNextPlatform = true;
						Transform targetNode = findNearestPlatform (playerComponent.currentPlatform.GetComponent<Platform> (), true);
						Move (targetNode.position, true);
					}
				} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x < 50f || jumpingToNextPlatform == true) {
					if (jumpingToNextPlatform == false) {
						Move (player.transform.position - new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
					}

					if (transform.position.x - savedPlatform.getLeft () < 15f) {
						jumpingToNextPlatform = true;
						Transform targetNode = findNearestPlatform (playerComponent.currentPlatform.GetComponent<Platform> (), false);
						Move (targetNode.position, true);
					} 
				}
			} else {
				Move (player.transform.position + new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
			}*/

		} else if (state == States.Follow) {
			//print ("Follow");
			bool onNeighbourPlatform = false;

			if (AI.currentPlatform == playerComponent.currentPlatform) {
				state = States.SamePlatform;
				inMotion = false;
			}
			if (AI.currentPlatform != null) {
				Platform neighboursPlatform = AI.currentPlatform.GetComponent<Platform> ();
				if (neighboursPlatform != null) {
					foreach (Transform neighbourPlatform in neighboursPlatform.neighbours) {
						if (neighbourPlatform == playerComponent.currentPlatform) {
							onNeighbourPlatform = true;
							break;
						}
					}
				}
			}

			//On a neighbouring platform to the Player
			if (onNeighbourPlatform == true) {
				if (path != null) {
					path.Clear ();
				}
				hasPath = false;
				target = null;
				AI.setbuttonPressedJump (false);
				AI.setbuttonReleasedJump (false);

				if (player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x < 30f && inMotion == false) { //If within a certain distance stop
					AI.setMovementAxis (new Vector2 (0, 0));
				} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x < 30f && inMotion == false) { //If within a certain distance stop
					AI.setMovementAxis (new Vector2 (0, 0));
				} else if (player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x > 30f) { //If not within a certain distance continue
					inMotion = true;
					//nodes[1] represents the second(last) node on platform
					if (transform.position.x - savedPlatform.getLeft () < 15f) {
						Move (playerComponent.currentPlatform.GetComponent<Platform> ().nodes [1].transform.position, true);
					} else {
						Move (playerComponent.currentPlatform.GetComponent<Platform> ().nodes [1].transform.position, false);
					}
				} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x > 30f) { //If not within a certain distance continue
					inMotion = true;
					//nodes[0] represents the first node on platform
					if (savedPlatform.getRight () - transform.position.x < 17f) {
						Move(playerComponent.currentPlatform.GetComponent<Platform> ().nodes[0].transform.position, true);
					} else {
						Move(playerComponent.currentPlatform.GetComponent<Platform> ().nodes[0].transform.position, false);
					}
				}
			} else { //target represents a node on the platform
				//print("Not Neighbour");
				if (!hasPath) {
					path = pathFinder.FindShortestPath (playerComponent);
					if (path != null) {
						target = path [0];
						path.RemoveAt (0);
						hasPath = true;
					}
				}

				if (target != null && target.transform.parent == AI.currentPlatform) {
					AI.setbuttonPressedJump (false);
					AI.setbuttonReleasedJump (false);
					WalkOnPlatform ();
				} else if (target != null && target.transform.parent != AI.currentPlatform) {
					WalkOnPlatform();
				} else {
					AI.setMovementAxis (new Vector2 (0, 0));
				}
			}
		} else if (state == States.Disregard) {
			//print ("Disregard");
			if (health.getHealth () > 80f) {
				state = States.Follow;
			}
			AI.setMovementAxis (new Vector2 (0, 0));

			if (!hasPath) {
				int index;
				do {
					index = (int)Random.Range (0f, pathGen.NodesList.Count);
				} while (pathGen.NodesList [index].parent.name == AI.currentPlatform.name);

				path = pathFinder.FindShortestPath (pathGen.NodesList [index].GetComponent<Node> ());
				if (path != null) {
					target = path [0];
					path.RemoveAt (0);
					hasPath = true;
				}
			}

			if (target != null && target.transform.parent == AI.currentPlatform) {
				AI.setbuttonPressedJump (false);
				AI.setbuttonReleasedJump (false);
				WalkOnPlatform ();
			} else if (target != null && target.transform.parent != AI.currentPlatform) {
				WalkOnPlatform();
			} else {
				AI.setMovementAxis (new Vector2 (0, 0));
			}
		}

		/*if (AI.currentPlatform == playerComponent.currentPlatform) {
			path.Clear ();
			hasPath = false;
			target = null;

			AI.setbuttonPressedJump (false);
			AI.setbuttonReleasedJump (false);

            float variablePos = Random.Range(-5f, 5f);

			if (transform.position.x - player.transform.position.x < 40f || jumpingToNextPlatform == true) {
				if ((player.transform.position.x < transform.position.x && transform.position.x - player.transform.position.x < 50f) || jumpingToNextPlatform == true) {
					if (jumpingToNextPlatform == false) {
						Move (player.transform.position + new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
					}
						
					if (savedPlatform.getRight () - transform.position.x < 15f) {
						jumpingToNextPlatform = true;
						Transform targetNode = findNearestPlatform (playerComponent.currentPlatform.GetComponent<Platform> (), true);
						Move (targetNode.position, true);
					}
				} else if (player.transform.position.x > transform.position.x && player.transform.position.x - transform.position.x < 50f || jumpingToNextPlatform == true) {
					if (jumpingToNextPlatform == false) {
						Move (player.transform.position - new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
					}

					if (transform.position.x - savedPlatform.getLeft () < 15f) {
						jumpingToNextPlatform = true;
						Transform targetNode = findNearestPlatform (playerComponent.currentPlatform.GetComponent<Platform> (), false);
						Move (targetNode.position, true);
					} 
				}
			} else {
				Move (player.transform.position + new Vector3 (variablePos + 40f, -playerHeight + 3, 0), false); //Add a random x value so it doesn't always stay the same distance
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
				} else {
					//ReCalcPath ();
					AI.setMovementAxis (new Vector2 (0, 0));
				}
			}
		}*/
	}

	private Transform findNearestPlatform (Platform platform, bool right) {
		List<Transform> rightSide = new List<Transform>();
		List<Transform> leftSide = new List<Transform>();
		Transform targetPlatform;
		Transform targetNode;

		foreach (Transform neighbour in platform.neighbours) {
			if (neighbour.transform.position.x > platform.transform.position.x) {
				rightSide.Add (neighbour);
			} else if (neighbour.transform.position.x < platform.transform.position.x) {
				leftSide.Add (neighbour);
			}
		}

		if (right == true) {
			if (rightSide.Count != 0) {
				targetPlatform = getRandomPlatform (rightSide);
				targetNode = targetPlatform.GetComponent<Platform> ().nodes [0];
			} else {
				targetNode = platform.nodes [0];
			}
		} else {
			if (leftSide.Count != 0) {
				targetPlatform = getRandomPlatform (leftSide);
				targetNode = targetPlatform.GetComponent<Platform> ().nodes [1];
			} else {
				targetNode = platform.nodes [1];
			}
		}

		return targetNode;
	}

	private Transform getRandomPlatform (List<Transform> platforms) {
		int index = (int)Random.Range (0.5f, platforms.Count  - 0.5f);

		return platforms [index];
	}

	private void WalkOnPlatform () {
		if(Mathf.Abs(target.transform.position.x - transform.position.x) < 0.5f && target.transform.parent == AI.currentPlatform) {
			if (path.Count == 0) {
				//print ("0 Path Count");
				target = null;
				hasPath = false;
				return;
			}
			target = path [0];
			path.RemoveAt (0);
		}

		//print (target.name);
		Move (target.transform.position, true);
	}

	void Move(Vector3 target, bool canJump) {
		if (target.x < transform.position.x) {
            AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}
		//Nodes are 3 units above the ground but I added 4 because the player isn't always touching the ground
		if ((canJump && Mathf.Abs (target.x - transform.position.x) < 27f && target.y > transform.position.y - AIHeight + 4)) {
			JumpingHelper ();
		} else {
			amountOfTimePassed = 0f;
			firstStep = true;
			secondStep = false;
			thirdStep = false;
			once = false;
		}
	}

	private float amountOfTimePassed = 0f;
	private bool firstStep = true;
	private bool secondStep = false;
	private bool thirdStep = false;
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
			}
		} else {
			AI.setbuttonPressedJump (false);
			AI.setbuttonReleasedJump (true);
			firstStep = true;
			amountOfTimePassed = 0f;
		}
	}

	enum States {Follow, Disregard, SamePlatform};
}