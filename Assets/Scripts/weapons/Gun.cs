﻿/********************************************************
 * Authors: Christian Szucs (with damage, score and spray done by Lajos and Lorant Polya)
 * This class controls the generi gun behaviour
********************************************************/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Gun : MonoBehaviour {

	//public variables
	public float rpm;
	public float bulletSpeed;
	public GameObject bulletPrefab;

	//components
	public Transform spawn;
	public AudioClip shot;
	private float spawnRotation;
    AudioSource audio;

    //external objects
    private Player player;

	//system variables
	private float timeBetweenShots;
	private float nextShot;
	private float currentRange;
	
	void Start(){
		
        audio = GetComponent<AudioSource>();

        timeBetweenShots = 60 / rpm;
		spawnRotation = spawn.localEulerAngles.y;
		player = (Player) transform.parent.gameObject.GetComponent(typeof(Player));
	}
	
	public void shoot() {

		if (canShoot ()) {

			spawn.localEulerAngles = new Vector3(0f, spawnRotation, 0f);

			//get direction and create ray
			Vector2 direction;

			if (player.isFacingRight()){
				direction = new Vector2 (1, 0);
			} else {
				direction = new Vector2 (-1, 0);
			}

            //play gun shot sound
            audio.PlayOneShot(shot);

			//instantiate bullet prefab
			GameObject bullet = (GameObject)Instantiate(bulletPrefab, spawn.position, spawn.rotation);
			bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
			GetComponentInParent<Player>().CmdSpawn(bullet);

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


