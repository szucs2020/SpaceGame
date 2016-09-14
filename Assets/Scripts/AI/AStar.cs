using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {
	public Node startNode;
	private Node currentNode;
	public Node target;


	public List<Node> nodes;
	public Node[] closedNodes;


	Heap open;

	void Start() {
		nodes = new List<Node> ();
		List<GameObject> objectList = GameObject.Find ("Platforms").GetComponent<PathGen> ().ObjectList;

		for (int i = 0; i < objectList.Count; i++) {
			nodes.Add (objectList[i].GetComponent<Node> ());
		}

		startNode = GameObject.Find ("1").GetComponent<Node> ();
	}

	public List<Node> FindShortestPath() {

		//Initialize each node
		for(int i = 0; i < nodes.Count; i++) {
			nodes [i].Init (target);
		}

		for(int i = 0; i < closedNodes.Length; i++) {
			if (closedNodes [i] != null) {
				closedNodes [i].setClosed(true);
				closedNodes [i].setH(float.MaxValue);
				closedNodes [i].setColour(Color.white);
			}
		}

		open = new Heap ();
		open.Init ();
		//Initializing First Node
		currentNode = startNode;
		currentNode.setG(0f);
		currentNode.setF(currentNode.getH());
		currentNode.clearParent();
		currentNode.setColour(Color.red);
		open.Insert (currentNode);

		while(open.GetLength() > 0) {

			currentNode = open.Extract ();
			currentNode.setClosed(true);
			currentNode.setOpen(false);

			if (currentNode == null) {
				break;
			}

			for (int i = 0; i < currentNode.neighbour.Count; i++) {
				
				if (currentNode.neighbour [i] != null) {

					if (currentNode.neighbour [i].isTarget == true && currentNode.neighbour[i].getClosed() == false) {

						currentNode.neighbour [i].setParent(currentNode);
						currentNode.setColour(Color.blue);
						ColourPath (currentNode.neighbour [i]);
						List<Node> path = new List<Node>();
						//path.Init ();
						//Add Target and CurrentNode to Queue before traversing backwards
						path.Insert (0, target);
						target.inPath = true;
						path.Insert (0, currentNode);
						currentNode.inPath = true;
						while (currentNode.getParent () != null) {
							path.Insert (0, currentNode.getParent ());
							currentNode = currentNode.getParent ();
							currentNode.inPath = true;
						}
						return path;
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
			
		List<Node> backupPath = new List<Node> ();
		backupPath.Insert (0, startNode);
		return backupPath;
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
