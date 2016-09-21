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
    float radius;
    int amountExists;
    Particle.ParticleTypes particleType;
    GameObject centrePoint;

    List<Particle> particles;

    public Particle particle;

    // Use this for initialization
    void Start () {
        //Initialze Particle
        //particle.initialize(Particle.ParticleTypes.Plasma);
        particleType = particle.getType();
        //print("Emmiter " + particleType);

        centrePoint = new GameObject();
        centrePoint.transform.position = this.transform.position;

        // fire... refactor later
        if (particleType == Particle.ParticleTypes.Fire)
        {
            xPos = 0;
            ypos = 0;
            rate = 60;
            prevTime = Time.time;

            angle = 90 * Mathf.PI / 180; ;
            speed = 10;

            particle.initialize(Particle.ParticleTypes.Fire);

            amount = particle.GetLifespan() * rate;
            amountExists = 0;

            CreateParticles((int)amount, Particle.ParticleTypes.Fire);
        }
        else if (particleType == Particle.ParticleTypes.Plasma)
        {
            xPos = 0;
            ypos = 0;
            prevTime = Time.time;

            angle = 360 * Mathf.PI / 180; ;
            speed = 0.1f;

            particle.initialize(particle.getType());

            amount = 100;
            amountExists = 0;

            radius = 2f;

            CreateParticles((int)amount, particle.getType());
            placeParticles();
        }

	}
	
    
	// Update is called once per frame
	void Update () {
        //update particle position and properties
        float velocityx;
        float velocityy;

        dt = Time.time - prevTime;

        if (particleType == Particle.ParticleTypes.Fire)
        {
            if (dt >= rate / 60)
            {
                amountExists++;

                prevTime = Time.time;
            }

            for (int i = 0; i < amountExists && amountExists < amount; i++)
            {

                particles[i].SettAlive(particles[i].GettAlive() + dt);

                if (particles[i].GetLifespan() >= particles[i].GettAlive())
                {
                    particles[i].Reset();
                }
                velocityx = speed * Mathf.Cos(angle);
                velocityy = speed * Mathf.Sin(angle);

                //  Old way
                //velocityx = particles[i].GetSpeed() * Mathf.Cos(particles[i].GetAngle());
                //velocityy = particles[i].GetSpeed() * Mathf.Sin(particles[i].GetAngle());

                //centrePoint.Set(velocityx * Time.deltaTime, velocityy * Time.deltaTime, 0);
                //particles[i].transform.Translate(centrePoint.x, centrePoint.y, centrePoint.z);

            }
        }
        else if (particleType == Particle.ParticleTypes.Plasma)
        {
            for (int i = 0; i < amount - 1; i++)
            {

                particles[i].SettAlive(particles[i].GettAlive() + dt);

                if (particles[i].GetLifespan() >= particles[i].GettAlive())
                {
                    particles[i].Reset();
                }

                velocityx = speed * Mathf.Cos(angle);
                velocityy = speed * Mathf.Sin(angle);

                //velocityx = particles[i].GetSpeed() * Mathf.Cos(particles[i].GetAngle());
                //velocityy = particles[i].GetSpeed() * Mathf.Sin(particles[i].GetAngle());

                //Particle Movement left, right, up, down
                centrePoint.transform.Translate(velocityx * Time.deltaTime, velocityy * Time.deltaTime, 0);

                //Particle Rotation along an axis
                particles[i].transform.RotateAround(-(particles[i].transform.localPosition - centrePoint.transform.position), Vector3.back, 2);
            }

            //transform.Rotate(Vector3.back * Time.deltaTime * 180);
        }
        

    }

    private void CreateParticles(int amount, Particle.ParticleTypes type)
    {
        particles = new List<Particle>();

        for (int i = 0; i < amount - 1; i++)
        {
            particles.Add((Particle)Instantiate(particle, transform.position, Quaternion.identity));
            
            particles[i].transform.SetParent(centrePoint.transform, false);
            particles[i].transform.localPosition = new Vector3(0, 0, 0);

            particles[i].initialize(type);
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
}
