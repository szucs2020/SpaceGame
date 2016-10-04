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

    List<Particle> particles;

    public LaserDot laserDot;
    public Plasma plasma;
    public GameObject plasmaAnchor;

    // Use this for initialization
    void Start () {
        //Initialze Particle
        //particle.initialize(Particle.ParticleTypes.LaserDot);
        //particleType = particle.getType();
        //print("Emmiter " + particleType);

        //laserDot.initialize(Particle.ParticleTypes.Plasma);
        //plasma.initialize(Particle.ParticleTypes.Plasma);

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

            //CreateParticles((int)amount);
        }
        else if (particleType == Particle.ParticleTypes.Plasma)
        {
            //angle = 360 * Mathf.PI / 180; ;
            //speed = 20f;

            //particle.initialize(particle.getType());

            //amount = 50;
            //amountExists = 0;

            //centrePoint = new GameObject();
            //centrePoint.transform.position = this.transform.position;
            //particleCollider = centrePoint.AddComponent<CircleCollider2D>();
            //radius = 2f;
            //particleCollider.radius = radius;

            //centrePoint.AddComponent<Rigidbody2D>();

            //CreateParticles((int)amount);
            //placeParticles();
        }
        else if (particleType == Particle.ParticleTypes.LaserDot)
        {
            //angle = 360 * Mathf.PI / 180; ;
            //velocity = 50f;
            //print(velocity);

            //radius = 0.5f;
            

            //CreateParticle();
        }


    }
	
    
	// Update is called once per frame
	void Update () {

    }

    public LaserDot[] GenerateShotgunShells(Vector2 direction, Vector2 position)
    {
        LaserDot[] lasers = new LaserDot[5];
        Vector2 newDir;

        for (int i = 0; i < 5; i++)
        {
            newDir = Quaternion.AngleAxis(Random.Range(-10, 10), Vector3.back) * direction;
            lasers[i] = (LaserDot)CreateParticle((Particle)laserDot, position);
            lasers[i].direction = newDir;
            //lasers[i].GetComponent<Rigidbody2D>().velocity = velocity * newDir;
        }

        return lasers;
    }

    public GameObject GeneratePlasma(Vector2 position)
    {
        amount = 50;
        radius = 2;

        GameObject centrePoint = (GameObject)Instantiate(plasmaAnchor, position, Quaternion.identity);

        CreateParticles((int)amount, centrePoint);
        placeParticles();

        return centrePoint;
    }

    public LaserDot GenerateLaserDot(Vector2 position)
    {
        LaserDot laser;

        laser = (LaserDot)CreateParticle((Particle)laserDot, position);

        return laser;
    }

    private Particle CreateParticle(Particle particle, Vector2 position)
    {
        return (Particle)Instantiate(particle, position, Quaternion.identity);
    }

    private void CreateParticles(int amount, GameObject anchor)
    {
        particles = new List<Particle>();

        for (int i = 0; i < amount - 1; i++)
        {
            particles.Add((Particle)Instantiate(plasma, transform.position, Quaternion.identity));
            
            particles[i].transform.SetParent(anchor.transform, false);
            particles[i].transform.localPosition = new Vector3(0, 0, 0);
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
