using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpBehaviour : Behaviour {
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

	bool buttonPressedJumped;

	public JumpBehaviour(Transform me, AStar pathFinder, List<Node> path, Node target, Node previousNode, AIPlayer AI, GameObject player, Player playerComponent, Controller2D controller) {
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
		
	public override Status update() {

		Jump ();

		return Status.BH_SUCCESS;
	}


	bool firstStep = true;
	bool secondStep = false;
	bool thirdStep = false;
	float amountOfTimePassed = 0f;
	public void JumpingHelper() {

		if (firstStep) {
			AI.setbuttonPressedJump (true);
			AI.setbuttonReleasedJump (false);

			firstStep = false;
			secondStep = true;
		} else if (secondStep) {
			AI.setbuttonPressedJump (false);
			if (amountOfTimePassed > 0.4f) {
				AI.setbuttonReleasedJump (true);
				//AI.setbuttonHeldJump (true);
				secondStep = false;
				thirdStep = true;
			}
		} else if (thirdStep) {
			AI.setbuttonPressedJump (true);
			AI.setbuttonReleasedJump (false);
		}
		amountOfTimePassed += Time.deltaTime;
	}









	bool isFlying = false;

	private void Jump() {

		if (Mathf.Abs (target.transform.position.x - me.position.x) < 1f && path.Count == 0) {
			AI.moveSpeed = 1;
		} else {
			AI.moveSpeed = 6;
		}

		if(Mathf.Abs(target.transform.position.x - me.position.x) < .25f && path.Count != 0) {
			target = path [0];
			path.RemoveAt (0);
			firstStep = true;
			secondStep = false;
			thirdStep = false;
			AI.setbuttonPressedJump (false);
			amountOfTimePassed = 0;

			if (target == null) {
				return;
			}
		}

		if(target.transform.position.y > me.position.y + 10 || target.transform.position.x > me.position.x + 10) {

			isFlying = true;
		}

		if (isFlying) {

			if (!(me.position.y - target.transform.position.y > 3f) || target.transform.position.x > me.position.x + 10) {
				JumpingHelper ();
			} else {
				isFlying = false;
				AI.setbuttonHeldJump (false);
				AI.setbuttonReleasedJump (true);
			}
		}

		if (target.transform.position.x < me.position.x) {
			AI.setMovementAxis (new Vector2 (-5, 1));
		} else {
			AI.setMovementAxis (new Vector2 (5, 1));
		}
	}

}
