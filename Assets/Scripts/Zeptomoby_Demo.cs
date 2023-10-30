using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Zeptomoby.OrbitTools;

public class Zeptomoby_Demo : MonoBehaviour {

    [SerializeField] public Transform Zarya_Pos;
    [SerializeField] public Transform Earth_Rotation;
    [SerializeField] public Transform Satellite_Camera;
    [SerializeField] public GameObject Camera_Target;

    public LineRenderer lineRenderer;

    public Text eci_coord_text = null; // ECI Coordinates Text
    public Text gmst_text = null;

    Quaternion yRotation;
    Quaternion xRotation;
    Quaternion zRotation;
    Vector3 c_rot;

    int interval = 1;
    float nextTime = 0;
    double gmst = 0;
    double utcJulian = 0;
    double utcJ2000 = 0;
    float latitude,longitude;
    const double f = 0.00335287;

    private string tle_data_1;
    private string tle_data_2;

    DateTime utcNow;

    string TLEFromSpaceTrack;

    void Start ()
    {
        TLEFromSpaceTrack = PlayerPrefs.GetString("Name");

        foreach (var eachString in TLEFromSpaceTrack.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (eachString.Substring(0, 8) == "1 39030U")
            {
                tle_data_1 = eachString;
            }
            else if (eachString.Substring(0, 8) == "2 39030 ")
            {
                tle_data_2 = eachString;
            }

        }

        printPosVel(tle_data_1,tle_data_2);
    }

    void Update()
    {
        //ISS Position Tracking**********************************************
        if (Time.time >= nextTime)
        {
            utcNow = DateTime.UtcNow;

            var aStringBuilder = new StringBuilder(tle_data_1);
            var aStringBuilder_2 = new StringBuilder(tle_data_2);
            aStringBuilder.Remove(9, 6);
            aStringBuilder.Insert(9, "      ");
            tle_data_1 = aStringBuilder.ToString();
            aStringBuilder_2.Insert(64, "  ");
            tle_data_2 = aStringBuilder_2.ToString();

            string str1 = "SGP4 Test";
            string str2 = tle_data_1;
            string str3 = tle_data_2;

            Tle tle = new Tle(str1, str2, str3);

            Satellite sat = new Satellite(tle);

            Debug.Log("TLE Epoch = " + tle.Epoch + " Epoch Time = " + tle.EpochJulian.ToTime() + "Epoch Julian = " + tle.EpochJulian.FromJan1_12h_2000());
            TimeSpan duration = utcNow - tle.EpochJulian.ToTime();
            Debug.Log("Epoch Total Minutes = " + duration.TotalMinutes);

            Eci Zarya_Eci = sat.PositionEci(duration.TotalMinutes);

            Zarya_Pos.position = new Vector3(Convert.ToSingle(Zarya_Eci.Position.X / 10),
                                             Convert.ToSingle(-Zarya_Eci.Position.Y / 10),
                                             Convert.ToSingle(Zarya_Eci.Position.Z / 10));

            gmst = earthRotation(utcNow);

            latitude = Mathf.Atan(Convert.ToSingle(Zarya_Eci.Position.Z) /
                                   Mathf.Sqrt(Convert.ToSingle(Zarya_Eci.Position.X) * Convert.ToSingle(Zarya_Eci.Position.X) +
                                              Convert.ToSingle(Zarya_Eci.Position.Y) * Convert.ToSingle(Zarya_Eci.Position.Y)))*180/Mathf.PI;
            longitude = Mathf.Atan(Convert.ToSingle(Zarya_Eci.Position.Y)/ Convert.ToSingle(Zarya_Eci.Position.X))*180/Mathf.PI - Convert.ToSingle(gmst) + 360;

            Debug.Log("Latitude = " + latitude);
            Debug.Log("Longitude = " + longitude);

            eci_coord_text.text = "Satellite ECI Coordinates" + 
                            System.Environment.NewLine + "X = " + Zarya_Eci.Position.X +
                            System.Environment.NewLine + "Y = " + Zarya_Eci.Position.Y +
                            System.Environment.NewLine + "Z = " + Zarya_Eci.Position.Z;

            Satellite_Camera.position = new Vector3(Zarya_Pos.position.x + 40 * Zarya_Pos.position.x / Mathf.Abs(Zarya_Pos.position.x),
                                                    Zarya_Pos.position.y + 40 * Zarya_Pos.position.y / Mathf.Abs(Zarya_Pos.position.y),
                                                    Zarya_Pos.position.z + 40 * Zarya_Pos.position.z / Mathf.Abs(Zarya_Pos.position.z));

            nextTime += interval;
        }
    }

