/*
 * Bullet.cs
 * Authors: Christian
 * Description: This class is attached to every "standard" bullet. It supports having an owner and doing damage to other players.
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class bullet : MonoBehaviour {

	public float flightTime;
    public Player bulletOwner;

	void Start(){
		Destroy(gameObject, flightTime);
	}

	void OnTriggerEnter2D(Collider2D col){
        print(col.gameObject.tag);
		if (LayerMask.LayerToName(col.gameObject.layer) == "Ground"){
			Destroy(gameObject);
		}else if (col.gameObject.tag == "Portal"){

        }
        else if (LayerMask.LayerToName(col.gameObject.layer) == "Player") {

            if (col.gameObject.GetComponent<Player>().netId != bulletOwner.netId) {
                //do damage to the other player and destroy the bullet
                col.gameObject.GetComponent<Health>().Damage(5.0f);
                Destroy(gameObject);
            }
        }
	}
}
