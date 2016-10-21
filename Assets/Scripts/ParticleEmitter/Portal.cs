using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    public GameObject target;
    public float waitTime = 2f;
    private float startTime;
    private bool wait = false;
    private Portal targetScript;

	// Use this for initialization
	void Start () {
        targetScript = target.GetComponent<Portal>();
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void Wait()
    {
        wait = true;
        startTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (target != null)
        {
            if (!wait)
            {
                if (LayerMask.LayerToName(col.gameObject.layer) != "Ground")
                {
                    col.gameObject.transform.position = target.transform.position;
                    col.gameObject.transform.position = col.gameObject.transform.position + Vector3.up * 5f;
                    targetScript.Wait();
                }
            }
            else
            {
                if (Time.time - startTime >= waitTime)
                {
                    wait = false;
                }
            }
            
        }
    }
}
