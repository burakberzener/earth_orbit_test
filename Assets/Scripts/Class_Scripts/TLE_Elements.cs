using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class TLE_Elements
    {
            //First Line

            public string Satellite_ID;
            public string Catalog_Number;
            public string Designation_Number;
            public byte Epoch_Year;
            public double Epoch_Date;
            public int Epoch_Day;
            public byte Epoch_Hour;
            public byte Epoch_Minute;
            public byte Epoch_Second;
            public double First_DoMM;
            public string Second_DoMM;
            public string BSTAR;
            public byte Ephemesis_Type;
            public double Element_Set_Number;
            public byte Firstline_Checksum;

            //Second Line

            public double Inclination;
            public double RAAN;
            public double Eccentricity;
            public double Arg_of_Perigee;
            public double Mean_Anomaly;
            public double Mean_Motion;
            public double Rev_Num_Epoch;
            public byte Secondline_Checksum;
        
    }