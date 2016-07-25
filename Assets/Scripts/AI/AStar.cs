using UnityEngine;
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
		target = GameObject.Find ("W9").GetComponent<Node>();

		//Initialize each node
		for(int i = 0; i < nodes.Length; i++) {
			nodes [i].Init (target);
		}
		target.isTarget = true;
		target.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;

		open = new Heap ();
		open.Init ();
		startNode.open = true;
		startNode.g = 0;
		for (int i = 0; i < 4; i++) {
			if (startNode.neighbour [i] != null) {
				//startNode.neighbour [i].Init (target);
				startNode.neighbour [i].parent = startNode;
				startNode.neighbour [i].open = true;
				startNode.neighbour [i].g = Vector2.Distance (startNode.neighbour [i].transform.position, startNode.transform.position) + startNode.g; //Movement cost = distance to neighbour
				startNode.neighbour [i].f = startNode.neighbour [i].g + startNode.neighbour [i].h;
				open.Insert (startNode.neighbour [i]);
			} else {
				break;
			}
		}

		startNode.closed = true;
		startNode.open = false;
		closed.Add (startNode);
		startNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
		open.Print ();

		while(true) {
			currentNode = open.Extract ();
			closed.Add (currentNode);
			currentNode.closed = true;
			currentNode.open = false;
			if (currentNode == null) {
				break;
			}
			for (int i = 0; i < 4; i++) {
				if (currentNode.neighbour [i] != null) {
					if (currentNode.neighbour [i].isTarget == true && currentNode.neighbour[i].closed == false) {
						Debug.Log ("Found Target");
						Debug.Log (currentNode.neighbour [i]);
						return;
					}

					float tentativeG = currentNode.g + Vector2.Distance (currentNode.neighbour [i].transform.position, currentNode.transform.position);

					if (open.inHeap (currentNode.neighbour [i]) == false) {
						open.Insert (currentNode.neighbour [i]);
					} else if (tentativeG >= currentNode.neighbour [i].g) {
						continue;
					}

					currentNode.neighbour [i].parent = currentNode;
					currentNode.neighbour [i].g = tentativeG;
					currentNode.neighbour [i].f = currentNode.neighbour [i].g + currentNode.neighbour [i].h;
					currentNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
					open.inHeap (currentNode.neighbour [i]);
				} else {
					break;
				}
			}
		}

		Debug.Log ("Extracted");
		Debug.Log (currentNode);
		/*Node[] startNeighbours = startNode.neighbour;
		for (int i = 0; i < startNeighbours.Length; i++) {
			if (startNeighbours [i] != null) {
				startNode.f = startNode.h + startNode.g[i];
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
