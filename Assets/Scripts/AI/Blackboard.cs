using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blackboard : MonoBehaviour {
	private GameObject instance;
	Node target;
	
	public void setTarget(Node t) {
		target = t;
	}

	public Node getTarget() {
		return target;
	}
}
