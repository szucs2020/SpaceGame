using UnityEngine;
using System.Collections;

public class Plasma : Particle {

	// Use this for initialization
	void Start () {
        this.type = ParticleTypes.Plasma;

        speed = 0;
        angle = 90 * Mathf.PI / 180;
        lifeSpan = 5f;
        tAlive = 0;
        minSize = Random.Range(0.01f, 0.02f);
        maxSize = Random.Range(minSize + 0.01f, minSize + 0.03f);
        deltaSizeRate = 0.02f;
        startTime = Time.time;
        minRed = 0;
        maxRed = 200;
        minBlue = 200;
        maxBlue = 255;
        minGreen = 50;
        maxGreen = 150;
        deltaTC = Random.Range(0.5f, 2f);
        Color a = Color.cyan;
        Color b = Color.blue;
        radius = 1;

        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        controlPlasma();
    }

    private void controlPlasma()
    {
        changeParticleSize();
        changeParticleColour(sprite);
        rotateParticle();
    }
}
