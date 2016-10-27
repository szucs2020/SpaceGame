﻿using UnityEngine;
using System.Collections;

public class LaserDot : Particle {

    public Player bulletOwner;
    private bool hurtSelf;

	// Use this for initialization
	void Start () {
        this.type = ParticleTypes.LaserDot;

        lifeSpan = 5f;
        tAlive = 0;
        startTime = Time.time;
        minRed = 150;
        maxRed = 255;
        minBlue = 0;
        maxBlue = 50;
        minGreen = 0;
        maxGreen = 50;
        deltaTC = Random.Range(0.5f, 2f);
        Color a = Color.red;
        Color b = Color.red;

        sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.red;

        hurtSelf = false;

        destroy();
    }
	
	// Update is called once per frame
	void Update () {
        //changeParticleColour(sprite);
	}

    private void destroy()
    {
        Destroy(this.gameObject, lifeSpan);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Ground") {
            Destroy(gameObject);
            Instantiate(Resources.Load("Spark"), transform.position, Quaternion.identity);
        } else if (LayerMask.LayerToName(col.gameObject.layer) == "Player") {
            Instantiate(Resources.Load("Spark"), transform.position, Quaternion.identity);
            Destroy(gameObject);
            if (col.gameObject.GetComponent<Player>().netId != bulletOwner.netId || hurtSelf == true) {
                col.gameObject.GetComponent<Health>().Damage(5.0f);
            }
        }
        else if (col.gameObject.tag == "Portal")
        {
            hurtSelf = true;
        }
    }
}
