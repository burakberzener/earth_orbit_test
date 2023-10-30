using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEquator : MonoBehaviour {

    float theta_scale = 0.01f;        //Set lower to add more points
    int size; //Total number of points in circle
    float radius = 700f;
    float equator_angle = 0;
    float equator_scale = 0.01f;
    float incline_angle = -0.41015237422f;

    LineRenderer lineRenderer;

    void Awake()
    {
        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        size = (int)sizeValue;
        size++;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = size;
    }

    void Update()
    {
        Vector3 pos;
        float theta = 0f;
        equator_angle = 0f;
        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);
            equator_angle += (2.0f * Mathf.PI * equator_scale);
            float x = radius * Mathf.Sin(theta);
            float z = radius * Mathf.Cos(theta)*Mathf.Cos(incline_angle);
            float y = radius * Mathf.Sin(equator_angle)*Mathf.Sin(incline_angle);

            x += gameObject.transform.position.x;
            y += gameObject.transform.position.y;
            z += gameObject.transform.position.z;

            pos = new Vector3(x, y, z);
            lineRenderer.SetPosition(i, pos);
        }
    }
}