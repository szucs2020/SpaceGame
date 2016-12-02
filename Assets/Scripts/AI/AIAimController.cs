using UnityEngine;
using System.Collections;

public class AIAimController : MonoBehaviour {
	private Player AI;
	private Transform Spawns;
	private Transform player;
	private SyncFlip AISync;
	private PlayerFinder playerFinder;

	private RaycastHit2D Hit;

	//Spawns
	private Transform spawn0;
	private Transform spawn1;
	private Transform spawn2;
	private Transform spawn3;
	private Transform spawn4;

	// Use this for initialization
	void Start () {
		AI = transform.GetComponent<Player> ();
		Spawns = transform.Find ("spawn");

		playerFinder = transform.GetComponent<PlayerFinder> ();
		player = playerFinder.getPlayerTransform ();

		AISync = transform.GetComponent<SyncFlip> ();

		spawn0 = Spawns.GetChild (0);
		spawn1 = Spawns.GetChild (1);
		spawn2 = Spawns.GetChild (2);
		spawn3 = Spawns.GetChild (3);
		spawn4 = Spawns.GetChild (4);
	}
	
	// Update is called once per frame
	private float angle1 = 0;
	private float angle2 = 0;
	private float angle3 = 0;
	private AimDirection angleEnum1 = 0;
	private AimDirection angleEnum2 = 0;
	private bool aimRight = false;
	private bool aimLeft = false;
	private bool aimUp = false;
	private bool aimDown = false;
	private Vector3 vector1;
	private Vector3 vector2;
	private Vector2 Up;
	private Vector2 Down;
	private Vector2 UpRight;
	private Vector2 UpLeft;
	private Vector2 DownRight;
	private Vector2 DownLeft;
	private Vector2 Right;
	private Vector2 Left;
	void Update () {
		if (player == null) {
			player = playerFinder.getPlayerTransform ();
			AI.setbuttonHeldAimRight (true);
			AI.setbuttonHeldAimLeft (false);
			AI.setbuttonHeldAimUp (false);
			AI.setbuttonHeldAimDown (false);
			return;
		}

		if (player.position.x > transform.position.x && AISync.getFacingRight () == false) {
			aimLeft = false;
			aimRight = true;
			AI.setbuttonHeldAimLeft (aimLeft);
			AI.setbuttonHeldAimRight (aimRight);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
			return;
		} else if (player.position.x < transform.position.x && AISync.getFacingRight () == true) {
			aimRight = false;
			aimLeft = true;
			AI.setbuttonHeldAimRight (aimRight);
			AI.setbuttonHeldAimLeft (aimLeft);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
			return;
		}

		Vector3 playerPos = player.transform.position - new Vector3 (0, 5, 0);
		Vector2 origin = new Vector2(0f, 0f);//Init
		Vector2 hitDirection = new Vector2(0f, 0f);//Init
		AimDirection vectorDirection = 0;
		Quaternion rotation;
		Vector2 direction;
		if (AISync.getFacingRight () == true) {
			if (player.position.y >= transform.position.y) {
				//Up
				aimUp = true;
				aimDown = false;

				//Up
				rotation = spawn4.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.right;
				vector1 = direction * 50;
				vector2 = (playerPos - spawn4.position);
				angle1 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2 (spawn4.position.x, spawn4.position.y), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn4.position.x, spawn4.position.y), (playerPos - spawn4.position), Color.white);
				angleEnum1 = AimDirection.Up;
				Up = vector1;

				//Up Right
				rotation = spawn3.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.right;
				vector1 = direction * 50;
				vector2 = (playerPos - spawn3.position);
				angle2 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2 (spawn3.position.x, spawn3.position.y), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn3.position.x, spawn3.position.y), (playerPos - spawn3.position), Color.white);
				angleEnum2 = AimDirection.UpRight;
				UpRight = vector1;
			} else {
				//Down
				aimUp = false;
				aimDown = true;

				//Down
				rotation = spawn0.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.right;
				Vector3 spawn0Pos = spawn0.position - new Vector3 (0, 4, 0);
				vector1 = direction * 50;
				vector2 = (playerPos - spawn0Pos);
				angle1 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2 (spawn0.position.x, spawn0.position.y - 4), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn0.position.x, spawn0.position.y - 4), (playerPos - spawn0Pos), Color.white);
				angleEnum1 = AimDirection.Down;
				Down = vector1;

				//Down Right
				rotation = spawn1.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.right;
				vector1 = direction * 50;
				vector2 = (playerPos - spawn1.position);
				angle2 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2 (spawn1.position.x, spawn1.position.y), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn1.position.x, spawn1.position.y), (playerPos - spawn1.position), Color.white);
				angleEnum2 = AimDirection.DownRight;
				DownRight = vector1;
			}

			if (angle1 > angle2) {
				angle1 = angle2;
				vectorDirection = angleEnum2;
				aimRight = true;
			} else {
				aimRight = false;
				aimLeft = false;
				vectorDirection = angleEnum1;
			}


			//Right
			Debug.DrawRay (new Vector2 (spawn2.position.x, spawn2.position.y), new Vector3 (50, 0, 0), Color.yellow);
			Debug.DrawRay (new Vector2 (spawn2.position.x, spawn2.position.y), (playerPos - spawn2.position), Color.white);
			vector1 = new Vector3 (spawn2.position.x + 50, spawn2.position.y + 0, 0) - new Vector3 (spawn2.position.x, spawn2.position.y, 0);
			vector2 = (playerPos - spawn2.position);
			angle3 = Vector3.Angle (vector1, vector2);
			Right = vector1;

			if (angle1 > angle3) {
				angle1 = angle3;
				vectorDirection = AimDirection.Right;
				aimUp = false;
				aimDown = false;
			}

			// If the AI gets close to a target it'll aim up
			if (Mathf.Abs (transform.position.x - player.position.x) < 10f && Mathf.Abs (transform.position.y - player.position.y) < 3) {
				angle1 = angle3;
				vectorDirection = AimDirection.Right;
				aimUp = false;
				aimDown = false;
			}

			AI.setbuttonHeldAimRight (aimRight);
			AI.setbuttonHeldAimLeft (aimLeft);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
		} else if (AISync.getFacingRight () == false) {
			if (player.position.y >= transform.position.y) {
				//Up
				aimUp = true;
				aimDown = false;

				//Up
				rotation = spawn4.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				vector1 = direction * 50;
				vector2 = (playerPos - spawn4.position);
				angle1 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2(spawn4.position.x, spawn4.position.y), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn4.position.x, spawn4.position.y), (playerPos - spawn4.position), Color.white);
				angleEnum1 = AimDirection.Up;
				Up = vector1;

				//Up Left
				rotation = spawn3.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				vector1 = direction * 50;
				vector2 = (playerPos - spawn3.position);
				angle2 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2(spawn3.position.x, spawn3.position.y), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn3.position.x, spawn3.position.y), (playerPos - spawn3.position), Color.white);
				angleEnum2 = AimDirection.UpLeft;
				UpLeft = vector1;
			} else {
				//Up
				aimUp = false;
				aimDown = true;

				//Down
				Vector3 spawn0Pos = spawn0.position - new Vector3 (0, 4, 0);
				rotation = spawn0.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				vector1 = direction * 50;
				vector2 = (playerPos - spawn0Pos);
				angle1 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2(spawn0.position.x, spawn0.position.y - 4), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn0.position.x, spawn0.position.y - 4), (playerPos - spawn0Pos), Color.white);
				angleEnum1 = AimDirection.Down;
				Down = vector1;

				//Down Left
				rotation = spawn1.rotation;
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
				direction = rotation * Vector2.left;
				vector1 = direction * 50;
				vector2 = (playerPos - spawn1.position);
				angle2 = Vector3.Angle (vector1, vector2);
				Debug.DrawRay (new Vector2(spawn1.position.x, spawn1.position.y), vector1, Color.yellow);
				Debug.DrawRay (new Vector2 (spawn1.position.x, spawn1.position.y), (playerPos - spawn1.position), Color.white);
				angleEnum2 = AimDirection.DownLeft;
				DownLeft = vector1;
			}

			if (angle1 > angle2) {
				angle1 = angle2;
				vectorDirection = angleEnum2;
				aimLeft = true;
			} else {
				aimRight = false;
				aimLeft = false;
				vectorDirection = angleEnum1;
			}

			//Left
			rotation = spawn2.rotation;
			rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
			direction = rotation * Vector2.left;
			vector1 = direction * 50;
			vector2 = (playerPos - spawn2.position);
			angle3 = Vector3.Angle (vector1, vector2);
			Debug.DrawRay (new Vector2 (spawn2.position.x, spawn2.position.y), vector1, Color.yellow);
			Debug.DrawRay (new Vector2 (spawn2.position.x, spawn2.position.y), (playerPos - spawn2.position), Color.white);
			Left = vector1;

			if (angle1 > angle3) {
				angle1 = angle3;
				vectorDirection = AimDirection.Left;
				aimUp = false;
				aimDown = false;
			}

			// If the AI gets close to a target it'll aim up
			if (Mathf.Abs (transform.position.x - player.position.x) < 5f && Mathf.Abs (transform.position.y - player.position.y) < 3) {
				angle1 = angle3;
				vectorDirection = AimDirection.Left;
				aimUp = false;
				aimDown = false;
			}
			AI.setbuttonHeldAimRight (aimRight);
			AI.setbuttonHeldAimLeft (aimLeft);
			AI.setbuttonHeldAimUp (aimUp);
			AI.setbuttonHeldAimDown (aimDown);
		}

		if (vectorDirection == AimDirection.Up) {
			origin = spawn4.position;
			hitDirection = Up;
		} else if (vectorDirection == AimDirection.Down) {
			origin = spawn0.position;
			hitDirection = Down;
		} else if (vectorDirection == AimDirection.UpRight) {
			origin = spawn3.position;
			hitDirection = UpRight;
		} else if (vectorDirection == AimDirection.UpLeft) {
			origin = spawn3.position;
			hitDirection = UpLeft;
		} else if (vectorDirection == AimDirection.DownRight) {
			origin = spawn1.position;
			hitDirection = DownRight;
		} else if (vectorDirection == AimDirection.DownLeft) {
			origin = spawn1.position;
			hitDirection = DownLeft;
		} else if (vectorDirection == AimDirection.Right) {
			origin = spawn2.position;
			hitDirection = Right;
		} else if (vectorDirection == AimDirection.Left) {
			origin = spawn2.position;
			hitDirection = Left;
		}
		Hit = Physics2D.Raycast (origin, hitDirection, 100f);
	}

	public RaycastHit2D getHit() {
		return Hit;
	}

	enum AimDirection {
		Up = 1,
		Down,
		UpRight,
		UpLeft,
		DownRight,
		DownLeft,
		Right,
		Left
	};
}
