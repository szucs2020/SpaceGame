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

			//Generates nodes for each individual platform
			for (int i = length; i > 0; i = i - 5) {

				//If the node is too close to the edge of the platform it will not get generated
				if(!(length - i < 3) && !(length - i > length - 3)) {
					instance = (GameObject)Instantiate (Node, new Vector3 ((int)child.position.x - i + 1 + length / 2, (int)child.position.y - 1, 0), Quaternion.identity);
					instance.transform.SetParent (child, true);
					platform.nodes.Add (instance.transform);
					ObjectList.Add(instance);
				}

				
			}


			//Each node in each platform neighbours the nodes next to it
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

		//Connects the neighbourng platforms
		foreach (Transform child in transform) {

			MeshRenderer renderer = child.GetComponent<MeshRenderer> ();
			Platform platform = child.GetComponent<Platform> ();
			int length = (int)renderer.bounds.size.x;

			foreach (Transform neighour in child.GetComponent<Platform> ().neighbours) {
				MeshRenderer neighbourRenderer = neighour.GetComponent<MeshRenderer> ();
				Platform neighbourPlatform = neighour.GetComponent<Platform> ();
				int neighbourLength = (int)neighbourRenderer.bounds.size.x;

				//If the platform is to the right or left of its neighbour connect its respective nodes
				if (child.position.x + length / 2 < neighour.position.x - neighbourLength / 2) {
					
					platform.nodes [platform.nodes.Count - 1].GetComponent<Node> ().neighbour.Add(neighbourPlatform.nodes [0].GetComponent<Node> ());
				} else if (child.position.x - length / 2 > neighour.position.x + neighbourLength / 2) {

					platform.nodes [0].GetComponent<Node> ().neighbour.Add(neighbourPlatform.nodes [neighbourPlatform.nodes.Count - 1].GetComponent<Node> ());
				}

				//If you can go through the platform then connect respective nodes
				if (child.tag == "Through" || neighour.tag == "Through") {
					for (int i = 0; i < platform.nodes.Count; i++) {
						for (int j = 0; j < neighbourPlatform.nodes.Count; j++) {
							if (platform.nodes[i].transform.position.x - 3f <= neighbourPlatform.nodes[j].transform.position.x && neighbourPlatform.nodes[j].transform.position.x <= platform.nodes[i].transform.position.x + 3f) {
								platform.nodes [i].GetComponent<Node> ().neighbour.Add (neighbourPlatform.nodes[j].GetComponent<Node> ());
							}
						}
					}
				}
			}
		}
	}	
}