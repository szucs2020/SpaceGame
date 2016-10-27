/*
 * Network.cs
 * Authors: Nigel
 * Description: Initiates and controls the animators of the head, torso, and legs of the Player object.
 */
using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {
    public RuntimeAnimatorController blueHead;
    public RuntimeAnimatorController blueTorso;
    public RuntimeAnimatorController blueLegs;

    public RuntimeAnimatorController redHead;
    public RuntimeAnimatorController redTorso;
    public RuntimeAnimatorController redLegs;

    public RuntimeAnimatorController yellowHead;
    public RuntimeAnimatorController yellowTorso;
    public RuntimeAnimatorController yellowLegs;

    public RuntimeAnimatorController greenHead;
    public RuntimeAnimatorController greenTorso;
    public RuntimeAnimatorController greenLegs;


    Transform head;
    Transform torso;
    Transform legs;
    Player player;
    LobbyPlayer myPlayer;

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

        //Search for the LobbyPlayer associated with this Player.
        LobbyPlayer[] lobbyPlayers = Object.FindObjectsOfType<LobbyPlayer>();
        foreach (LobbyPlayer lobbyPlayer in lobbyPlayers) {
            if (lobbyPlayer.slot == player.playerSlot) {
                myPlayer = lobbyPlayer;
            }
        }

        if (myPlayer != null) {
            switch (myPlayer.GetTeam()) {
                case 0:
                    headAnimator.runtimeAnimatorController = blueHead as RuntimeAnimatorController;
                    torsoAnimator.runtimeAnimatorController = blueTorso as RuntimeAnimatorController; 
                    legsAnimator.runtimeAnimatorController = blueLegs as RuntimeAnimatorController;
                    break;
                case 1:
                    headAnimator.runtimeAnimatorController = redHead as RuntimeAnimatorController;
                    torsoAnimator.runtimeAnimatorController = redTorso as RuntimeAnimatorController;
                    legsAnimator.runtimeAnimatorController = redLegs as RuntimeAnimatorController;
                    break;
                case 2:
                    headAnimator.runtimeAnimatorController = yellowHead as RuntimeAnimatorController;
                    torsoAnimator.runtimeAnimatorController = yellowTorso as RuntimeAnimatorController;
                    legsAnimator.runtimeAnimatorController = yellowLegs as RuntimeAnimatorController;
                    break;
                case 3:
                    headAnimator.runtimeAnimatorController = greenHead as RuntimeAnimatorController;
                    torsoAnimator.runtimeAnimatorController = greenTorso as RuntimeAnimatorController;
                    legsAnimator.runtimeAnimatorController = greenLegs as RuntimeAnimatorController;
                    break;
            }
        }

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
