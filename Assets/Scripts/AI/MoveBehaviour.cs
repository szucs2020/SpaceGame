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

	Node previousNode;

	//Movement
	bool buttonPressedJumped;


	//WalkBehaviour
	WalkBehaviour Walk;
	JumpBehaviour Jump;

	public MoveBehaviour(Transform me, AStar pathFinder, List<Node> path, Node target, Node previousNode, AIPlayer AI, GameObject player, Player playerComponent, Controller2D controller) {
		this.me = me;
		this.pathFinder = pathFinder;
		this.controller = controller;
		this.path = path;
		this.target = target;
		this.previousNode = previousNode;
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
		m_children.Add (new JumpBehaviour(me, pathFinder, path, target, previousNode, AI, player, playerComponent, controller));

		m_Currentchild = (Behaviour)m_children[0];
	}

	double timedelta = 0;
}
