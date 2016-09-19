﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof (Controller2D))]

public class Player : NetworkBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;
	public float jumpDeceleration = 0.5f;
	public float maxFallSpeed = -110f;

	public Gun gun;
	private SyncFlip syncFlip;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

    private bool facingRight = false;
	private bool decelerating = false;
	private int jump;
	private SpriteRenderer fire;
    AudioSource audio;

    //movement flags
    private Vector2 movementAxis;
    private bool buttonPressedJump;
    private bool buttonHeldJump;
    private bool buttonReleasedJump;
    private bool buttonPressedShoot;
    private bool buttonHeldShoot;

    Controller2D controller;

	void Awake(){
		syncFlip = GetComponent<SyncFlip>();
	}

	void Start() {

        if (!isLocalPlayer) {
            return;
        }

        controller = GetComponent<Controller2D> ();
        audio = GetComponent<AudioSource>();

        jump = 0;
		gravity = (2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

		fire = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
		fire.enabled = false;

        movementAxis = new Vector2(0, 0);
        buttonPressedJump = false;
        buttonHeldJump = false;
        buttonReleasedJump = false;
        buttonPressedShoot = false;
        buttonHeldShoot = false;
    }

	void Update() {

        if (!isLocalPlayer) {
            return;
        }

        Vector2 input = movementAxis;
        int wallDirX = (controller.collisions.left) ? -1 : 1;

		//flip sprite
        if (movementAxis.x > 0 && !facingRight) {
            flip();
        } else if (movementAxis.x < 0 && facingRight) {
            flip();
        }

        float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		if (buttonPressedJump) {
			if (controller.collisions.below || jump == 1) {
				velocity.y = maxJumpVelocity;
				decelerating = false;
				if (jump == 1){
					fire.enabled = true;
				}
			}

			if (jump == 0) {
				jump = 1;
			} else if (jump == 1){
				jump = 2;
			}
		} 

		else if (buttonReleasedJump) {
			decelerating = true;
			fire.enabled = false;
		}

		if (velocity.y < 0){
			fire.enabled = false;
		}

        //decelerate after jump
        if (decelerating && velocity.y > minJumpVelocity) {
            if (velocity.y - jumpDeceleration > minJumpVelocity) {
                velocity.y -= jumpDeceleration;
            } else {
                velocity.y = minJumpVelocity;
            }
        }

		//gravity, with max fall speed
		if (velocity.y > maxFallSpeed){
			if (velocity.y - (gravity * Time.deltaTime) > maxFallSpeed){
				velocity.y -= gravity * Time.deltaTime;
			} else {
				velocity.y = maxFallSpeed;
			}
		}
		
		controller.Move (velocity * Time.deltaTime, input);

		 if(controller.collisions.below){
			velocity.y = 0;
			decelerating = false;
			jump = 0;
			fire.enabled = false;
		} else {
			if (controller.collisions.above) {
				velocity.y = 0;
				decelerating = false;
			}
		}

		//shooting
		if (buttonPressedShoot) {
			gun.shoot();
		}
	}

    private void flip() {
        facingRight = !facingRight;
		syncFlip.CmdSyncFlip(facingRight);
    }

	[Command]
	public void CmdSpawn(GameObject obj){
		NetworkServer.Spawn(obj);
	}

	public bool isFacingRight(){
		return facingRight;
	}
    public void setMovementAxis(Vector2 input) {
        this.movementAxis = input;
    }
    public void setbuttonPressedJump(bool input) {
        this.buttonPressedJump = input;
    }
    public void setbuttonHeldJump(bool input) {
        this.buttonHeldJump = input;
    }
    public void setbuttonReleasedJump(bool input) {
        this.buttonReleasedJump = input;
    }
    public void setbuttonPressedShoot(bool input) {
        this.buttonPressedShoot = input;
    }
    public void setbuttonHeldShoot(bool input) {
        this.buttonHeldShoot = input;
    }
}