    void LateUpdate()
    {
        Satellite_Camera.transform.LookAt(Camera_Target.transform);
    }

    public void Main()
    {
        // Test SGP4
        //string str1 = "SGP4 Test";
        //string str2 = "1 25544U          19359.88213810  .00000012  00000-0  82355-5 0  9999";
        //string str3 = "2 25544  51.6422 130.9828 0007766  72.8131  70.4402 15.50127765   20499";

        //Make a function for that chapter
        //readTleData(DocumentsPath + "/" + file_path);
        
        
    }

    public void printPosVel(string tle_data_1,string tle_data_2)
    {
        var aStringBuilder = new StringBuilder(tle_data_1);
        var aStringBuilder_2 = new StringBuilder(tle_data_2);
        aStringBuilder.Remove(9, 6);
        aStringBuilder.Insert(9, "      ");
        tle_data_1 = aStringBuilder.ToString();
        aStringBuilder_2.Insert(64, "  ");
        tle_data_2 = aStringBuilder_2.ToString();
        //********************************

        Debug.Log("TLE1" + tle_data_1);
        Debug.Log("TLE2" + tle_data_2);

        string str1 = "SGP4 Test";
        string str2 = tle_data_1;
        string str3 = tle_data_2;

        Tle tle = new Tle(str1, str2, str3);

        const int Step = 1;

        Satellite sat = new Satellite(tle);
        List<Eci> coords = new List<Eci>();

        for (int mpe = 0; mpe <= (Step * 400); mpe += Step)
        {
            Eci eci = sat.PositionEci(mpe);

            coords.Add(eci);
        }

        //Debug.Log("{0}\n" + tle.Name);
        //Debug.Log("{0}\n" + tle.Line1); 
        //Debug.Log("{0}\n" + tle.Line2);

        //Debug.Log("\n  TSINCE            X                Y                Z\n\n");

        for (int i = 0; i < (coords.Count-1); i++)
        {
            Eci e = coords[i] as Eci;

            //Debug.Log(i * Step + "  X = " +
            //           e.Position.X + "  Y=" +
            //           e.Position.Y + "  Z=" +
            //           e.Position.Z);

            lineRenderer.SetPosition(i, new Vector3(Convert.ToSingle(e.Position.X / 10), Convert.ToSingle(-e.Position.Y / 10), Convert.ToSingle(e.Position.Z / 10)));
        }
        
        //Debug.Log("\n                  XDOT             YDOT             ZDOT\n\n");

        for (int i = 0; i < coords.Count; i++)
        {
            Eci e = coords[i] as Eci;

            //Debug.Log("Vx"+ e.Velocity.X+"  " +
            //    "Vy" + e.Velocity.Y+ "  " +
            //    "Vz" + e.Velocity.Z);
        }
    }

    public double toGmst(double utcJulian, double utcJ2000)
    {
        double UT = (utcJulian + 0.5) % 1.0;
        double TU = (utcJ2000 - UT) / 36525.0;

        double GMST = 24110.54841 + TU *
                      (8640184.812866 + TU * (0.093104 - TU * 6.2e-06));

        GMST = (GMST + Globals.SecPerDay * Globals.OmegaE * UT) % Globals.SecPerDay;

        if (GMST < 0.0)
        {
            GMST += Globals.SecPerDay;  // "wrap" negative modulo value
        }

        return (Globals.TwoPi * (GMST / Globals.SecPerDay));
    }

    public double earthRotation(DateTime utcNow)
    {
        utcJulian = utcNow.ToOADate() + 2415018.5;
        utcJ2000 = utcJulian - 2451545.0;
        gmst = toGmst(utcJulian, utcJ2000); // Radian GMST
        gmst = (gmst * 180) / Mathf.PI; // Degree GMST
        gmst_text.text = "GMST = " + Convert.ToString(gmst) + " in Degree";
        c_rot = new Vector3(0, 0, Convert.ToSingle(-gmst));
        Quaternion yRotation = Quaternion.AngleAxis(c_rot.y, Vector3.up);
        Quaternion xRotation = Quaternion.AngleAxis(c_rot.x, Vector3.right);
        Quaternion zRotation = Quaternion.AngleAxis(c_rot.z, Vector3.forward);
        Earth_Rotation.transform.rotation = yRotation * xRotation * zRotation;
        return gmst;
    }
}