using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {
	public Node startNode;
	private Node currentNode;

	//public Node[] nodes;

	Heap open;
	List<Node> closed = new List<Node>();

	// Use this for initialization
	void Start () {
		
		open = new Heap ();
		open.Init ();
		for (int i = 0; i < 4; i++) {
			if (startNode.neighbour [i] != null) {
				startNode.neighbour [i].Init ();
				startNode.neighbour [i].parent = startNode;
				open.Insert (startNode.neighbour[i]);
			}
		}

		open.Print ();

		closed.Add (startNode);
		Node[] startNeighbours = startNode.neighbour;
		for (int i = 0; i < startNeighbours.Length; i++) {
			if (startNeighbours [i] != null) {
				startNode.f = startNode.h + startNode.g;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
