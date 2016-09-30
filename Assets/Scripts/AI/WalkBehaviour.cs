using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	Node previousNode;

	//Movement
	bool buttonPressedJumped;

	public WalkBehaviour(Transform me, AStar pathFinder, List<Node> path, Node target, Node previousNode, AIPlayer AI, GameObject player, Player playerComponent, Controller2D controller) {
		this.me = me;
		this.pathFinder = pathFinder;
		this.controller = controller;
		this.path = path;
		this.target = target;
		this.previousNode = previousNode;
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

		MoveToPlayersPlatform ();

		return Status.BH_SUCCESS;
	}
}
