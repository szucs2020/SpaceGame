using UnityEngine;
using System.Collections;

public class Audio2D : MonoBehaviour {

    //components
    private AudioSource audio;

    //audio clips
    private AudioClip audioJump;
    private AudioClip audioBoost;
    private AudioClip audioDie;

    // Use this for initialization
    void Start () {

        audio = GetComponent<AudioSource>();

        //load clips
        audioJump = Resources.Load<AudioClip>("Audio/Player/Footstep1");
        audioBoost = Resources.Load<AudioClip>("Audio/Player/Boost");
        audioDie = Resources.Load<AudioClip>("Audio/Player/Wilhelm");
    }

    public void playJump() {
        audio.PlayOneShot(audioJump, 0.7f);
    }

    public void playBoost() {
        audio.PlayOneShot(audioBoost, 0.7f);
    }

    public void playDie() {
        audio.PlayOneShot(audioDie, 0.7f);
    }

}
