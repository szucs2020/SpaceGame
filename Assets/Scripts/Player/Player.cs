using UnityEngine;
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
	private bool flying;
	private SpriteRenderer fire;

    Controller2D controller;

	void Start() {
		controller = GetComponent<Controller2D> ();
		jump = 0;

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		print ("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);

		fire = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
		fire.enabled = false;
	}

	void Update() {
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

		//flip sprite
        if (Input.GetAxis("Horizontal") > 0 && !facingRight) {
            flip();
        } else if (Input.GetAxis("Horizontal") < 0 && facingRight) {
            flip();
        }

        float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		//jumping
		if (Input.GetButtonDown("Jump")) {
			if (controller.collisions.below) {
				velocity.y = maxJumpVelocity;
			}

			if (jump == 0) {
				jump = 1;
			} else if (jump == 1){
				jump = 2;
			}
		} 

		//jetpack
		else if (Input.GetButton("Jump")){
			if (jump == 2 && flying){
				fire.enabled = true;
				if (!controller.collisions.below) {
					decelerating = false;
					if (velocity.y < maxJetpackVelocity){
						velocity.y += jetpackAcceleration;
					}
				}
			}
		} 

		//release jump
		else if (Input.GetKeyUp (KeyCode.Space)) {
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
			
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input);

		 if(controller.collisions.below){
			velocity.y = 0;
			decelerating = false;
			jump = 0;
			flying = false;
			fire.enabled = false;
		} else {
			flying = true;
			if (controller.collisions.above) {
				velocity.y = 0;
				decelerating = false;
			}
		}

		//shooting
		if (Input.GetButton("Shoot") || Input.GetAxis("Shoot") != 0){
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
}
