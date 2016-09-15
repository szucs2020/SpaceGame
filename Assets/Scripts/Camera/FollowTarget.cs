﻿using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -1f);


    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
