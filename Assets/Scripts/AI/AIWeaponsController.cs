using UnityEngine;
using System.Collections;

public class AIWeaponsController : MonoBehaviour {
	Pistol pistol;
	Player AI;
	Transform Spawns;
	Transform player;
	SyncFlip AISync;
	BoxCollider2D playerCollider;

	// Use this for initialization
	void Start () {
		pistol = transform.GetComponent<Pistol> ();
		AI = transform.GetComponent<Player> ();
		Spawns = transform.Find ("spawn");

		player = GameObject.Find ("Player(Clone)").transform;
		AISync = transform.GetComponent<SyncFlip> ();
		playerCollider = player.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	private float timePassed = 0f;
	private float randomTime = 0f;
	void Update () {
		Vector3 dir3 = (player.position - new Vector3 (0, 5, 0)) - (transform.position - new Vector3 (0, 5, 0));
		Vector2 dir2 = new Vector2 (dir3.x, dir3.y);
		RaycastHit2D Hit;
		float angle;
		bool belowPlayer;

		if (AISync.getFacingRight () == true) {
			//Hit = Physics2D.Raycast (new Vector2 (Spawns.GetChild (1).transform.position.x - 1, Spawns.GetChild (1).transform.position.y), dir2, 50f);
			Debug.DrawRay (transform.position - new Vector3 (0, 5, 0), dir3, Color.green);
			Debug.DrawRay (transform.position - new Vector3(0, 5, 0), Vector3.right * 50, Color.blue);

			angle = Vector3.Angle (new Vector3 (transform.position.x + 50, transform.position.y - 5, transform.position.z) - (transform.position - new Vector3 (5, 0, 0)), dir3);

			if (angle > 95f) {
				AI.setbuttonHeldAimLeft (true);
				AI.setbuttonHeldAimRight (false);
			} else if (angle > 80) {
				AI.setbuttonHeldAimLeft (false);
				AI.setbuttonHeldAimRight (false);

				if (player.position.y > transform.position.y) {
					AI.setbuttonHeldAimUp (true);
					AI.setbuttonHeldAimDown (false);
				} else {
					AI.setbuttonHeldAimUp (false);
					AI.setbuttonHeldAimDown (true);
				}
			} else {
				if (angle > 10) {
					AI.setbuttonHeldAimRight (true);
					AI.setbuttonHeldAimUp (true);

					if (player.position.y > transform.position.y) {
						AI.setbuttonHeldAimUp (true);
						AI.setbuttonHeldAimDown (false);
					} else {
						AI.setbuttonHeldAimUp (false);
						AI.setbuttonHeldAimDown (true);
					}

				} else {
					AI.setbuttonHeldAimRight (true);
					AI.setbuttonHeldAimDown (false);
					AI.setbuttonHeldAimUp (false);
				}
			}
		} else if (AISync.getFacingRight () == false) {
			//Hit = Physics2D.Raycast (new Vector2 (Spawns.GetChild (1).transform.position.x - 1, Spawns.GetChild (1).transform.position.y), dir2, 50f);
			Debug.DrawRay (transform.position - new Vector3 (0, 5, 0), dir3, Color.green);
			Debug.DrawRay (transform.position - new Vector3(0, 5, 0), Vector3.left * 50, Color.red);

			angle = Vector3.Angle (new Vector3 (transform.position.x - 50, transform.position.y - 5, transform.position.z) - (transform.position - new Vector3 (5, 0, 0)), dir3);

			if (angle > 95f) {
				AI.setbuttonHeldAimLeft (false);
				AI.setbuttonHeldAimRight (true);
			} else if (angle > 80) {
				AI.setbuttonHeldAimLeft (false);
				AI.setbuttonHeldAimRight (false);

				if (player.position.y > transform.position.y) {
					AI.setbuttonHeldAimUp (true);
					AI.setbuttonHeldAimDown (false);
				} else {
					AI.setbuttonHeldAimUp (false);
					AI.setbuttonHeldAimDown (true);
				}
			} else {
				if (angle > 10) {
					AI.setbuttonHeldAimLeft (true);
					AI.setbuttonHeldAimUp (true);

					if (player.position.y > transform.position.y) {
						AI.setbuttonHeldAimUp (true);
						AI.setbuttonHeldAimDown (false);
					} else {
						AI.setbuttonHeldAimUp (false);
						AI.setbuttonHeldAimDown (true);
					}
				} else {
					AI.setbuttonHeldAimLeft (true);
					AI.setbuttonHeldAimDown (false);
					AI.setbuttonHeldAimUp (false);
				}
			}
		}

		Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(1).transform.position.x - 1, Spawns.GetChild(1).transform.position.y), new Vector2(-1, 0), 50f);
		//Debug.DrawRay(new Vector2(Spawns.GetChild(1).transform.position.x - 1, Spawns.GetChild(1).transform.position.y), new Vector2(-50, 0),Color.red);

		timePassed += Time.deltaTime;

		if (timePassed > 1f + randomTime) {
			print ("Time Passed");
			randomTime = Random.Range (0f, 1f);
			timePassed = 0f;

			if (Hit.distance < 50) {
				AI.setbuttonPressedShoot (true);
			} else {
				AI.setbuttonPressedShoot (false);
			}
		} else {
			AI.setbuttonPressedShoot (false);
		}
	}
}
