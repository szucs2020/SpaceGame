using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Need to find away to attach this class onto the player so the BehaviourScript has access to its Unity Components
 * I'm deciding if I should have other Behaviour Derived classes in here
 */

public class MoveBehaviour : Selector {
	AStar pathFinder;
	List<Node> path;
	Node target;
	AIPlayer AI;
	GameObject player;
	Player playerComponent;
	Controller2D controller;
	Transform me;

	float heightOverTwo;

	//Movement
	bool buttonPressedJumped;


	//WalkBehaviour
	WalkBehaviour Walk;

	public MoveBehaviour(Transform me, AStar pathFinder, List<Node> path, Node target, AIPlayer AI, GameObject player, Player playerComponent, Controller2D controller) {
		this.me = me;
		this.pathFinder = pathFinder;
		this.controller = controller;
		this.path = path;
		this.target = target;
		this.AI = AI;
		this.player = player;
		this.playerComponent = playerComponent;

		path = pathFinder.FindShortestPath (player.transform.position);
		target = path [0];
		path.RemoveAt (0);

		//Movement
		buttonPressedJumped = false;

		// I calculated the players height to be 16
		heightOverTwo = 8f;
	}

	// Use this for initialization
	public override void onInitialize () {
		m_children = new List<Behaviour> ();
		m_children.Add (new WalkBehaviour(me, pathFinder, path, target, AI, player, playerComponent, controller));

		m_Currentchild = (Behaviour)m_children[0];
	}

	double timedelta = 0;
}

public class WalkBehaviour : Behaviour {
	Transform me;
	AStar pathFinder;
	List<Node> path;
	Node target;
	AIPlayer AI;
	GameObject player;
	Player playerComponent;
	Controller2D controller;

	float heightOverTwo;

	//Movement
	bool buttonPressedJumped;

	public WalkBehaviour() {
		this.me = null;
	}

	public WalkBehaviour(Transform me, AStar pathFinder, List<Node> path, Node target, AIPlayer AI, GameObject player, Player playerComponent, Controller2D controller) {
		this.me = me;
		this.pathFinder = pathFinder;
		this.controller = controller;
		this.path = path;
		this.target = target;
		this.AI = AI;
		this.player = player;
		this.playerComponent = playerComponent;

		//Movement
		buttonPressedJumped = false;

		// I calculated the players height to be 16
		heightOverTwo = 8f;
	}

	float holdJumpButtom = 0f;
	private void MoveToPlayersPlatform () {
		if (target == null) {
			AI.setMovementAxis (new Vector2(0, 0));
			return;
		}

		if (Time.time - holdJumpButtom > 1f || holdJumpButtom == 0f) {
			AI.setbuttonPressedJump (false);
		} else {
			holdJumpButtom += Time.deltaTime;
		}

		if(Mathf.Abs(target.transform.position.x - me.position.x) < .5f) {

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

		if(target.y > me.position.y - heightOverTwo /*&& Mathf.Abs(target.x - transform.position.x) < 15f*/) {
			holdJumpButtom = Time.time;
			AI.setbuttonPressedJump (true);
		}

		if (target.x < me.position.x) {
			AI.setMovementAxis (new Vector2 (-5, 1));
		} else {
			AI.setMovementAxis (new Vector2 (5, 1));
		}
	}

	double timedelta = 0;
	public override Status update() {
		timedelta += Time.deltaTime;
		if (timedelta < 2) {
			return Status.BH_RUNNING;
		}

		if (AI.currentPlatform != playerComponent.currentPlatform) {
			MoveToPlayersPlatform ();
		} else {
			AI.setMovementAxis (new Vector2 (0, 0));
		}

		return Status.BH_SUCCESS;
	}
}
