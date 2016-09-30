using UnityEngine;
using System.Collections;

public class LaserDot : Particle {

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
}
