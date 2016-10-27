/*
 * Shotgun.cs
 * Authors: Lorant
 * Description: This script controls how the shotgun works
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Shotgun : Gun
{
    private GameObject laserDot;

    void Start()
    {
        rpm = 450;
        bulletSpeed = 45;
        init();
        laserDot = Resources.Load("LaserDot") as GameObject;
    }

    public GameObject[] generateShotgunShells(Vector2 direction, Vector2 position)
    {
        GameObject[] lasers = new GameObject[5];

        for (int i = 0; i < 5; i++)
        {
            lasers[i] = (GameObject)Instantiate(laserDot, position, Quaternion.identity);
        }

        return lasers;
    }


    [Command]
    public override void CmdShoot(Vector2 direction, Vector2 position)
    {
        GameObject[] lasers;
        lasers = generateShotgunShells(direction, position);

        for (int i = 0; i < lasers.Length; i++)
        {
            if (i % 2 == 0)
            {
                lasers[i].GetComponent<Rigidbody2D>().velocity = bulletSpeed * (Quaternion.AngleAxis(i * 1.5f, Vector3.back) * direction);

            }
            else
            {
                lasers[i].GetComponent<Rigidbody2D>().velocity = bulletSpeed * (Quaternion.AngleAxis(i * -1.5f, Vector3.back) * direction);
            }
            lasers[i].GetComponent<LaserDot>().bulletOwner = GetComponent<Player>();
            NetworkServer.Spawn(lasers[i].gameObject);
        }
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


