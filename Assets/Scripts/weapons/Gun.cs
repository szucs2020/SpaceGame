/********************************************************
 * Authors: 
********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gun : NetworkBehaviour {

	//public variables
	public float rpm;
	public float bulletSpeed;
	public GameObject bulletPrefab;

	//components
	public Transform spawn;
    private Transform[] spawnPositions;

	//external objects
	private Player player;

	//system variables
	private float spawnRotation;
	private float timeBetweenShots;
	private float nextShot;
	private float currentRange;
	
	void Start(){

        timeBetweenShots = 60 / rpm;
		player = GetComponent<Player>();

        //set spawn positions
        spawnPositions = new Transform[5];

        for (int i = 0; i < spawnPositions.Length; i++) {
            spawnPositions[i] = spawn.GetChild(i);
        }
    }

    private Transform getSpawn() {
        return spawnPositions[player.getCurrentPosition()];
    }

    [Command]
    public void CmdShoot(Vector2 direction, Vector2 position) {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        NetworkServer.Spawn(bullet);
    }

    public void shoot() {

		if (canShoot ()) {

			spawn.localEulerAngles = new Vector3(0f, spawn.localEulerAngles.y, 0f);

            Vector2 position = getSpawn().transform.position;
            Vector2 direction;
            Quaternion rotation = getSpawn().transform.rotation;

            if (player.isFacingRight()) {
                direction = rotation * Vector2.right;
            } else {
                rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
                direction = rotation * Vector2.left;
            }

            //create bullet on all clients
            CmdShoot(direction, position);

            //set next shot time
            nextShot = Time.time + timeBetweenShots;
		}
	}

	private bool canShoot(){

		bool canShoot = true;
			
		if (Time.time < nextShot) {
			canShoot = false;
		}
		return canShoot;
	}
}


