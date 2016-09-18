using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    private float speed;
    private float angle;
    private float lifeSpan;
    private float tAlive;
    private float minSize;
    private float maxSize;
    float startTime;
    Vector3 prevSize = new Vector3(0.3f, 0.3f, 1f);
    private bool shrink = false;


    void Update()
    {
        flickerFlame();
    }

    public void initialize(ParticleTypes type)
    {
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
            minSize = 0.3f;
            maxSize = 0.2f;
            startTime = Time.time;
        }
    }

    private void flickerFlame()
    {
        if (shrink == false)
        {
            transform.localScale += new Vector3(0.6f * Time.deltaTime, 0.6f * Time.deltaTime, 0f);

            if (this.transform.localScale.x >= 0.6f)
            {
                shrink = true;
            }
        }
        else if (shrink == true)
        {
            transform.localScale -= new Vector3(0.6f * Time.deltaTime, 0.6f * Time.deltaTime, 0f);

            if (this.transform.localScale.x <= 0.4f)
            {
                shrink = false;
            }
        }
    }

    public void Reset()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        return;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
    }

    public void SetLifeSpan(float lifeSpan)
    {
        this.lifeSpan = lifeSpan;
    }

    public void SettAlive(float tAlive)
    {
        this.tAlive = tAlive;
    }

    public float GetSpeed()
    {
        return this.speed;
    }

    public float GetAngle()
    {
        return this.angle;
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

    public enum ParticleTypes
    {
        Water,
        Mist,
        Bullet,
        Blood,
        Fire,
        Flame
    }
}
