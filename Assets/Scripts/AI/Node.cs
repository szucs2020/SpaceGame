using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
	public Node[] neighbour = new Node[4];
	public float g;  //Movement cost to neighbour relative to parent;
	public float f;  // g + h

	public Node parent;

	public float h;  //Heuristic; Distance to target node

	public bool open;
	public bool closed;
	public bool isTarget;

	// Use this for initialization
	public void Init (Node target) {
		isTarget = false;
		parent = null;
		g = 0;
		closed = false;
		open = false;
		transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;

		h = Vector2.Distance (target.transform.position, transform.position);
		//Calcualte movement cost of each neighbour
		/*for (int i = 0; i < 4; i++) {
			if (neighbour [i] != null) {
				g[i] = Vector2.Distance (neighbour [i].transform.position, transform.position); //Movement cost = distance to neighbour
			} else {
				break;
			}
		}*/
	}

	public void setParents () {
		for (int i = 0; i < 4; i++) {
			if (neighbour [i] != null) {
				neighbour [i].parent = this;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
