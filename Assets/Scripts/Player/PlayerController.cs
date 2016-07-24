using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    /*our speeds*/
	public int walkSpeed;
	public int runSpeed;
	public int jumpSpeed;
	public int fallingSpeed;
	public LayerMask detectedLayers;

    private bool isGrounded;
	private bool facingRight;

    public void Start(){
		isGrounded = true;
		facingRight = true;
    }

    public void Update(){

        /*Check if Right arrow key is pressed and its useable*/
        transform.Translate(new Vector2(Input.GetAxis("Horizontal") * runSpeed * Time.deltaTime, 0));

		if (Input.GetAxis("Horizontal") > 0 && !facingRight){
			flip ();
		} else if (Input.GetAxis("Horizontal") < 0 && facingRight){
			flip ();
		}

		if (Input.GetButtonDown("Jump")){
			if (isGrounded == true) {
				transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
				StartCoroutine("jump");
			}
        }

        /*Check if player collides anything under it*/
		float temp = GetComponent<BoxCollider2D>().bounds.extents.y;

		if (Physics2D.Raycast(transform.position, Vector2.down, (GetComponent<BoxCollider2D>().size.y * transform.lossyScale.y) / 2, detectedLayers)){

			isGrounded = true;
        } else{
			isGrounded = false;
            transform.Translate(Vector2.down * fallingSpeed * Time.deltaTime);
        }

    }
    /*Coroutine that we start if we jump. It will wait one millisecond and then it realizes its in air and starts falling down, this is done so it doesn't start to fall down same time as it jumps*/
    IEnumerator jump(){
        yield return new WaitForSeconds(0.1f);
		isGrounded = false;
    }

	private void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}