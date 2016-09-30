using UnityEngine;
using System.Collections.Generic;

public class ParticleEmitterScript : MonoBehaviour {

    float xPos;
    float ypos;
    float angle;
    float velocity;
    float rate;
    float dt;
    float prevTime;
    float lifeSpan;
    float amount;
    float radius;
    int amountExists;
    Particle.ParticleTypes particleType;
    GameObject centrePoint;
    CircleCollider2D particleCollider;

    List<Particle> particles;

    public LaserDot laserDot;
    public Plasma plasma;

    // Use this for initialization
    void Start () {
        //Initialze Particle
        //particle.initialize(Particle.ParticleTypes.LaserDot);
        //particleType = particle.getType();
        //print("Emmiter " + particleType);

        //laserDot.initialize(Particle.ParticleTypes.Plasma);
        plasma.initialize(Particle.ParticleTypes.Plasma);

        // fire... refactor later
        if (particleType == Particle.ParticleTypes.Fire)
        {
            xPos = 0;
            ypos = 0;
            rate = 60;
            prevTime = Time.time;

            angle = 90 * Mathf.PI / 180; ;
            velocity = 10;

            //particle.initialize(Particle.ParticleTypes.Fire);

            //amount = particle.GetLifespan() * rate;
            amountExists = 0;

            CreateParticles((int)amount);
        }
        else if (particleType == Particle.ParticleTypes.Plasma)
        {
            //angle = 360 * Mathf.PI / 180; ;
            //speed = 20f;

            //particle.initialize(particle.getType());

            amount = 50;
            amountExists = 0;

            centrePoint = new GameObject();
            centrePoint.transform.position = this.transform.position;
            particleCollider = centrePoint.AddComponent<CircleCollider2D>();
            radius = 2f;
            particleCollider.radius = radius;

            centrePoint.AddComponent<Rigidbody2D>();

            //CreateParticles((int)amount);
            //placeParticles();
        }
        else if (particleType == Particle.ParticleTypes.LaserDot)
        {
            angle = 360 * Mathf.PI / 180; ;
            velocity = 50f;
            print(velocity);

            radius = 0.5f;
            

            //CreateParticle();
        }


    }
	
    
	// Update is called once per frame
	void Update () {
        ////update particle position and properties
        //float velocityx;
        //float velocityy;

        //dt = Time.time - prevTime;

        //if (particleType == Particle.ParticleTypes.Fire)
        //{
        //    if (dt >= rate / 60)
        //    {
        //        amountExists++;

        //        prevTime = Time.time;
        //    }

        //    for (int i = 0; i < amountExists && amountExists < amount; i++)
        //    {

        //        particles[i].SettAlive(particles[i].GettAlive() + dt);

        //        if (particles[i].GetLifespan() >= particles[i].GettAlive())
        //        {
        //            particles[i].Reset();
        //        }
        //        velocityx = speed * Mathf.Cos(angle);
        //        velocityy = speed * Mathf.Sin(angle);

        //        //  Old way
        //        //velocityx = particles[i].GetSpeed() * Mathf.Cos(particles[i].GetAngle());
        //        //velocityy = particles[i].GetSpeed() * Mathf.Sin(particles[i].GetAngle());

        //        //centrePoint.Set(velocityx * Time.deltaTime, velocityy * Time.deltaTime, 0);
        //        //particles[i].transform.Translate(centrePoint.x, centrePoint.y, centrePoint.z);

        //    }
        //}
        //else if (particleType == Particle.ParticleTypes.Plasma)
        //{


        //        //particles[i].SettAlive(particles[i].GettAlive() + dt);

        //        //if (particles[i].GetLifespan() >= particles[i].GettAlive())
        //        //{
        //        //    particles[i].Reset();
        //        //}

        //        velocityx = speed * Mathf.Cos(angle);
        //        velocityy = speed * Mathf.Sin(angle);

        //        //velocityx = particles[i].GetSpeed() * Mathf.Cos(particles[i].GetAngle());
        //        //velocityy = particles[i].GetSpeed() * Mathf.Sin(particles[i].GetAngle());

        //        //Particle Movement left, right, up, down
        //        centrePoint.transform.Translate(velocityx * Time.deltaTime, velocityy * Time.deltaTime, 0);

        //        //Particle Rotation along an axis
        //        //particles[i].transform.RotateAround(-(particles[i].transform.localPosition - centrePoint.transform.position), Vector3.back * Time.deltaTime, 2);
        //    //transform.Rotate(Vector3.back * Time.deltaTime * 180);
        //}
        

    }

    public void GeneratePlasma(Vector3 direction)
    {
        amount = 50;
        amountExists = 0;
        velocity = 20;

        centrePoint = new GameObject();
        centrePoint.transform.position = this.transform.position;
        particleCollider = centrePoint.AddComponent<CircleCollider2D>();
        radius = 2f;
        particleCollider.radius = radius;

        centrePoint.AddComponent<Rigidbody2D>();

        CreateParticles((int)amount);
        placeParticles();



        centrePoint.GetComponent<Rigidbody2D>().velocity = velocity * direction;
    }

    public void GenerateLaserDot(Vector3 direction)
    {
        velocity = 20;
        LaserDot laser;
        //laserDot.initialize(Particle.ParticleTypes.LaserDot);
        laser = (LaserDot)CreateParticle((Particle)laserDot);
        //laser = (LaserDot)Instantiate(laserDot, transform.position, Quaternion.identity);
        //particle.initialize(particle.getType());
        laser.GetComponent<Rigidbody2D>().velocity = velocity * direction;
    }

    private Particle CreateParticle(Particle particle)
    {
        return (Particle)Instantiate(particle, transform.position, Quaternion.identity);
    }

    private void CreateParticles(int amount)
    {
        particles = new List<Particle>();

        for (int i = 0; i < amount - 1; i++)
        {
            particles.Add((Particle)Instantiate(plasma, transform.position, Quaternion.identity));
            
            particles[i].transform.SetParent(centrePoint.transform, false);
            particles[i].transform.localPosition = new Vector3(0, 0, 0);

            //particles[i].initialize(type);
        }
    }

    private void placeParticles()
    {
        for (int i = 0; i < amount - 1; i++)
        {
            Vector3 newPosition = Quaternion.AngleAxis(Mathf.Acos(Random.Range(0f, 2 * Mathf.PI)), particles[i].transform.localPosition)
                                     * new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0f);

            particles[i].transform.localPosition = newPosition;
            //particles[i].transform.localPosition.Set(newPosition.x, newPosition.y, newPosition.z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print(collision.collider);
    }

}
