/*
 * Gun.cs
 * Authors: Christian
 * Description: This is the original gun class that shot on the network. 
 * Lorant made changes to this class for some of the other guns. 
 */
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
        spawn = this.transform.GetChild(0);
        spawnPositions = new Transform[5];

        for (int i = 0; i < spawnPositions.Length; i++) {
            spawnPositions[i] = spawn.GetChild(i);
        }
    }

    protected Transform getSpawn() {
        return spawnPositions[player.getCurrentPosition()];
    }

    protected bool canShoot() {

        bool canShoot = true;

        if (Time.time < nextShot) {
            canShoot = false;
        }

        return canShoot;
    }
}


