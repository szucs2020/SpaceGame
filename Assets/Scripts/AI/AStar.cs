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

	bool found = false;

	// Use this for initialization
	void Start () {
		//Grab the target element
		target = GameObject.Find ("1,5").GetComponent<Node>();

		//Initialize each node
		for(int i = 0; i < nodes.Length; i++) {
			nodes [i].Init (target);
			if (nodes [i].name == "2,4" || nodes [i].name == "2,5" || nodes [i].name == "4,4" || nodes [i].name == "2,2") {
				nodes [i].closed = true;
				nodes [i].h = 999999;
				nodes [i].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
			}
		} 

		open = new Heap ();
		open.Init ();
		//Initializing First Node
		currentNode = startNode;
		currentNode.g = 0;
		currentNode.f = currentNode.h;
		currentNode.parent = null;
		//currentNode.closed = true;
		//currentNode.open = false;
		closed.Add (currentNode);
		currentNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
		open.Insert (currentNode);
		Debug.Log ("Printing");
		open.Print ();
		Debug.Log ("Done");


		while(open.GetLength() > 0) {
			currentNode = open.Extract ();
			closed.Add (currentNode);
			currentNode.closed = true;
			currentNode.open = false;
			if (currentNode == null) {
				break;
			}
			///
			///
			///
			///You have to go through everything in the Open list not just the neighbours!!!!!!
			///But also take a look at the wikipedia algorithm, seems more optimized
			///
			///
			for (int i = 0; i < 8; i++) {
				
				if (currentNode.neighbour [i] != null) {

					if (currentNode.neighbour [i].isTarget == true && currentNode.neighbour[i].closed == false) {
						/*Debug.Log ("Found Target");
						Debug.Log (currentNode.neighbour [i]);
						Debug.Log (currentNode.neighbour [i].parent);*/
						currentNode.neighbour [i].parent = currentNode;
						currentNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
						ColourPath (currentNode.neighbour [i]);
						return;
					} else if (currentNode.neighbour [i].closed == true) {
						continue;
					}

					currentNode.neighbour [i].g = Mathf.Abs(Vector2.Distance (currentNode.neighbour [i].transform.position, currentNode.transform.position));
					float tentativeG = currentNode.neighbour [i].g + currentNode.g;

					if (currentNode.neighbour [i].open == false) {
						currentNode.neighbour [i].open = true;
						open.Insert (currentNode.neighbour [i]);
						Debug.Log ("Printing");
						currentNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
						open.Print ();
						Debug.Log ("Done");
					} else if (tentativeG >= currentNode.neighbour [i].g) {
						continue;
					}/* else if (tentativeG < currentNode.neighbour [i].g) {
						currentNode.neighbour [i].parent = currentNode;
						currentNode.neighbour [i].g = tentativeG;
					}*/

					//currentNode.neighbour [i].closed = true;
					currentNode.neighbour [i].parent = currentNode;
					currentNode.neighbour [i].g = tentativeG;
					//currentNode.neighbour [i].f = currentNode.neighbour [i].g + currentNode.neighbour [i].h;
					currentNode.neighbour [i].f = currentNode.neighbour [i].g + currentNode.neighbour [i].h;
					currentNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
					//currentNode = currentNode.neighbour [i];
				}
			}
		}
	}

	private void ColourPath(Node target) {
		Node current = target;
		Debug.Log ("Colour");
		while (current.parent != null) {
			current.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
			current = current.parent;
			Debug.Log (current.name);
		}
		startNode.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
		target.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
