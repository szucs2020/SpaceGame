using UnityEngine;
using System.Collections.Generic;

public class ParticleEmitterScript : MonoBehaviour {

    float xPos;
    float ypos;
    float angle;
    float speed;
    float rate;
    float dt;
    float prevTime;
    float lifeSpan;
    float amount;
    int amountExists;

    List<Particle> particles;

    public Particle particle;

    // Use this for initialization
    void Start () {
        //Initialze Particle

        xPos = 0;
        ypos = 0;
        rate = 60;
        prevTime = Time.time;

        particle.initialize(Particle.ParticleTypes.Fire);

        amount = particle.GetLifespan() * rate;
        amountExists = 0;

        particles = CreateParticles((int)amount, Particle.ParticleTypes.Fire);

	}
	
    
	// Update is called once per frame
	void Update () {
        //update particle position and properties
        float velocityx;
        float velocityy;

        dt = Time.time - prevTime;

        if (dt >= rate / 60)
        {
            amountExists++;

            prevTime = Time.time;
        }

        for (int i = 0; i < amountExists && amountExists < amount; i++)
        {
            //if (i < amountExists)
            //{
            //    //Particle newParticle;
            //    //newParticle = (Particle)Instantiate(particle, transform.position, Quaternion.identity);
            //    particles.Add((Particle)Instantiate(particle, transform.position, Quaternion.identity));
            //    //particles[i] = (Particle)Instantiate(particle, transform.position, Quaternion.identity);
            //    particles[i].transform.SetParent(transform, false);
            //    particles[i].transform.localPosition = new Vector3(0, 0, 0);

            //    particles[i].initialize(Particle.ParticleTypes.Fire);
            //}

            particles[i].SettAlive(particles[i].GettAlive() + dt);

            if (particles[i].GetLifespan() >= particles[i].GettAlive())
            {
                particles[i].Reset();
            }
            velocityx = particles[i].GetSpeed() * Mathf.Cos(particles[i].GetAngle());
            velocityy = particles[i].GetSpeed() * Mathf.Sin(particles[i].GetAngle());

            particles[i].transform.Translate(velocityx * Time.deltaTime, velocityy * Time.deltaTime, 0);
            
        }

    }

    private List<Particle> CreateParticles(int amount, Particle.ParticleTypes type)
    {
        List<Particle> particles = new List<Particle>();

        for (int i = 0; i < amount - 1; i++)
        {
            particles.Add((Particle)Instantiate(particle, transform.position, Quaternion.identity));
            particles[i].transform.SetParent(transform, false);
            particles[i].transform.localPosition = new Vector3(0, 0, 0);

            particles[i].initialize(type);
        }

        return particles;
    }
}
