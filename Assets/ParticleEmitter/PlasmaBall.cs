using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlasmaBall : MonoBehaviour {

    List<Particle> particles;
    public int amount;
    public float radius;
    public Plasma plasma;

    // Use this for initialization
    void Start () {

        particles = new List<Particle>();

        for (int i = 0; i < amount - 1; i++) {

            //create particles
            particles.Add((Particle)Instantiate(plasma, transform.position, Quaternion.identity));

            particles[i].transform.SetParent(transform, false);
            particles[i].transform.localPosition = new Vector3(0, 0, 0);

            //place particles
            Vector3 newPosition = Quaternion.AngleAxis(Mathf.Acos(Random.Range(0f, 2 * Mathf.PI)), particles[i].transform.localPosition)
                                     * new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0f);
            particles[i].transform.localPosition = newPosition;
        }
    }
}
