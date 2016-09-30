using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

    Transform head;
    Transform torso;
    Transform legs;
    Player player;

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

        player = GetComponent<Player>();
    }

    //upper body
    public void setNeutral() {
        headAnimator.SetTrigger("Neutral");
        torsoAnimator.SetTrigger("Neutral");
        player.setCurrentPosition(2);
    }
    public void setUp() {
        headAnimator.SetTrigger("Up");
        torsoAnimator.SetTrigger("Up");
        player.setCurrentPosition(4);
    }
    public void setUpTilt() {
        headAnimator.SetTrigger("Up Tilt");
        torsoAnimator.SetTrigger("Up Tilt");
        player.setCurrentPosition(3);
    }
    public void setDown() {
        headAnimator.SetTrigger("Down");
        torsoAnimator.SetTrigger("Down");
        player.setCurrentPosition(0);
    }
    public void setDownTilt() {
        headAnimator.SetTrigger("Down Tilt");
        torsoAnimator.SetTrigger("Down Tilt");
        player.setCurrentPosition(1);
    }

    //legs
    public void setIdle() {
        legsAnimator.SetTrigger("Idle");
    }
    public void setWalkForward() {
        legsAnimator.SetTrigger("Walk Forward");
    }
    public void setWalkBackward() {
        legsAnimator.SetTrigger("Walk Backward");
    }

    //jumping
    public void setJump() {
        legsAnimator.SetTrigger("Jump");
    }

    public void setFall() {
        legsAnimator.SetTrigger("Fall");
    }
}
