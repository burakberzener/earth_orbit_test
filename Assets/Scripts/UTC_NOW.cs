using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UTC_NOW : MonoBehaviour {

    float mesafe;
    int interval = 1;
    float nextTime = 0;
    public Transform hedef;
    public Text text = null;

    void Update()
    {
        if (Time.time >= nextTime)
        {
            mesafe = Vector3.Distance(transform.position, hedef.position);

            //Debug.Log(mesafe);
            Debug.Log("Satellite's Altitude = " + (mesafe-637.1)*10);

            text.text = "UTC.NOW = " + DateTime.UtcNow.ToLongDateString() + " " + DateTime.UtcNow.ToLongTimeString();
            nextTime += interval;
        }

    }
}
