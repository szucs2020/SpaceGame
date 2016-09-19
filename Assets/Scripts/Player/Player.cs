﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;
	public float jumpDeceleration = 0.5f;

	public float minJetpackVelocity;
	public float maxJetpackVelocity;
	public float jetpackAcceleration;

	public Gun gun;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

    private bool facingRight = false;
	private bool decelerating = false;
	private int jump;
	private SpriteRenderer fire;

    //movement flags
    private Vector2 movementAxis;
    private bool buttonPressedJump;
    private bool buttonHeldJump;
    private bool buttonReleasedJump;
    private bool buttonPressedShoot;
    private bool buttonHeldShoot;

    Controller2D controller;

	//For PathGeneration
	public Transform currentPlatform;

	void Start() {
		controller = GetComponent<Controller2D> ();
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

		currentPlatform = null;
    }

	void Update() {

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
			//jumping
			if (controller.collisions.below) {
				velocity.y = maxJumpVelocity;
			}

			if (jump == 0) {
				jump = 1;
			} else if (jump == 1){
				jump = 2;
			}
		} 
		else if (buttonHeldJump) {
			//jetpack
            if (jump == 2) {
                fire.enabled = true;
				if (!controller.collisions.below) {
					decelerating = false;
					if (velocity.y < maxJetpackVelocity){
						velocity.y += jetpackAcceleration;
					}
				}
            }
        } 
		else if (buttonReleasedJump) {
			//release jump
			decelerating = true;
			jump = 1;
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

        velocity.y -= gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input);

		 if(controller.collisions.below){
			currentPlatform = controller.collisions.platform;
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
		if (buttonHeldShoot) {
			gun.shootAutomatic();
		}
	}

    private void flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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


	public bool getDecelerating() {
		return this.decelerating;
	}

	public int getJump() {
		return this.jump;
	}
}
