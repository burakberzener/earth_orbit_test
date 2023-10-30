using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TLE_Data_Read : MonoBehaviour {

    public int satellite_counter = 0;

    public TLE_Elements tle2ob = new TLE_Elements();

    public string readTleData(string file_path, int index_number)
    {
        char line_number;
        
        try
        {
            using (StreamReader sr = new StreamReader(file_path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    line_number = line[0];

                    if (line_number == Convert.ToChar(index_number))
                    {
                        return line;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }

        return "Error";
    }

    public void readTextFile(string file_path)
    {
        char line_number;

        try
        {
            using (StreamReader sr = new StreamReader(file_path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    line_number = line[0];

                    if (line_number == '1')
                    {
                        firstLineDecode(line);
                    }
                    else
                    {
                        secondLineDecode(line);
                    }
                    //Debug.Log(line);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    void firstLineDecode(string line)
    {
        string[] tle_first_line = line.Split(new[]{' '},StringSplitOptions.RemoveEmptyEntries);

        tle2ob.Catalog_Number = tle_first_line[1];
        tle2ob.Designation_Number = tle_first_line[2];
        tle2ob.Epoch_Year = Convert.ToByte(tle_first_line[3].Substring(0,2));
        tle2ob.Epoch_Date = Convert.ToDouble(tle_first_line[3].Substring(2, tle_first_line[3].Length-2));
        epochDateParse(tle2ob.Epoch_Date);
        tle2ob.First_DoMM = Convert.ToDouble(tle_first_line[4]);
        tle2ob.Second_DoMM = tle_first_line[5];
        tle2ob.BSTAR = tle_first_line[6];
        tle2ob.Ephemesis_Type = Convert.ToByte(tle_first_line[7]);
        tle2ob.Element_Set_Number = Convert.ToDouble(tle_first_line[8].Substring(0, tle_first_line[8].Length - 1));
        tle2ob.Firstline_Checksum = Convert.ToByte(tle_first_line[8].Substring(tle_first_line[8].Length - 1,1));

        //Debug.Log("Catalog Number :" + tle2ob.Catalog_Number);
        //Debug.Log("Designation Number :" + tle2ob.Designation_Number);
        //Debug.Log("Epoch Year :" + tle2ob.Epoch_Year);
        //Debug.Log("Epoch Date :" + tle2ob.Epoch_Date);
        //Debug.Log("Epoch Day :" + tle2ob.Epoch_Day);
        //Debug.Log("Epoch Hour :" + tle2ob.Epoch_Hour);
        //Debug.Log("Epoch Minute :" + tle2ob.Epoch_Minute);
        //Debug.Log("First Derivative of MM :" + tle2ob.First_DoMM);
        //Debug.Log("Second Derivative of MM :" + tle2ob.Second_DoMM);
        //Debug.Log("BSTAR :" + tle2ob.BSTAR);
        //Debug.Log("Ephemesis Type :" + tle2ob.Ephemesis_Type);
        //Debug.Log("Element Set Number :" + tle2ob.Element_Set_Number);
        //Debug.Log("Firstline Checksum :" + tle2ob.Firstline_Checksum);

        //foreach (string s in tle_first_line)
        //{
        //    Debug.Log(s);
        //}

        satellite_counter++;
    }

    void secondLineDecode(string line)
    {
        string[] tle_second_line = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        tle2ob.Inclination = Convert.ToDouble(tle_second_line[2]);
        tle2ob.RAAN = Convert.ToDouble(tle_second_line[3]);
        tle2ob.Eccentricity = Convert.ToDouble(tle_second_line[4]);
        tle2ob.Arg_of_Perigee = Convert.ToDouble(tle_second_line[5]);
        tle2ob.Mean_Anomaly = Convert.ToDouble(tle_second_line[6]);

        if (tle_second_line.Length == 8)
        {
            tle2ob.Mean_Motion = Convert.ToDouble(tle_second_line[7].Substring(0, 11));
            tle2ob.Rev_Num_Epoch = Convert.ToDouble(tle_second_line[7].Substring(11, 5));
            tle2ob.Secondline_Checksum = Convert.ToByte(tle_second_line[7].Substring(16, 1));
        }

        else
        {
            tle2ob.Mean_Motion = Convert.ToDouble(tle_second_line[7]);
            tle2ob.Rev_Num_Epoch = Convert.ToDouble(tle_second_line[8].Substring(0, 4));
            tle2ob.Secondline_Checksum = Convert.ToByte(tle_second_line[8].Substring(4, 1));
        }

        //Debug.Log("Inclination :" + tle2ob.Inclination);
        //Debug.Log("RAAN :" + tle2ob.RAAN);
        //Debug.Log("Eccentricity :" + tle2ob.Eccentricity);
        //Debug.Log("Arg. of Perigee :" + tle2ob.Arg_of_Perigee);
        //Debug.Log("Mean Anomaly :" + tle2ob.Mean_Anomaly);
        //Debug.Log("Mean Motion :" + tle2ob.Mean_Motion);
        //Debug.Log("Rev. Num. Epoch :" + tle2ob.Rev_Num_Epoch);
        //Debug.Log("Checksum :" + tle2ob.Secondline_Checksum);

        //foreach (string s in tle_second_line)
        //{
        //    Debug.Log(s);
        //}
    }

    void epochDateParse(double Epoch_Date)
    {
        double Frac_Day;
        double Dec_Hour;

        tle2ob.Epoch_Day = Convert.ToInt16(Math.Floor(Epoch_Date));

        Frac_Day = Epoch_Date - tle2ob.Epoch_Day;
        Dec_Hour = Frac_Day * 24;

        tle2ob.Epoch_Hour = Convert.ToByte(Math.Floor(Dec_Hour));
        tle2ob.Epoch_Minute = Convert.ToByte(Math.Floor((Dec_Hour-tle2ob.Epoch_Hour)*60));
    }


}