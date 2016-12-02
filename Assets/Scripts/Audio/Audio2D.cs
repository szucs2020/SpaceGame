/*
 * Audio2D.cs
 * Authors: Christian
 * Description: This script allows us to play non-spatial sounds 
 */
using UnityEngine;
using System.Collections;

public class Audio2D : MonoBehaviour {

    //components
    private AudioSource audio2D;

    //audio clips
    private AudioClip audioJump;
    private AudioClip audioBoost;
    private AudioClip audioDie;
    private AudioClip audioPistol;

    // Use this for initialization
    void Start () {

        audio2D = GetComponent<AudioSource>();

        //load clips
        audioJump = Resources.Load<AudioClip>("Audio/Player/Footstep1");
        audioBoost = Resources.Load<AudioClip>("Audio/Player/Boost");
        audioDie = Resources.Load<AudioClip>("Audio/Player/Wilhelm");
        audioPistol = Resources.Load<AudioClip>("Audio/Weapons/PistolZap");
    }

    public void PlayShootPistol() {
        audio2D.PlayOneShot(audioPistol, 0.7f);
    }

    public void playJump() {
        audio2D.PlayOneShot(audioJump, 0.7f);
    }

    public void playBoost() {
        audio2D.PlayOneShot(audioBoost, 0.7f);
    }

    public void playDie() {
        audio2D.PlayOneShot(audioDie, 0.7f);
    }

}
