using UnityEngine;

public class PlayerController : MonoBehaviour {

	private int speed = 10;
	private int jumpHeight = 10000;

	bool facingRight = true;
	private bool isGrounded = true;
	private bool secondJump = false;
    private float distToGround;

    // Use this for initialization
    void Start () {
        //distToGround = transform..bounds.extents.y;
    }
	
	// Update is called once per frame
	void Update () {

		float movement = Input.GetAxis("Horizontal");
		GetComponent<Rigidbody2D>().velocity = new Vector2(movement * speed, GetComponent<Rigidbody2D>().velocity.y);

		//face left or right depending on movement direction
		if (movement > 0 && !facingRight){
			flip ();
		} else if (movement < 0 && facingRight){
			flip ();
		}

		//jump with space, but only if on ground
		if (Input.GetButtonDown("Jump")){
            isGrounded = Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), LayerMask.GetMask("Ground"));
            if (isGrounded == true || secondJump == false) {
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpHeight));
				isGrounded = false;
				secondJump = secondJump ? false : true;
			}
		}
	}

	private void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
