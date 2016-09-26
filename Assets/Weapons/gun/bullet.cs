using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour {

	public float flightTime;

	void Start(){
		Destroy(gameObject, flightTime);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (LayerMask.LayerToName(col.gameObject.layer) == "Ground"){
			Destroy(gameObject);
		}
	}
}
