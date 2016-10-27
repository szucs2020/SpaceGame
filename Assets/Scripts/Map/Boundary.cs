﻿using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Player")
        {
            col.gameObject.GetComponent<Health>().Kill();
        }
    }

}

