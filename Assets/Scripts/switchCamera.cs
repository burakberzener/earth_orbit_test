using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class switchCamera : MonoBehaviour {

    [SerializeField] public Camera Sat_Cam;
    [SerializeField] public Camera Main_Cam;

    int interval = 1;
    float nextTime = 0;

    private bool spawned = false;
    private float decay;

    void Start () {
        Sat_Cam.enabled = true;
        Main_Cam.enabled = false;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        Reset();

        if (Input.GetKey(KeyCode.C) && !spawned)
        {
            decay = 1f;
            spawned = true;
            //Invoke("newVoid", 1);
            Sat_Cam.enabled = !Sat_Cam.enabled;
            Main_Cam.enabled = !Main_Cam.enabled;
        }
    }
    void newVoid()
    {
        Sat_Cam.enabled = !Sat_Cam.enabled;
        Main_Cam.enabled = !Main_Cam.enabled;
    }

    private void Reset()
    {
        if (spawned && decay > 0)
        {
            decay -= Time.deltaTime;
        }
        if (decay < 0)
        {
            decay = 0;
            spawned = false;
        }
    }
}
