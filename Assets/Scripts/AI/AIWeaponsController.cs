using UnityEngine;
using System.Collections;

public class AIWeaponsController : MonoBehaviour {
	Pistol pistol;
	Player AI;
	Transform Spawns;
	Transform player;
	// Use this for initialization
	void Start () {
		pistol = transform.GetComponent<Pistol> ();
		AI = transform.GetComponent<Player> ();
		Spawns = transform.Find ("spawn");

		player = GameObject.Find ("Player(Clone)").transform;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D Hit;

		Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(1).transform.position.x, Spawns.GetChild(1).transform.position.y), new Vector2(Spawns.GetChild(1).transform.position.x, Spawns.GetChild(1).transform.position.y) + new Vector2(-1, 0));
		Debug.DrawRay(new Vector2(Spawns.GetChild(1).transform.position.x, Spawns.GetChild(1).transform.position.y), new Vector2(-1, 0),Color.red);
		if (Hit.collider == player.GetComponent<BoxCollider2D>()) {
			AI.setbuttonPressedShoot (true);
		} else {
			AI.setbuttonPressedShoot (false);
		}
	}
}
