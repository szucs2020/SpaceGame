using UnityEngine;
using System.Collections;

public class AIAimController : MonoBehaviour {
	private Player AI;
	private Transform Spawns;
	private Transform player;
	private SyncFlip AISync;

	private RaycastHit2D Hit;

	// Use this for initialization
	void Start () {
		AI = transform.GetComponent<Player> ();
		Spawns = transform.Find ("spawn");

		if (GameObject.Find ("Player(Clone)") != null) {
			player = GameObject.Find ("Player(Clone)").transform;
		}
		AISync = transform.GetComponent<SyncFlip> ();
	}
	
	// Update is called once per frame
	private float angle1 = 0;
	private float angle2 = 0;
	private float angle3 = 0;
	private string angle1str = "";
	private string angle2str = "";
	private string angle3str = "";
	private bool aimRight = false;
	private bool aimLeft = false;
	private bool aimUp = false;
	private bool aimDown = false;
	void Update () {
		if (player == null) {
			if (GameObject.Find ("Player(Clone)") != null) {
				player = GameObject.Find ("Player(Clone)").transform;
			}
			return;
		}

		if (player.position.x > transform.position.x && AISync.getFacingRight () == false) {
			AI.setbuttonHeldAimRight (true);
			AI.setbuttonHeldAimLeft (false);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
			return;
		} else if (player.position.x < transform.position.x && AISync.getFacingRight () == true) {
			AI.setbuttonHeldAimRight (false);
			AI.setbuttonHeldAimLeft (true);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
			return;
		}

		Vector3 playerPos = player.transform.position - new Vector3 (0, 5, 0);
		Vector3 vector1;
		Vector3 vector2;
		string vectorDirection = "";
		if (AISync.getFacingRight () == true) {
			if (player.position.y >= transform.position.y) {
				//Up
				aimUp = true;
				aimDown = false;

				//Up
				Debug.DrawRay (new Vector2 (Spawns.GetChild (4).position.x, Spawns.GetChild (4).position.y), (Spawns.GetChild (4).rotation * Vector2.right) * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (4).position.x, Spawns.GetChild (4).position.y), (playerPos - Spawns.GetChild (4).position), Color.white);
				vector1 = (Spawns.GetChild (4).rotation * Vector2.right) * 50/* - new Vector3 (Spawns.GetChild (4).position.x, Spawns.GetChild (4).position.y, 0)*/;
				vector2 = (playerPos - Spawns.GetChild (4).position)/* - new Vector3 (Spawns.GetChild (4).position.x, Spawns.GetChild (4).position.y, 0)*/;
				angle1 = Vector3.Angle (vector1, vector2);
				angle1str = "UP";

				//Up Right
				Debug.DrawRay (new Vector2 (Spawns.GetChild (3).position.x, Spawns.GetChild (3).position.y), (Spawns.GetChild (3).rotation * Vector2.right) * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (3).position.x, Spawns.GetChild (3).position.y), (playerPos - Spawns.GetChild (3).position), Color.white);
				vector1 = (Spawns.GetChild (3).rotation * Vector2.right) * 50 /*- new Vector3 (Spawns.GetChild (3).position.x, Spawns.GetChild (3).position.y, 0)*/;
				vector2 = (playerPos - Spawns.GetChild (3).position)/* - new Vector3 (Spawns.GetChild (3).position.x, Spawns.GetChild (3).position.y, 0)*/;
				angle2 = Vector3.Angle (vector1, vector2);
				angle2str = "UPRIGHT";
			} else {
				//Down
				aimUp = false;
				aimDown = true;

				//Down
				Vector3 spawn0Pos = Spawns.GetChild (0).position - new Vector3 (0, 4, 0);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (0).position.x, Spawns.GetChild (0).position.y - 4), (Spawns.GetChild (0).rotation * Vector2.right) * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (0).position.x, Spawns.GetChild (0).position.y - 4), (playerPos - spawn0Pos), Color.white);
				vector1 = (Spawns.GetChild (0).rotation * Vector2.right) * 50/* - new Vector3 (Spawns.GetChild (0).position.x, Spawns.GetChild (0).position.y - 4, 0)*/;
				vector2 = (playerPos - spawn0Pos)/* - new Vector3 (Spawns.GetChild (0).position.x, Spawns.GetChild (0).position.y - 4, 0)*/;
				angle1 = Vector3.Angle (vector1, vector2);
				angle1str = "DOWN";

				//Down Right
				Debug.DrawRay (new Vector2 (Spawns.GetChild (1).position.x, Spawns.GetChild (1).position.y), (Spawns.GetChild (1).rotation * Vector2.right) * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (1).position.x, Spawns.GetChild (1).position.y), (playerPos - Spawns.GetChild (1).position), Color.white);
				vector1 = (Spawns.GetChild (1).rotation * Vector2.right) * 50/* - new Vector3 (Spawns.GetChild (1).position.x, Spawns.GetChild (1).position.y, 0)*/;
				vector2 = (playerPos - Spawns.GetChild (1).position)/* - new Vector3 (Spawns.GetChild (1).position.x, Spawns.GetChild (1).position.y, 0)*/;
				angle2 = Vector3.Angle (vector1, vector2);
				angle2str = "DOWN RIGHT";
			}
			aimRight = false;
			aimLeft = false;
			vectorDirection = angle1str;
			if (angle1 > angle2) {
				angle1 = angle2;
				vectorDirection = angle2str;
				aimRight = true;
			}


			//Right
			Debug.DrawRay (new Vector2 (Spawns.GetChild (2).position.x, Spawns.GetChild (2).position.y), new Vector3 (50, 0, 0), Color.yellow);
			Debug.DrawRay (new Vector2 (Spawns.GetChild (2).position.x, Spawns.GetChild (2).position.y), (playerPos - Spawns.GetChild (2).position), Color.white);
			vector1 = new Vector3 (Spawns.GetChild (2).position.x + 50, Spawns.GetChild (2).position.y + 0, 0) - new Vector3 (Spawns.GetChild (2).position.x, Spawns.GetChild (2).position.y, 0);
			vector2 = (playerPos - Spawns.GetChild (2).position)/* - new Vector3 (Spawns.GetChild (2).position.x, Spawns.GetChild (2).position.y, 0)*/;
			angle3 = Vector3.Angle (vector1, vector2);

			if (angle1 > angle3) {
				angle1 = angle3;
				vectorDirection = "RIGHT";
				aimUp = false;
				aimDown = false;
			}
			//print (vectorDirection + " " + angle1);
			AI.setbuttonHeldAimRight (aimRight);
			AI.setbuttonHeldAimLeft (aimLeft);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
		} else if (AISync.getFacingRight () == false) {
			Quaternion rotation;
			Vector2 direction;

			if (player.position.y >= transform.position.y) {
				//Up
				aimUp = true;
				aimDown = false;

				//Up
				rotation = Spawns.GetChild (4).rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				Debug.DrawRay (new Vector2(Spawns.GetChild(4).position.x, Spawns.GetChild(4).position.y), direction * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (4).position.x, Spawns.GetChild (4).position.y), (playerPos - Spawns.GetChild (4).position), Color.white);
				vector1 = direction * 50/* - new Vector2(Spawns.GetChild(4).position.x, Spawns.GetChild(4).position.y)*/;
				vector2 = (playerPos - Spawns.GetChild (4).position)/* - new Vector3 (Spawns.GetChild (4).position.x, Spawns.GetChild (4).position.y, 0)*/;
				angle1 = Vector3.Angle (vector1, vector2);
				angle1str = "UP";

				//Up Left
				rotation = Spawns.GetChild (3).rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				Debug.DrawRay (new Vector2(Spawns.GetChild(3).position.x, Spawns.GetChild(3).position.y), direction * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (3).position.x, Spawns.GetChild (3).position.y), (playerPos - Spawns.GetChild (3).position), Color.white);
				vector1 = direction * 50/* - new Vector2(Spawns.GetChild(3).position.x, Spawns.GetChild(3).position.y)*/;
				vector2 = (playerPos - Spawns.GetChild (3).position)/* - new Vector3 (Spawns.GetChild (3).position.x, Spawns.GetChild (3).position.y, 0)*/;
				angle2 = Vector3.Angle (vector1, vector2);
				angle2str = "UPLEFT";
			} else {
				//Up
				aimUp = false;
				aimDown = true;

				//Down
				Vector3 spawn0Pos = Spawns.GetChild (0).position - new Vector3 (0, 4, 0);
				rotation = Spawns.GetChild (0).rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				Debug.DrawRay (new Vector2(Spawns.GetChild(0).position.x, Spawns.GetChild(0).position.y - 4), direction * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (0).position.x, Spawns.GetChild (0).position.y - 4), (playerPos - spawn0Pos), Color.white);
				vector1 = direction * 50/* - new Vector2(Spawns.GetChild(0).position.x, Spawns.GetChild(0).position.y - 4)*/;
				vector2 = (playerPos - spawn0Pos)/* - new Vector3 (Spawns.GetChild (0).position.x, Spawns.GetChild (0).position.y - 4, 0)*/;
				angle1 = Vector3.Angle (vector1, vector2);
				angle1str = "DOWN";

				//Down Left
				rotation = Spawns.GetChild (1).rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				Debug.DrawRay (new Vector2(Spawns.GetChild(1).position.x, Spawns.GetChild(1).position.y), direction * 50, Color.yellow);
				Debug.DrawRay (new Vector2 (Spawns.GetChild (1).position.x, Spawns.GetChild (1).position.y), (playerPos - Spawns.GetChild (1).position), Color.white);
				vector1 = direction * 50/* - new Vector2(Spawns.GetChild(1).position.x, Spawns.GetChild(1).position.y)*/;
				vector2 = (playerPos - Spawns.GetChild (1).position)/* - new Vector3 (Spawns.GetChild (1).position.x, Spawns.GetChild (1).position.y, 0)*/;
				angle2 = Vector3.Angle (vector1, vector2);
				angle2str = "DOWNLEFT";
			}
			aimRight = false;
			aimLeft = false;
			vectorDirection = angle1str;
			if (angle1 > angle2) {
				angle1 = angle2;
				vectorDirection = angle2str;
				aimLeft = true;
			}

			//Left
			rotation = Spawns.GetChild (2).rotation;
			rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
			direction = rotation * Vector2.left;
			Debug.DrawRay (new Vector2(Spawns.GetChild(2).position.x, Spawns.GetChild(2).position.y), direction * 50, Color.yellow);
			Debug.DrawRay (new Vector2 (Spawns.GetChild (2).position.x, Spawns.GetChild (2).position.y), (playerPos - Spawns.GetChild (2).position), Color.white);
			vector1 = direction * 50/* - new Vector2(Spawns.GetChild(2).position.x, Spawns.GetChild(2).position.y)*/;
			vector2 = (playerPos - Spawns.GetChild (2).position)/* - new Vector3 (Spawns.GetChild (2).position.x, Spawns.GetChild (2).position.y, 0)*/;
			angle3 = Vector3.Angle (vector1, vector2);

			if (angle1 > angle3) {
				angle1 = angle3;
				vectorDirection = "LEFT";
				aimUp = false;
				aimDown = false;
			}
			//print (vectorDirection + " " + angle1);
			AI.setbuttonHeldAimRight (aimRight);
			AI.setbuttonHeldAimLeft (aimLeft);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
		}



		/*Vector3 dir3 = (player.position - new Vector3 (0, 5, 0)) - (transform.position - new Vector3 (0, 5, 0));
		Vector2 dir2 = new Vector2 (dir3.x, dir3.y);
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
		}*/
	}

	public void getLowestAngle() {

	}

	public RaycastHit2D getHit() {
		return new RaycastHit2D ();
		//return Hit;
	}
}
