using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
	AStar pathFinder;
	Queue path;
	Node target;


	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<AStar> ();
		path = pathFinder.FindShortestPath ();

		target = path.Dequeue ();

		Debug.Log (target.name);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Move() {
		if (target.transform.position.x < transform.position.x) {

		}
	}
}
