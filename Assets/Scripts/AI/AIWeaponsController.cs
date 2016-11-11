/*
 * AIWeaponsController.cs
 * Authors: Lajos Polya
 * Description: This script allows the AI to aim at its target and it also allows it to shoot in bursts
 */
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
	private bool burst = false;
	private int numberOfShots = 0;
	private int shotsFired = 0;
	private float burstTime = 0;
	void Update () {
		if (player == null) {
			player = GameObject.Find ("Player(Clone)").transform;
			return;
		}
		Vector3 dir3 = (player.position - new Vector3 (0, 5, 0)) - (transform.position - new Vector3 (0, 5, 0));
		Vector2 dir2 = new Vector2 (dir3.x, dir3.y);
		RaycastHit2D Hit;
		float angle;
		bool belowPlayer;

		//This RayCast initializes the Hit variable becuase it won't fire in all cases
		Hit = Physics2D.Raycast (new Vector2(transform.position.x, transform.position.y), Vector2.down, 0f);
		if (AISync.getFacingRight () == true) {
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
					Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(4).position.x, Spawns.GetChild(4).position.y), (Spawns.GetChild(4).rotation * Vector2.right) * 50, 50f);
					Debug.DrawRay (new Vector2(Spawns.GetChild(4).position.x, Spawns.GetChild(4).position.y), (Spawns.GetChild(4).rotation * Vector2.right) * 50, Color.magenta);
				} else {
					AI.setbuttonHeldAimUp (false);
					AI.setbuttonHeldAimDown (true);
					Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(0).position.x, Spawns.GetChild(0).position.y - 4), (Spawns.GetChild(0).rotation * Vector2.right) * 50, 50f);
					Debug.DrawRay (new Vector2(Spawns.GetChild(0).position.x, Spawns.GetChild(0).position.y - 4), (Spawns.GetChild(0).rotation * Vector2.right) * 50, Color.magenta);
				}
			} else {
				if (angle > 20) {
					AI.setbuttonHeldAimRight (true);
					AI.setbuttonHeldAimUp (true);

					if (player.position.y > transform.position.y) {
						AI.setbuttonHeldAimUp (true);
						AI.setbuttonHeldAimDown (false);
						Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(3).position.x, Spawns.GetChild(3).position.y), (Spawns.GetChild(3).rotation * Vector2.right) * 50, 50f);
						Debug.DrawRay (new Vector2(Spawns.GetChild(3).position.x, Spawns.GetChild(3).position.y), (Spawns.GetChild(3).rotation * Vector2.right) * 50, Color.magenta);
					} else {
						AI.setbuttonHeldAimUp (false);
						AI.setbuttonHeldAimDown (true);
						Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(1).position.x, Spawns.GetChild(1).position.y), (Spawns.GetChild(1).rotation * Vector2.right) * 50, 50f);
						Debug.DrawRay (new Vector2(Spawns.GetChild(1).position.x, Spawns.GetChild(1).position.y), (Spawns.GetChild(1).rotation * Vector2.right) * 50, Color.magenta);

					}

				} else {
					AI.setbuttonHeldAimRight (true);
					AI.setbuttonHeldAimDown (false);
					AI.setbuttonHeldAimUp (false);
					Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(2).position.x, Spawns.GetChild(2).position.y), new Vector2(1, 0), 50f);
					Debug.DrawRay (new Vector2(Spawns.GetChild(2).position.x, Spawns.GetChild(2).position.y), new Vector2(50, 0), Color.magenta);
				}
			}
		} else if (AISync.getFacingRight () == false) {
			Debug.DrawRay (transform.position - new Vector3 (0, 5, 0), dir3, Color.green);
			Debug.DrawRay (transform.position - new Vector3(0, 5, 0), Vector3.left * 50, Color.red);

			Quaternion rotation;
			Vector2 direction;

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
					rotation = Spawns.GetChild (4).rotation;
					rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
					direction = rotation * Vector2.left;
					Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(4).position.x, Spawns.GetChild(4).position.y), direction * 50, 50f);
					Debug.DrawRay (new Vector2(Spawns.GetChild(4).position.x, Spawns.GetChild(4).position.y), direction * 50, Color.magenta);
				} else {
					AI.setbuttonHeldAimUp (false);
					AI.setbuttonHeldAimDown (true);
					rotation = Spawns.GetChild (0).rotation;
					rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
					direction = rotation * Vector2.left;
					Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(0).position.x, Spawns.GetChild(0).position.y - 4), direction * 50, 50f);
					Debug.DrawRay (new Vector2(Spawns.GetChild(0).position.x, Spawns.GetChild(0).position.y - 4), direction * 50, Color.magenta);
				}
			} else {
				if (angle > 20) {
					AI.setbuttonHeldAimLeft (true);
					AI.setbuttonHeldAimUp (true);

					if (player.position.y > transform.position.y) {
						AI.setbuttonHeldAimUp (true);
						AI.setbuttonHeldAimDown (false);
						rotation = Spawns.GetChild (3).rotation;
						rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
						direction = rotation * Vector2.left;
						Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(3).position.x, Spawns.GetChild(3).position.y), direction * 50, 50f);
						Debug.DrawRay (new Vector2(Spawns.GetChild(3).position.x, Spawns.GetChild(3).position.y), direction * 50, Color.magenta);
					} else {
						AI.setbuttonHeldAimUp (false);
						AI.setbuttonHeldAimDown (true);
						rotation = Spawns.GetChild (1).rotation;
						rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
						direction = rotation * Vector2.left;
						Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(1).position.x, Spawns.GetChild(1).position.y), direction * 50, 50f);
						Debug.DrawRay (new Vector2(Spawns.GetChild(1).position.x, Spawns.GetChild(1).position.y), direction * 50, Color.magenta);
					}
				} else {
					AI.setbuttonHeldAimLeft (true);
					AI.setbuttonHeldAimDown (false);
					AI.setbuttonHeldAimUp (false);
					rotation = Spawns.GetChild (2).rotation;
					rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
					direction = rotation * Vector2.left;
					Hit = Physics2D.Raycast (new Vector2(Spawns.GetChild(2).position.x, Spawns.GetChild(2).position.y), direction * 50, 50f);
					Debug.DrawRay (new Vector2(Spawns.GetChild(2).position.x, Spawns.GetChild(2).position.y), direction * 50, Color.magenta);
				}
			}
		}

		timePassed += Time.deltaTime;
		if (Hit.transform != null && Hit.transform == player) {
			if (timePassed > 1f + randomTime) {
				burst = true;
				numberOfShots = (int)Random.Range (2.5f, 4.5f);
				randomTime = Random.Range (0f, 1f);
				timePassed = 0f;
			} else {
				AI.setbuttonPressedShoot (false);
			}
		}

		if (burst == true) {
			burstTime += Time.deltaTime;

			if (burstTime > 0.3f && shotsFired < numberOfShots) {
				shotsFired++;
				burstTime = 0;

				if (Hit.distance < 50) {
					AI.setbuttonPressedShoot (true);
				} else {
					AI.setbuttonPressedShoot (false);
				}
			} else {
				AI.setbuttonPressedShoot (false);

				if (shotsFired == numberOfShots) {
					shotsFired = 0;
					burst = false;
					burstTime = 0f;
					timePassed = 0f;
				}
			}
		}
	}
}
