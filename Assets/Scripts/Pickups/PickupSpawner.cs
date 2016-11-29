using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour {
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject cannon;

    private GameObject pickup;

    private bool[] canSpawn;
    private float[] spawnTimes;
    public float waitBetweenSpawns = 5;

	// Use this for initialization
	void Start () {
        spawnTimes = new float[this.transform.childCount];
        canSpawn = new bool[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++) {
            spawnTimes[i] = Time.time - waitBetweenSpawns;
            canSpawn[i] = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < this.transform.childCount; i++) {
            if (canSpawn[i] == false && this.transform.GetChild(i).transform.childCount == 0) {
                canSpawn[i] = true;
                spawnTimes[i] = Time.time + waitBetweenSpawns;
            }
            
            if (spawnTimes[i] < Time.time && canSpawn[i]) {
                int rand = Random.Range(1, 100);
                if (rand < 35) {
                    pickup = (GameObject)Instantiate(pistol, this.transform.GetChild(i).transform.position, Quaternion.identity);
                    pickup.transform.parent = this.transform.GetChild(i).transform;
                    pickup.transform.position = pickup.transform.parent.position;
                }
                else if (rand < 80)
                {
                    pickup = (GameObject)Instantiate(shotgun, this.transform.GetChild(i).transform.position, Quaternion.identity);
                    pickup.transform.parent = this.transform.GetChild(i).transform;
                    pickup.transform.position = pickup.transform.parent.position;
                }
                else if (rand <= 100)
                {
                    pickup = (GameObject)Instantiate(cannon, this.transform.GetChild(i).transform.position, Quaternion.identity);
                    pickup.transform.position = new Vector3(0, 0, 0);
                    pickup.transform.parent = this.transform.GetChild(i).transform;
                    pickup.transform.position = pickup.transform.parent.position;
                }
                canSpawn[i] = false;
            }
        }
	}
}
