using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
	public Node[] neighbour = new Node[4];
	public float g;  //Movement cost to neighbour relative to parent;
	public float f;  // g + h

	public Node parent;

	public Node target;
	public float h;  //Heuristic; Distance to target node

	// Use this for initialization
	public void Init () {
		transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
		target = GameObject.Find ("W16").GetComponent<Node>();
		h = Vector2.Distance (target.transform.position, transform.position);
		for (int i = 0; i < 4; i++) {
			if (neighbour [i] != null) {
				neighbour[i].g = Vector2.Distance (neighbour [i].transform.position, transform.position); //Movement cost = distance to neighbour
			}
		}
	}

	public void CalcH () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
