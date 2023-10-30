using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class tle2orbitalelements : MonoBehaviour {

    public TLE_Data_Read TLE_Data_Read;

    Orbital_Elements oe = new Orbital_Elements();

    double to_radian = Mathf.PI / 180;

    double mu_of_earth = 3.98601 * Mathf.Pow(10,5);
    
    void Start () {
        TLE_Data_Read.readTextFile("Assets/TLEs/VANGUARD1_TLE.txt");
        toOrbitalElements();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void toOrbitalElements()
    {
        TLE_Elements tle2ob = TLE_Data_Read.tle2ob;
        consoleTLE(tle2ob);
        oe.Inclination = tle2ob.Inclination * to_radian;
        oe.RAAN = tle2ob.RAAN * to_radian;
        oe.Eccentricity = tle2ob.Eccentricity * to_radian;
        oe.Arg_of_Perigee = tle2ob.Arg_of_Perigee * to_radian;
        oe.Mean_Anomaly = tle2ob.Mean_Anomaly * to_radian;
        oe.Mean_Motion = (tle2ob.Mean_Motion * 2 * Mathf.PI) / (24 * 3600);
        oe.Semi_Major_Axis = Mathf.Pow(Convert.ToSingle(mu_of_earth/(oe.Mean_Motion*oe.Mean_Motion)),1/3);
    }

    void consoleTLE(TLE_Elements tle2ob)
    {
        Debug.Log("Catalog Number :" + tle2ob.Catalog_Number);
        Debug.Log("Designation Number :" + tle2ob.Designation_Number);
        Debug.Log("Epoch Year :" + tle2ob.Epoch_Year);
        Debug.Log("Epoch Date :" + tle2ob.Epoch_Date);
        Debug.Log("Epoch Day :" + tle2ob.Epoch_Day);
        Debug.Log("Epoch Hour :" + tle2ob.Epoch_Hour);
        Debug.Log("Epoch Minute :" + tle2ob.Epoch_Minute);
        Debug.Log("First Derivative of MM :" + tle2ob.First_DoMM);
        Debug.Log("Second Derivative of MM :" + tle2ob.Second_DoMM);
        Debug.Log("BSTAR :" + tle2ob.BSTAR);
        Debug.Log("Ephemesis Type :" + tle2ob.Ephemesis_Type);
        Debug.Log("Element Set Number :" + tle2ob.Element_Set_Number);
        Debug.Log("Firstline Checksum :" + tle2ob.Firstline_Checksum);

        Debug.Log("Inclination :" + tle2ob.Inclination);
        Debug.Log("RAAN :" + tle2ob.RAAN);
        Debug.Log("Eccentricity :" + tle2ob.Eccentricity);
        Debug.Log("Arg. of Perigee :" + tle2ob.Arg_of_Perigee);
        Debug.Log("Mean Anomaly :" + tle2ob.Mean_Anomaly);
        Debug.Log("Mean Motion :" + tle2ob.Mean_Motion);
        Debug.Log("Rev. Num. Epoch :" + tle2ob.Rev_Num_Epoch);
        Debug.Log("Checksum :" + tle2ob.Secondline_Checksum);
    }
}
