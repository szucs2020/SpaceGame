﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    private Gun gun;
    private SyncFlip syncFlip;

    //movement variables
    float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

    private bool facingRight = false;
	private bool decelerating = false;
	private int jump;
    private int currentPosition;

    //private SpriteRenderer fire;
    private AnimationManager animator;

    //movement flags
    private Vector2 movementAxis;

    private bool buttonPressedJump;
    private bool buttonHeldJump;
    private bool buttonReleasedJump;

    private bool buttonPressedShoot;
    private bool buttonHeldShoot;

    private bool buttonHeldAimLeft;
    private bool buttonHeldAimRight;
    private bool buttonHeldAimUp;
    private bool buttonHeldAimDown;

    Controller2D controller;
    NetworkManager networkManager;

    [SyncVar]
    public int playerSlot;

	//Temp AI Spawning
	public GameObject AI;

	//For PathGeneration
	public Transform currentPlatform;
	private bool isAI = false;

    private Audio2D audio;

	void Start() {

        syncFlip = GetComponent<SyncFlip>();
        syncFlip.player = this;

		if (!isLocalPlayer && !isAI) {
            return;
        }

        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();

        controller = GetComponent<Controller2D> ();
        gun = GetComponent<Gun>();
        animator = GetComponent<AnimationManager>();

        jump = 0;
		gravity = (2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

        //syncPlayer.CmdSyncJetpack(false);

        movementAxis = new Vector2(0, 0);
        buttonPressedJump = false;
        buttonHeldJump = false;
        buttonReleasedJump = false;
        buttonPressedShoot = false;
        buttonHeldShoot = false;

        currentPlatform = null;
        currentPosition = 2;

        audio = GameObject.Find("Audio").GetComponent<Audio2D>();
        if (audio == null) {
            print("Audio null");
        }
    }

    void Update() {

		if (!isLocalPlayer && !isAI) {
            return;
        }

        Vector2 input = movementAxis;
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        //Aiming - TEMPORARY: This will probably change into a function that checks if the player is using controller or keyboard.
        //Flip if J or L are pressed.
        if (buttonHeldAimLeft) {
            if (facingRight) {
                flip();
            }
        } else if (buttonHeldAimRight) {
            if (!facingRight) {
                flip();
            }
        }

        //Change aiming angle based on which keys are pressed.
        if (buttonHeldAimUp && buttonHeldAimLeft || buttonHeldAimUp && buttonHeldAimRight) {
            animator.setUpTilt();
        } else if (buttonHeldAimDown && buttonHeldAimLeft || buttonHeldAimDown && buttonHeldAimRight) {
            animator.setDownTilt();
        } else if (buttonHeldAimUp) {
            animator.setUp();
        } else if (buttonHeldAimDown) {
            animator.setDown();
        } else if (buttonHeldAimLeft || buttonHeldAimRight) {
            animator.setNeutral();
        } else {
            animator.setNeutral();
        }

        //switcing weapons
        if (Input.GetKeyDown("1")) {
            if (GetComponent<Pistol>() == null)
            {
                RemoveGun();
                gameObject.AddComponent<Pistol>();
                gun = (Gun)GetComponent<Pistol>();
            }
        } else if (Input.GetKeyDown("2")) {
            if (GetComponent<Shotgun>() == null)
            {
                RemoveGun();
                gameObject.AddComponent<Shotgun>();
                gun = (Gun)GetComponent<Shotgun>();
            }
        } else if (Input.GetKeyDown("3")) {
            if (GetComponent<PlasmaCannon>() == null)
            {
                RemoveGun();
                gameObject.AddComponent<PlasmaCannon>();
                gun = (Gun)GetComponent<PlasmaCannon>();
            }
        }

        //OLD CODE: Flipping based on movement. Will probably still need this for sprint.
        ////flip sprite
        //if (movementAxis.x > 0 && !facingRight) {
        //    flip();
        //} else if (movementAxis.x < 0 && facingRight) {
        //    flip();
        //}

        float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

        //walking
        if (controller.collisions.below) {
			currentPlatform = controller.collisions.platform;
            if (targetVelocityX < 0) {
                if (facingRight) {
                    animator.setWalkBackward();
                } else {
                    animator.setWalkForward();
                }
            } else if (targetVelocityX > 0) {
                if (!facingRight) {
                    animator.setWalkBackward();
                } else {
                    animator.setWalkForward();
                }
            } else {
                animator.setIdle();
            }
        }

        //jumping
        if (buttonPressedJump) {
			if (controller.collisions.below || jump == 1) {
				velocity.y = maxJumpVelocity;
				decelerating = false;
                if (jump == 1){
                    audio.playBoost();
                }
			}

			if (jump == 0) {
				jump = 1;
                audio.playJump();
            } else if (jump == 1){
				jump = 2;
            }
		} 
		else if (buttonReleasedJump) {
			decelerating = true;
            //syncPlayer.CmdSyncJetpack(false);
        }

		if (velocity.y < 0){
            animator.setFall();
            //syncPlayer.CmdSyncJetpack(false);
        } else if (velocity.y > 0) {
            animator.setJump();
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

    public void Die() {
        //audio.playDie();
        Destroy(gameObject);
    }

    //flip 2D sprite
    private void flip() {
        facingRight = !facingRight;
        syncFlip.CmdSyncFlip(facingRight);
    }

    private void RemoveGun() {
        Destroy(GetComponent<Gun>());
    }

    //getters & setters
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

    //right stick / right hand aiming
    public void setbuttonHeldAimLeft(bool input) {
        this.buttonHeldAimLeft = input;
    }
    public void setbuttonHeldAimRight(bool input) {
        this.buttonHeldAimRight = input;
    }
    public void setbuttonHeldAimUp(bool input) {
        this.buttonHeldAimUp = input;
    }
    public void setbuttonHeldAimDown(bool input) {
        this.buttonHeldAimDown = input;
    }

    //current position
    public void setCurrentPosition(int currentPosition) {
        this.currentPosition = currentPosition;
    }
    public int getCurrentPosition() {
        return this.currentPosition;
    }

	public void setIsAI(bool isAI) {
		this.isAI = isAI;
	}

    public bool getIsAI() {
        return this.isAI;
    }
}
