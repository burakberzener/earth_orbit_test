using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {

    float mesafe;
    int interval = 1;
    float nextTime = 0;
    public Transform hedef;
    public Text text = null;

    void Start () {
        Debug.Log("I'm Alive!");
    }

	void Update () {
        if (Time.time >= nextTime)
        {
            mesafe = Vector3.Distance(transform.position, hedef.position);

            Debug.Log(mesafe);
            Debug.Log(mesafe);
            text.text = "UTC.NOW = " + DateTime.UtcNow.ToLongDateString() + " " + DateTime.UtcNow.ToLongTimeString();
            nextTime += interval;
        }
    }
}