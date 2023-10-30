using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeOrbit : MonoBehaviour {

    LineRenderer line;
    [SerializeField] float alpha;
    [SerializeField] float a;
    [SerializeField] float b;

	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < line.positionCount; i++)
        {
            alpha = alpha + 360 / (line.positionCount - 1) * Mathf.Deg2Rad;
            line.SetPosition(i, new Vector3(a*Mathf.Sin(alpha), 0, b*Mathf.Cos(alpha)));
        }
        alpha = 0;
	}
}
