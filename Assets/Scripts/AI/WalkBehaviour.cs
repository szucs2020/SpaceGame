using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkBehaviour : Behaviour {
	Transform me;
	Node target;
	AIPlayer AI;
	GameObject player;

	MoveClass Move;


	//Blackboard (Memory Storage)
	GameObject blackBoard;
	Blackboard memory;

	public WalkBehaviour(Transform me, Node target, AIPlayer AI, GameObject player) {
		this.me = me;
		this.target = target;
		this.AI = AI;

		Move = new MoveClass ();

		blackBoard = GameObject.Find ("Blackboard");
		memory = blackBoard.GetComponent<Blackboard> ();
	}

	public override Status update() {
		target = memory.getTarget ();

		Move.Move (AI, me, target.transform.position);

		//Debug.Log (AI.currentPlatform + " " + target.transform.parent);
		if (AI.currentPlatform == target.transform.parent) {
			if (Mathf.Abs (me.position.x - target.transform.position.x) < 1f) {
				return Status.BH_SUCCESS;
			}
		} else {
			if (Mathf.Abs (me.position.x - target.transform.position.x) < 1f) {

			}
		}

		return Status.BH_RUNNING;
	}
}
