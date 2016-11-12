﻿using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    public float ascendHeigt;
    public float descendHeight;
    private bool ascend = false;
    private bool descend = true;

	void Start () {
	
	}
	
	void Update () {
        Vector3 pos = transform.position;
        pos.y = pos.y - 5;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.down, descendHeight+1);

        if (ascend)
        {
            transform.Translate(Vector2.up * Time.deltaTime);
        }
        else if (descend)
        {
            transform.Translate(Vector2.down * Time.deltaTime);
        }
        if (hit) {
            if (hit.collider.gameObject.layer == 8 && hit.distance >= descendHeight && ascend)
            {
                descend = true;
                ascend = false;
            }
            else if (hit.collider.gameObject.layer == 8 && hit.distance <= ascendHeigt && descend)
            {
                descend = false;
                ascend = true;
            }
        }
        
	}
}
