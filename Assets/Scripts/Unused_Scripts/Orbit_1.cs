using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit_1 : MonoBehaviour {

    [SerializeField] float a;
    [SerializeField] float b;
    [SerializeField] float c;
    [SerializeField] float alpha;
    [SerializeField] float delta_alpha;
    [SerializeField] float G;
    [SerializeField] float M;
    [SerializeField] Vector3 center;
    [SerializeField] Transform focus1;

    float r;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        center = new Vector3(focus1.position.x * c, 0, focus1.position.z);
        r = Vector3.Distance(focus1.position,transform.position);

        transform.position = new Vector3(center.x + a*Mathf.Cos(alpha),0,center.z + b*Mathf.Sin(alpha));
        c = Mathf.Sqrt(Mathf.Abs(a * a - b * b));

        delta_alpha = (Mathf.Sqrt(Mathf.Abs(2 *G*M*(1/r - 1/(2*b)))*180*Time.deltaTime)/Mathf.PI*Mathf.Sqrt(Mathf.Abs((a*a + b*b)/2)));
        alpha += delta_alpha;
	}
}