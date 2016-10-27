/*
 * Particle.cs
 * Authors: Lorant
 * Description: This script is the bass class for all
 *              particle types including plasma and laser dots
 */
using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    //  Time related factors
    protected float lifeSpan;
    protected float tAlive;
    protected float startTime;

    //  Phisical size
    protected float minSize;
    protected float maxSize;
    protected float deltaSizeRate;  //  Rate/Sec to change size
    protected Vector3 prevSize = new Vector3(0.3f, 0.3f, 1f);  // original starting size
    protected bool shrink = false;

    //  Rotation
    protected float radius;
    protected float axis;

    //  Colour and such
    protected int colour;
    protected int colourRange;
    protected float minRed;
    protected float maxRed;
    protected float minBlue;
    protected float maxBlue;
    protected float minGreen;
    protected float maxGreen;
    protected float deltaTC;           // delta time colour
    protected Color a;
    protected Color b;

    //Sprite Renderer
    protected SpriteRenderer sprite;
    //  Type
    protected ParticleTypes type;
    

    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }

    public void initialize(ParticleTypes type)
    {
        this.type = type;
        if (type == ParticleTypes.Water)
        {
            lifeSpan = 10;
        }
        else if (type == ParticleTypes.Fire)
        {
            lifeSpan = 5f;
            tAlive = 0;
            minSize = 0.6f;
            maxSize = 0.4f;

            deltaSizeRate = 0.6f;
            startTime = Time.time;
        }
        else if (type == ParticleTypes.Plasma)
        {
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
        }
        else if (type == ParticleTypes.LaserDot)
        {
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
        }
    }

    private void flickerFlame()
    {
        changeParticleSize();
    }

    protected void rotateParticle()
    {
        int randRot = Random.Range(1, 4);
        Vector3 rotAxis;

        if (randRot == 1)
        {
            rotAxis = new Vector3(1, 1, 1);
        }
        else if (randRot == 2)
        {
            rotAxis = new Vector3(-1, -1, -1);
        }
        else if (randRot == 3)
        {
            rotAxis = new Vector3(-1, 1, -1);
        }
        else
        {
            rotAxis = new Vector3(1, -1, 1);
        }

        if (this.transform.parent != null)
        {
            //Statioanry
            this.transform.RotateAround(this.transform.parent.position, rotAxis * Time.deltaTime, 2);


            //Expands circle
            //this.transform.RotateAround(-(this.transform.localPosition - this.transform.parent.position), rotAxis * Time.deltaTime, 2);
            this.transform.Rotate(-rotAxis * Time.deltaTime, 2);

        }
    }

    protected void changeParticleColour(SpriteRenderer sprite)
    {

        if (Time.time - startTime > deltaTC)
        {
            a = b;
            b = new Color(Random.Range(minRed / 255f, maxRed / 255f),
                          Random.Range(minGreen / 255f, maxGreen / 255f),
                          Random.Range(minBlue / 255f, maxBlue / 255f));

            startTime = Time.time;
        }

        Color newColour = Color.LerpUnclamped(a, b, Mathf.PingPong(Time.time, deltaTC));
        sprite.material.SetColor("_Color", newColour);
    }

    protected void changeParticleSize()
    {
        if (shrink == false)
        {
            transform.localScale += new Vector3(deltaSizeRate * Time.deltaTime, deltaSizeRate * Time.deltaTime, 0f);

            if (this.transform.localScale.x >= maxSize)
            {
                shrink = true;
            }
        }
        else if (shrink == true)
        {
            transform.localScale -= new Vector3(deltaSizeRate * Time.deltaTime, deltaSizeRate * Time.deltaTime, 0f);

            if (this.transform.localScale.x <= minSize)
            {
                shrink = false;
            }
        }
    }
    public void Reset()
    {
        //this.transform.localPosition = new Vector3(0, 0, 0);
        return;
    }

    public void SetLifeSpan(float lifeSpan)
    {
        this.lifeSpan = lifeSpan;
    }

    public void SettAlive(float tAlive)
    {
        this.tAlive = tAlive;
    }

    public float GetLifespan()
    {
        return this.lifeSpan;
    }

    public float GettAlive()
    {
        return this.tAlive;
    }

    public float GetMinSize()
    {
        return this.minSize;
    }

    public float GetMaxSize()
    {
        return this.maxSize;
    }

    public ParticleTypes getType()
    {
        return this.type;
    }

    public enum ParticleTypes
    {
        Water,
        Mist,
        Bullet,
        Blood,
        Fire,
        Flame,
        Plasma,
        LaserDot,
        LaserLine
    }
}
