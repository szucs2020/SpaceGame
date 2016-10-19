using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    public GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        print("Hit");
        if (target != null)
        {
            other.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0);
        }
        
    }
}
