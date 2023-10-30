using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Zeptomoby.OrbitTools;

public class GroundTrackScript : MonoBehaviour {

    public LineRenderer lineRenderer;
    public Image satelliteImage;
    public Text latitude_text = null;
    public Text longitude_text = null;
    public Text utc_text = null;

    int interval = 1;
    float nextTime = 0;
    double gmst = 0;
    double utcJulian = 0;
    double utcJ2000 = 0;
    double date = 0;
    float latitude, longitude;
    const double f = 0.00335287;
    double[] gmst_array = new double[500];

    DateTime utcNow;

    private string tle_data_1;
    private string tle_data_2;

    string file_path = "/Satellite_TLE.txt";
    string DocumentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

    string TLEFromSpaceTrack;
    
    void Start()
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

        drawLatLon(tle_data_1,tle_data_2);
    }

    void Update()
    {
        //ISS Position Tracking**********************************************
        if (Time.time >= nextTime)
        {
            utcNow = DateTime.UtcNow;

            string str1 = "SGP4 Test";
            string str2 = tle_data_1;
            string str3 = tle_data_2;

            Tle tle = new Tle(str1, str2, str3);

            Satellite sat = new Satellite(tle);

            TimeSpan duration = utcNow - tle.EpochJulian.ToTime();

            Eci e = sat.PositionEci(duration.TotalMinutes);

            latitude = Mathf.Atan(Convert.ToSingle(e.Position.Z) /
                               Mathf.Sqrt(Convert.ToSingle(e.Position.X) * Convert.ToSingle(e.Position.X) +
                                          Convert.ToSingle(e.Position.Y) * Convert.ToSingle(e.Position.Y))) * 180 / Mathf.PI;

            if (Convert.ToSingle(e.Position.X) > 0 && Convert.ToSingle(e.Position.Y) > 0)
            {//++
                longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI /*- Convert.ToSingle(gmst)*/;
            }
            else if (Convert.ToSingle(e.Position.X) < 0 && Convert.ToSingle(e.Position.Y) > 0)
            {//-+
                longitude = Math.Abs(Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI + 180 /*- Convert.ToSingle(gmst)*/);
            }
            else if (Convert.ToSingle(e.Position.X) < 0 && Convert.ToSingle(e.Position.Y) < 0)
            {//--
                longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI - 180 /*- Convert.ToSingle(gmst)*/;
            }
            else
            {//+-
                longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI /*- Convert.ToSingle(gmst)*/;
            }

            longitude -= Convert.ToSingle(toGmst(utcNow.ToOADate() + 2415018.5));

            if (longitude < -180)
            {
                longitude += 360;
            }

            latitude_text.text = "Latitude = " + latitude.ToString();
            longitude_text.text = "Longitude = " + longitude.ToString();
            utc_text.text = "UTC.NOW = " + DateTime.UtcNow.ToLongDateString() + " " + DateTime.UtcNow.ToLongTimeString();

            satelliteImage.transform.position = new Vector3(Convert.ToSingle(longitude * (111) / 180), Convert.ToSingle(latitude * (62.4) / 90), 1000f);

            Debug.Log(satelliteImage.transform.position.z);

            nextTime += interval;
        }
    }

    public void drawLatLon(String tle_data_1,String tle_data2)
    {
        string str1 = "SGP4 Test";
        string str2 = tle_data_1;
        string str3 = tle_data_2;

        Tle tle = new Tle(str1, str2, str3);

        const int Step = 1;

        Satellite sat = new Satellite(tle);
        List<Eci> coords = new List<Eci>();
        Julian gmt = new Julian(tle.EpochJulian);
        date = gmt.Date;

        for (int mpe = 0; mpe <= (Step * 400); mpe += Step)
        {
            Eci eci = sat.PositionEci(mpe);
            date = date + 0.000699999; // Add 1 Minute to Julian Date
            gmst_array[mpe] = toGmst(date);
            coords.Add(eci);
        }

        lineStartPosition(coords[0], gmst_array[0]);
        int cnt = 1;
        for (int i = 1; i < (coords.Count - 1); i++)
        {
            cnt += 1;
            Eci e = coords[i] as Eci;

            cnt = calculateLatLong(coords[i],i,gmst_array[i],cnt);
        }
    }

    public double toGmst(Double utcJulian)
    {
        utcJ2000 = utcJulian - 2451545.0;
        Debug.Log("UTC_Julian = " + utcJulian);
        double UT = (utcJulian + 0.5) % 1.0;
        double TU = (utcJ2000 - UT) / 36525.0;

        double GMST = 24110.54841 + TU *
                      (8640184.812866 + TU * (0.093104 - TU * 6.2e-06));

        GMST = (GMST + Globals.SecPerDay * Globals.OmegaE * UT) % Globals.SecPerDay;

        if (GMST < 0.0)
        {
            GMST += Globals.SecPerDay;  // "wrap" negative modulo value
        }

        GMST = (Globals.TwoPi * (GMST / Globals.SecPerDay));

        GMST = (GMST * 180) / Mathf.PI;
        return GMST;
    }

    public int calculateLatLong(Eci e, int i, double gmst,int cnt)
    {
        Vector3 old_position = lineRenderer.GetPosition(cnt-1);

        latitude = Mathf.Atan(Convert.ToSingle(e.Position.Z) /
                               Mathf.Sqrt(Convert.ToSingle(e.Position.X) * Convert.ToSingle(e.Position.X) +
                                          Convert.ToSingle(e.Position.Y) * Convert.ToSingle(e.Position.Y))) * 180 / Mathf.PI;

        if (Convert.ToSingle(e.Position.X) > 0 && Convert.ToSingle(e.Position.Y) > 0)
        {//++
            longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI /*- Convert.ToSingle(gmst)*/;
        }
        else if (Convert.ToSingle(e.Position.X) < 0 && Convert.ToSingle(e.Position.Y) > 0)
        {//-+
            longitude = Math.Abs(Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI + 180 /*- Convert.ToSingle(gmst)*/);
        }
        else if (Convert.ToSingle(e.Position.X) < 0 && Convert.ToSingle(e.Position.Y) < 0)
        {//--
            longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI - 180 /*- Convert.ToSingle(gmst)*/;
        }
        else
        {//+-
            longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI /*- Convert.ToSingle(gmst)*/;
        }

        if (longitude < 180 && longitude > 0)
        {
            longitude -= Convert.ToSingle(gmst);

            if (longitude < -180)
            {
                longitude += 360;
            }
        }
        else if (longitude < 0 && longitude > -180)
        {
            longitude -= Convert.ToSingle(gmst);
            if (longitude < -180)
            {
                longitude += 360;
            }
        }

        if (old_position.x > Convert.ToSingle(longitude * (111) / 180))
        {
            lineRenderer.SetPosition(cnt, new Vector3(old_position.x ,old_position.y , 0));
            lineRenderer.SetPosition(cnt + 1, new Vector3(Convert.ToSingle(longitude * (111) / 180), Convert.ToSingle(latitude * (62.4) / 90), 0));
            lineRenderer.SetPosition(cnt + 2, new Vector3(Convert.ToSingle(longitude * (111) / 180), Convert.ToSingle(latitude * (62.4) / 90), -15));
            return cnt + 2;
        }
        else
        {
            lineRenderer.SetPosition(cnt, new Vector3(Convert.ToSingle(longitude * (111) / 180), Convert.ToSingle(latitude * (62.4) / 90), -15));
            return cnt;
        }
    }

    public void readTleData(string file_path)
    {
        char line_number;
        string line;

        try
        {
            using (StreamReader sr = new StreamReader(file_path))
            {

                while ((line = sr.ReadLine()) != null)
                {
                    line_number = line[0];

                    if (line_number == '1')
                    {
                        tle_data_1 = line;
                    }
                    else if (line_number == '2')
                    {
                        tle_data_2 = line;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }


    }

    public void lineStartPosition(Eci e, double gmst)
    {
        latitude = Mathf.Atan(Convert.ToSingle(e.Position.Z) /
                               Mathf.Sqrt(Convert.ToSingle(e.Position.X) * Convert.ToSingle(e.Position.X) +
                                          Convert.ToSingle(e.Position.Y) * Convert.ToSingle(e.Position.Y))) * 180 / Mathf.PI;

        if (Convert.ToSingle(e.Position.X) > 0 && Convert.ToSingle(e.Position.Y) > 0)
        {//++
            longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI /*- Convert.ToSingle(gmst)*/;
        }
        else if (Convert.ToSingle(e.Position.X) < 0 && Convert.ToSingle(e.Position.Y) > 0)
        {//-+
            longitude = Math.Abs(Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI + 180 /*- Convert.ToSingle(gmst)*/);
        }
        else if (Convert.ToSingle(e.Position.X) < 0 && Convert.ToSingle(e.Position.Y) < 0)
        {//--
            longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI - 180 /*- Convert.ToSingle(gmst)*/;
        }
        else
        {//+-
            longitude = Mathf.Atan(Convert.ToSingle(e.Position.Y) / Convert.ToSingle(e.Position.X)) * 180 / Mathf.PI /*- Convert.ToSingle(gmst)*/;
        }

        if (longitude < 180 && longitude > 0)
        {
            longitude -= Convert.ToSingle(gmst);

            if (longitude < -180)
            {
                longitude += 360;
            }
        }

        else if (longitude < 0 && longitude > -180)
        {
            longitude -= Convert.ToSingle(gmst);
            if (longitude < -180)
            {
                longitude += 360;
            }
        }
        lineRenderer.SetPosition(0, new Vector3(Convert.ToSingle(longitude * (111) / 180), Convert.ToSingle(latitude * (62.4) / 90), 0));
        lineRenderer.SetPosition(1, new Vector3(Convert.ToSingle(longitude * (111) / 180), Convert.ToSingle(latitude * (62.4) / 90), -15));
    }
}