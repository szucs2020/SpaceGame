﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {
	public Node startNode;
	private Node currentNode;
	public Node target;


	public Node[] nodes;

	Heap open;
	List<Node> closed = new List<Node>();

	// Use this for initialization
	void Start () {
		//Grab the target element
		//target = GameObject.Find ("2,6").GetComponent<Node>();

		//Initialize each node
		for(int i = 0; i < nodes.Length; i++) {
			nodes [i].Init (target);
			if (nodes [i].name == "2,4" || nodes [i].name == "2,5" || nodes [i].name == "4,4" || nodes [i].name == "2,2") {
				nodes [i].setClosed(true);
				nodes [i].setH(999999f);
				nodes [i].setColour(Color.white);
			}
		} 

		open = new Heap ();
		open.Init ();
		//Initializing First Node
		currentNode = startNode;
		currentNode.setG(0f);
		currentNode.setF(currentNode.getH());
		currentNode.clearParent();
		closed.Add (currentNode);
		currentNode.setColour(Color.red);
		open.Insert (currentNode);

		while(open.GetLength() > 0) {
			
			currentNode = open.Extract ();
			closed.Add (currentNode);
			currentNode.setClosed(true);
			currentNode.setOpen(false);

			if (currentNode == null) {
				break;
			}
				
			for (int i = 0; i < 8; i++) {
				
				if (currentNode.neighbour [i] != null) {

					if (currentNode.neighbour [i].isTarget == true && currentNode.neighbour[i].getClosed() == false) {
						
						currentNode.neighbour [i].setParent(currentNode);
						currentNode.setColour(Color.blue);
						ColourPath (currentNode.neighbour [i]);
						return;
					} else if (currentNode.neighbour [i].getClosed() == true) {
						continue;
					}

					//Watch out for this................
					//currentNode.neighbour [i].g = Vector2.Distance(currentNode.neighbour [i].transform.position, currentNode.transform.position) + currentNode.g;
					float tentativeG = Vector2.Distance(currentNode.neighbour [i].transform.position, currentNode.transform.position) + currentNode.getG();

					if (currentNode.neighbour [i].getOpen() == false) {
						
						currentNode.neighbour [i].setOpen(true);
					} else if (tentativeG >= currentNode.neighbour [i].getG()) {
						continue;
					}

					currentNode.neighbour [i].setParent(currentNode);
					currentNode.neighbour [i].setG(tentativeG);
					currentNode.neighbour [i].setF(currentNode.neighbour [i].getG() + currentNode.neighbour [i].getH());
					currentNode.neighbour [i].setColour(Color.blue);

					//You have to add it to the Open Heap here because the Fvalue only gets calculated right before this
					if (open.inHeap(currentNode.neighbour [i]) == false && currentNode.neighbour [i].getOpen () == true && currentNode.neighbour [i].getClosed () == false) {
						open.Insert (currentNode.neighbour [i]);
					}
				}
			}
		}
	}


	//
	//Starting with the target node it travels through each parent
	//and colours it until it reaches the start node
	//
	private void ColourPath(Node target) {
		Node current = target;

		while (current.getParent() != null) {
			current.setColour(Color.green);
			current = current.getParent();
		}
		startNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
		target.setColour(Color.red);
	}
}
