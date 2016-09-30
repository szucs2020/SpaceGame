/********************************************************
 * Authors: Christian Szucs (with damage, score and spray done by Lajos and Lorant Polya)
 * This class controls the generi gun behaviour
********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]

public class GunP : NetworkBehaviour
{

    //public variables
    public float rpm;
    public float bulletSpeed;
    public GameObject bulletPrefab;

    //components
    public GameObject spawn;
    public AudioClip shot;
    private AudioSource audio;

    //external objects
    private Player player;

    //system variables
    private float spawnRotation;
    private float timeBetweenShots;
    private float nextShot;
    private float currentRange;

    private ParticleEmitterScript pEmitter;

    void Start()
    {

        audio = GetComponent<AudioSource>();

        timeBetweenShots = 60 / rpm;
        spawnRotation = spawn.transform.localEulerAngles.y;
        player = GetComponent<Player>();

        pEmitter = spawn.GetComponent<ParticleEmitterScript>();
    }

    [Command]
    public void CmdShoot(Vector3 direction)
    {

        //pEmitter.GenerateLaserDot(direction);
        pEmitter.GeneratePlasma(direction);
        //GameObject bullet = (GameObject)Instantiate(bulletPrefab, spawn.position, spawn.rotation);
        //bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        //NetworkServer.Spawn(bullet);

    }

    public void shoot()
    {

        if (canShoot())
        {

            spawn.transform.localEulerAngles = new Vector3(0f, spawnRotation, 0f);

            //get direction and create ray
            Vector2 direction;

            if (player.isFacingRight())
            {
                direction = new Vector2(1, 0);
            }
            else
            {
                direction = new Vector2(-1, 0);
            }

            //play gun shot sound
            audio.PlayOneShot(shot);

            //create bullet on all clients
            CmdShoot(direction);

            //set next shot time
            nextShot = Time.time + timeBetweenShots;
        }
    }

    private bool canShoot()
    {

        bool canShoot = true;

        if (Time.time < nextShot)
        {
            canShoot = false;
        }
        return canShoot;
    }
}


