using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){
		if (LayerMask.LayerToName(col.gameObject.layer) == "Ground"){
			Destroy(gameObject);
		}
	}
}
