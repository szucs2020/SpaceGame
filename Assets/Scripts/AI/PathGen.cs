using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGen : MonoBehaviour {

	public GameObject Node;
	public List<GameObject> ObjectList;

	// Use this for initialization
	void Start () {

		ObjectList = new List<GameObject> ();

		foreach (Transform child in transform) {
			Debug.Log ("Child");
			MeshRenderer renderer = child.GetComponent<MeshRenderer> ();
			int length = (int)renderer.bounds.size.x;
			//Debug.Log ( (int)child.transform.position.x + length / 2);
			for (int i = (int)child.transform.position.x + length / 2; i > (int)child.transform.position.x - length / 2; i = i - 2) {
				ObjectList.Add((GameObject)Instantiate (Node, new Vector3(child.transform.position.x - i, -1, 0), Quaternion.identity));
			}

			List<Node> objectNode;
			for (int i = 0; i < ObjectList.Count; i++) {
				ObjectList [i].name = i.ToString();
				//Debug.Log (i);
				ObjectList [i].GetComponent<Node> ().neighbour = new List<Node> ();
				objectNode = ObjectList [i].GetComponent<Node> ().neighbour;

				if (i == 0) {
					objectNode.Add (ObjectList [i + 1].GetComponent<Node>());
				} else if(i != 0 && i != ObjectList.Count - 1) {
					Debug.Log ("Not 0 Nor Count - 1");
					objectNode.Add (ObjectList [i + 1].GetComponent<Node>());
					objectNode.Add (ObjectList [i - 1].GetComponent<Node>());
				} else if(i != ObjectList.Count - 1) {
					objectNode.Add (ObjectList [i - 1].GetComponent<Node>());
				}
			}
			Debug.Log (renderer.bounds.size);
		}
	}
	
}
