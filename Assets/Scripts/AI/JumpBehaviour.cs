using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpBehaviour : Behaviour {
	Transform me;
	Node target;
	AIPlayer AI;
	GameObject player;

	MoveClass Move;


	bool buttonPressedJumped;

	public JumpBehaviour(Transform me, Node target, AIPlayer AI, GameObject player) {
		this.me = me;
		this.target = target;
		this.AI = AI;
		this.player = player;

		Move = new MoveClass ();

		//Movement
		buttonPressedJumped = false;
	}

	public override Status update() {
		return Jump ();
	}


	private bool firstStep = true;
	private bool secondStep = false;
	private bool thirdStep = false;
	private bool finalStep = false;
	private float amountOfTimePassed = 0f;
	public int JumpingHelper() {
		amountOfTimePassed += Time.deltaTime;
			
		if (firstStep) {
			AI.setbuttonPressedJump (true);
			AI.setbuttonReleasedJump (false);

			firstStep = false;
			secondStep = true;

			return 1;
		} else if (secondStep) {
			AI.setbuttonPressedJump (false);
			if (amountOfTimePassed > 0.1f) {
				AI.setbuttonReleasedJump (true);
				//AI.setbuttonHeldJump (true);
				secondStep = false;
				thirdStep = true;
			}
			return 2;
		} else if (thirdStep) {
			AI.setbuttonPressedJump (true);
			AI.setbuttonReleasedJump (false);
			thirdStep = false;
			return 3;
		} else {
			finalStep = false;
			firstStep = true;
			amountOfTimePassed = 0f;
			return 4;
		}
	}

	bool isJumping = false;

	private Status Jump() {

		int jumpProcess = 0;

		if (target.transform.position.y > me.position.y + 10 || target.transform.position.x > me.position.x + 5) {
			isJumping = true;
		} else {
			isJumping = false;
		}

		if (isJumping) {
			jumpProcess = JumpingHelper ();
		} else {
			AI.setbuttonPressedJump (false);
			AI.setbuttonHeldJump (false);
			AI.setbuttonReleasedJump (true);
		}

		Move.Move (AI, me, target.transform.position);
		/*if (target.transform.position.x < me.position.x) {
			AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}*/

		if (jumpProcess < 4 && isJumping) {
			return Status.BH_RUNNING;
		}

		return Status.BH_SUCCESS;
	}

}
