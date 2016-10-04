/********************************************************
 * Authors: 
********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pistol : Gun
{
    public ParticleEmitterScript pEmitter;

    void Start()
    {
        init();
    }


    [Command]
    public override void CmdShoot(Vector2 direction, Vector2 position)
    {
        LaserDot laser;
        laser = pEmitter.GenerateLaserDot(position);

        laser.GetComponent<Rigidbody2D>().velocity = bulletSpeed * direction;
        NetworkServer.Spawn(laser.gameObject);
    }

    public override void shoot()
    {

        if (canShoot())
        {

            spawn.localEulerAngles = new Vector3(0f, spawn.localEulerAngles.y, 0f);

            Vector2 position = getSpawn().transform.position;
            Vector2 direction;
            Quaternion rotation = getSpawn().transform.rotation;

            if (player.isFacingRight())
            {
                direction = rotation * Vector2.right;
            }
            else
            {
                rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z * -1);
                direction = rotation * Vector2.left;
            }

            //create bullet on all clients
            CmdShoot(direction, position);

            //set next shot time
            nextShot = Time.time + timeBetweenShots;
        }
    }
}


