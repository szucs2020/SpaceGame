/********************************************************
 * Authors: Christian Szucs (with damage, score and spray done by Lajos and Lorant Polya)
 * This class controls the generi gun behaviour
********************************************************/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]

public class Gun : MonoBehaviour {

	//public variables
	public enum FireMode {Semi, Auto};
	public FireMode gunType;
	public float rpm;
	public float range;
	public float reloadSpeed;
	public int clipSize;

	//components
	public Transform spawn;
	public AudioClip shot;
	public AudioClip click;
	public AudioClip reload;
	private LineRenderer tracer;
	private float spawnRotation;

	//external objects
	private Player player;

	//system variables
	private float timeBetweenShots;
	private float nextShot;
	private int ammo;
	private int ammoLoaded;
	private float currentRange;
	
	void Start(){
		timeBetweenShots = 60/rpm;
		tracer = GetComponent<LineRenderer>();
		ammoLoaded = clipSize;
		ammo = 5 * clipSize;
		spawnRotation = spawn.localEulerAngles.y;
		player = (Player) transform.parent.gameObject.GetComponent(typeof(Player));
	}

	public void shoot() {

		if (canShoot ()) {

			if (ammoLoaded == 0) {
				Reload ();
			}

			else {	
				spawn.localEulerAngles = new Vector3(0f, spawnRotation, 0f);

				//get direction and create ray
				Vector2 direction;

				if (player.isFacingRight()){
					direction = new Vector2 (1, 0);
				} else {
					direction = new Vector2 (-1, 0);
				}

				RaycastHit2D hit = Physics2D.Raycast (new Vector2(spawn.position.x, spawn.position.y), direction, range, LayerMask.GetMask("Ground"));

				//returns true if it hits a collider object
				if (hit.collider) {
					if (hit.transform.tag == "Enemy") {
//						GameObject otherPlayer = GameObject.FindGameObjectWithTag ("Player");
					} else if (hit.transform.tag == "Player") {
						//hitting other players
					}
					currentRange = hit.distance;
				} else {
					currentRange = range;
				}

				//play gun shot sound
//				audio.PlayOneShot(shot);

				//Draw tracer to show shot, pass end point location to coroutine
				StartCoroutine("drawTracer", direction * currentRange);

				//remove one from ammo
				ammoLoaded--;

				//set next shot time
				nextShot = Time.time + timeBetweenShots;
			}
		}
	}

	//support for automatic weapons
	public void shootAutomatic() {
		if (gunType == FireMode.Auto) {
			shoot ();
		}
	}

	public void Reload(){

		int diff;

		if (ammo > 0 && ammoLoaded < clipSize){

			nextShot = Time.time + reloadSpeed;
			diff = clipSize - ammoLoaded;

			if (ammo < diff){
				ammoLoaded += ammo;
				ammo = 0;
			} else{
				ammoLoaded += diff;
				ammo -= diff;
			}
//			audio.PlayOneShot(reload);

		} else if (ammo == 0 && ammoLoaded == 0){
//			audio.PlayOneShot (click);
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

	//This coroutine allows the program to draw the tracer over multiple frames
	//This way, it shows up every other frame
	IEnumerator drawTracer(Vector2 endPoint){



		tracer.enabled = true;

		//give tracer start and endpoint. Starts at gun, ends at range or collision
		tracer.SetPosition(0,spawn.position);
		tracer.SetPosition(1, new Vector2(spawn.position.x, spawn.position.y) + endPoint);
		
		yield return null;
		tracer.enabled = false;
	}

	//Private variable getters and setters
	public int getAmmoLeft(){
		return this.ammo;
	}
	public int getAmmoInGun(){
		return this.ammoLoaded;
	}
	public void addAmmoToGun(){
		this.ammo = clipSize * 5;
	}
}


