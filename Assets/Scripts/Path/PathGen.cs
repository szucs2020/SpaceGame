using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGen : MonoBehaviour {

	public GameObject Node;
	public List<GameObject> ObjectList;

	// Use this for initialization
	void Start () {

		ObjectList = new List<GameObject> ();
		GameObject instance;

		foreach (Transform child in transform) {
			
			MeshRenderer renderer = child.GetComponent<MeshRenderer> ();
			Platform platform = child.GetComponent<Platform> ();
			platform.nodes = new List<Transform> ();
			int length = (int)renderer.bounds.size.x;

			//Generates Nodes for Each Platform
			for (int i = length; i > 0; i = i - 4) {

				if (length - i < 3) {
					instance = (GameObject)Instantiate (Node, new Vector3 ((int)child.position.x - i + 1 + length / 2 + 1, (int)child.position.y - 1, 0), Quaternion.identity);
				} else if (length - i > length - 3) {
					instance = (GameObject)Instantiate (Node, new Vector3 ((int)child.position.x - i + 1 + length / 2 - 1, (int)child.position.y - 1, 0), Quaternion.identity);
				} else {
					instance = (GameObject)Instantiate (Node, new Vector3 ((int)child.position.x - i + 1 + length / 2, (int)child.position.y - 1, 0), Quaternion.identity);
				}

				instance.transform.SetParent (child, true);
				platform.nodes.Add (instance.transform);
				ObjectList.Add(instance);
			}

			List<Node> objectNode;
			for (int i = 0; i < platform.nodes.Count; i++) {
				
				platform.nodes [i].name = i.ToString();
				platform.nodes [i].GetComponent<Node> ().neighbour = new List<Node> ();
				objectNode = platform.nodes [i].GetComponent<Node> ().neighbour;

				if (i == 0) {
					objectNode.Add (platform.nodes [i + 1].GetComponent<Node>());
				} else if(i != 0 && i != platform.nodes.Count - 1) {
					objectNode.Add (platform.nodes [i + 1].GetComponent<Node>());
					objectNode.Add (platform.nodes [i - 1].GetComponent<Node>());
				} else if(i == platform.nodes.Count - 1) {
					objectNode.Add (platform.nodes [i - 1].GetComponent<Node>());
				}
			}
		}

		//Connects the platforms
		foreach (Transform child in transform) {

			MeshRenderer renderer = child.GetComponent<MeshRenderer> ();
			Platform platform = child.GetComponent<Platform> ();
			int length = (int)renderer.bounds.size.x;

			foreach (Transform neighour in child.GetComponent<Platform> ().neighbours) {
				MeshRenderer neighbourRenderer = neighour.GetComponent<MeshRenderer> ();
				Platform neighbourPlatform = neighour.GetComponent<Platform> ();
				int neighbourLength = (int)neighbourRenderer.bounds.size.x;


				if (child.position.x + length / 2 < neighour.position.x - neighbourLength / 2) {
					
					platform.nodes [platform.nodes.Count - 1].GetComponent<Node> ().neighbour.Add(neighbourPlatform.nodes [0].GetComponent<Node> ());
				} else if (child.position.x - length / 2 > neighour.position.x + neighbourLength / 2) {
					Debug.Log ((child.position.x - length / 2) + " " + (neighour.position.x + neighbourLength / 2));
					Debug.Log ("Node to the left");
					platform.nodes [0].GetComponent<Node> ().neighbour.Add(neighbourPlatform.nodes [neighbourPlatform.nodes.Count - 1].GetComponent<Node> ());
				}
			}
		}
	}
	
}



/*
 * 
 * for (int i = (int)child.transform.position.x + length / 2; i > (int)child.transform.position.x - length / 2; i = i - 2) {
				Debug.Log (i + " " + (child.transform.position.x - i) + " " + child.transform.position.x);
				instance = (GameObject)Instantiate (Node, new Vector3 ((int)child.transform.position.x - i, (int)child.transform.position.y - 1, 0), Quaternion.identity);
				//instance.transform.SetParent (child, true);
				//instance.transform.position = new Vector3 ((int)child.transform.localPosition.x - i, (int)child.transform.localPosition.y - 1, 0);
				ObjectList.Add(instance);
			}
 * 
 * 
 * */