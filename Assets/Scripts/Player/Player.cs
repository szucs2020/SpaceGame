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

    Controller2D controller;

	void Start() {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		print ("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
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
		} else if (Input.GetKeyUp (KeyCode.Space)) {
			decelerating = true;
		}

		//jetpack
		if (Input.GetButton("Jump")){
			if (!controller.collisions.below) {
				decelerating = false;
				if (velocity.y < maxJetpackVelocity){
					velocity.y += jetpackAcceleration;
				}
			}
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

		if (controller.collisions.above) {
			velocity.y = 0;
			decelerating = false;
		} else if(controller.collisions.below){
			velocity.y = 0;
			decelerating = false;
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
