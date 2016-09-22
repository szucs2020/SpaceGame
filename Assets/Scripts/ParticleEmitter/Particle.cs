using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {
    
    //  Time related factors
    private float lifeSpan;
    private float tAlive;
    float startTime;

    //  Phisical size
    private float minSize;
    private float maxSize;
    private float deltaSizeRate;  //  Rate/Sec to change size
    Vector3 prevSize = new Vector3(0.3f, 0.3f, 1f);  // original starting size
    private bool shrink = false;

    //  Rotation
    private float radius;
    private float speed;
    private float axis;
    private float angle;

    //  Colour and such
    private int colour;
    private int colourRange;
    private float minRed;
    private float maxRed;
    private float minBlue;
    private float maxBlue;
    private float minGreen;
    private float maxGreen;
    private float deltaTC;           // delta time colour
    Color a;
    Color b;

    //Sprite Renderer
    Renderer sprite;
    //  Type
    private ParticleTypes type;
    

    void Start()
    {
        //print("particle script " + this.transform.name);
        if (this.transform.name == "PlasmaBall(Clone)")
        {
            initialize(ParticleTypes.Plasma);
            sprite = this.GetComponent<SpriteRenderer>();

            //Physics2D.IgnoreLayerCollision(11, 11);
            ////this.transform.GetComponent<Collider2D>().enabled = false;
            //this.transform.GetComponent<Collider2D>().enabled = true;
        } 
    }

    void Update()
    {
        if (this.type == ParticleTypes.Fire)
        {
            flickerFlame();
        }
        else if (this.type == ParticleTypes.Plasma)
        {
            controlPlasma();
        }

    }

    public void initialize(ParticleTypes type)
    {
        this.type = type;
        if (type == ParticleTypes.Water)
        {
            speed = 10;
            angle = 90 * Mathf.PI / 180;
            lifeSpan = 10;
        }
        else if (type == ParticleTypes.Fire)
        {
            speed = 5;
            angle = 90 * Mathf.PI / 180;
            lifeSpan = 5f;
            tAlive = 0;
            minSize = 0.6f;
            maxSize = 0.4f;

            deltaSizeRate = 0.6f;
            startTime = Time.time;
        }
        else if(type == ParticleTypes.Plasma)
        {
            speed = 0;
            angle = 90 * Mathf.PI / 180;
            lifeSpan = 5f;
            tAlive = 0;
            minSize = Random.Range(0.01f, 0.02f);
            maxSize = Random.Range(minSize+0.01f, minSize+0.03f);
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
    }

    private void flickerFlame()
    {
        changeParticleSize();
    }

    private void controlPlasma()
    {
        changeParticleSize();
        changeParticleColour();
        rotateParticle();
    }

    private void rotateParticle()
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
            this.transform.RotateAround(-(this.transform.localPosition - this.transform.parent.position), rotAxis * Time.deltaTime, 2);
            this.transform.Rotate(-rotAxis * Time.deltaTime, 2);

        }
    }

    private void changeParticleColour()
    {

        if (Time.time - startTime > deltaTC)
        {
            a = b;
            b = new Color(Random.Range(minRed / 255f, maxRed / 255f),
                          Random.Range(minGreen / 255f, maxGreen / 255f),
                          Random.Range(minBlue / 255f, maxBlue / 255f));

            startTime = Time.time;
        }

        Color newColour = Color.Lerp(a, b, Mathf.PingPong(Time.time, deltaTC));
        sprite.material.SetColor("_Color", newColour);
    }

    private void changeParticleSize()
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
        Plasma
    }
}
