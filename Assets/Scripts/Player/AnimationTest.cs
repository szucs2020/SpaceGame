using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour {

    Transform head;
    Transform torso;
    Transform legs;

    Animator headAnimator;
    Animator torsoAnimator;
    Animator legsAnimator;

	// Use this for initialization
	void Start () {
        head = transform.Find("Head");
        torso = transform.Find("Torso");
        legs = transform.Find("Legs");

        headAnimator = head.GetComponent<Animator>();
        torsoAnimator = torso.GetComponent<Animator>();
        legsAnimator = legs.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //Upper Body Animations
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            headAnimator.SetTrigger("Neutral");
            torsoAnimator.SetTrigger("Neutral");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            headAnimator.SetTrigger("Up Tilt");
            torsoAnimator.SetTrigger("Up Tilt");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            headAnimator.SetTrigger("Up");
            torsoAnimator.SetTrigger("Up");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            headAnimator.SetTrigger("Down Tilt");
            torsoAnimator.SetTrigger("Down Tilt");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            headAnimator.SetTrigger("Down");
            torsoAnimator.SetTrigger("Down");
        }

        //Lower Body Animations
        if (Input.GetKeyDown(KeyCode.Q))
            legsAnimator.SetTrigger("Idle");
        else if(Input.GetKeyDown(KeyCode.W))
            legsAnimator.SetTrigger("Jump");
        else if (Input.GetKeyDown(KeyCode.S))
            legsAnimator.SetTrigger("Fall");
        else if (Input.GetKeyDown(KeyCode.A))
            legsAnimator.SetTrigger("Walk Forward");
        else if (Input.GetKeyDown(KeyCode.D))
            legsAnimator.SetTrigger("Walk Backward");

    }
}
