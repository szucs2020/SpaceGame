using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveBehaviour : Sequence {
	Node target;
	AIPlayer AI;
	Transform me;
	GameObject player;


	//WalkBehaviour
	WalkBehaviour Walk;
	JumpBehaviour Jump;

	public MoveBehaviour(Transform me, Node target, AIPlayer AI, GameObject player) {
		this.me = me;
		this.target = target;
		this.AI = AI;
		this.player = player;

		Walk = new WalkBehaviour (me, target, AI, player);
		Jump = new JumpBehaviour (me, target, AI, player);
	}

	// Use this for initialization
	public override void onInitialize () {
		m_children = new List<Behaviour> ();
		m_children.Add (Walk);
		m_children.Add (Jump);

		m_Currentchild = (Behaviour)m_children[0];
	}
}
