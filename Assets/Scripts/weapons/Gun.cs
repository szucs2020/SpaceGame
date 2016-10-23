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

	//components
	public Transform spawn;
    protected Transform[] spawnPositions;

	//external objects
	protected Player player;

	//system variables
	protected float spawnRotation;
    protected float timeBetweenShots;
    protected float nextShot;
    protected float currentRange;
	
	protected void init(){

        timeBetweenShots = 60 / rpm;
		player = GetComponent<Player>();

        //set spawn positions
        spawnPositions = new Transform[5];

        for (int i = 0; i < spawnPositions.Length; i++) {
            spawnPositions[i] = spawn.GetChild(i);
        }
    }

    protected Transform getSpawn() {
        return spawnPositions[player.getCurrentPosition()];
    }

    [Command]
    public virtual void CmdShoot(Vector2 direction, Vector2 position) {
        //GameObject bullet = (GameObject)Instantiate(bulletPrefab, position, Quaternion.identity);
        //bullet.GetComponent<LaserDot>().bulletOwner = player;
        //bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        //NetworkServer.Spawn(bullet);
    }

    public virtual void shoot() {

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

	protected bool canShoot(){

		bool canShoot = true;
			
		if (Time.time < nextShot) {
			canShoot = false;
		}
		return canShoot;
	}
}


