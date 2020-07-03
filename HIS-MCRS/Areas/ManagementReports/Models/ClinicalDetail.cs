using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using HIS_MCRS.Common;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class ClinicalDetail :  ARClinicalDetailModel
    {
       
        public ClinicalDetail(ARClinicalDetailModel arClinicDetail) :base()
        {
            this.Description = arClinicDetail.Description;
            this.parseClinicalDescription(arClinicDetail.Description);
        }

        public string Temperature   {get; private set;} 
        public string Pulse         {get; private set;}
        public string BloodPressure {get; private set;}

        private void parseClinicalDescription(string description)
        {
            if (!String.IsNullOrEmpty(description))
            {
                int x = 0;
                int y = 0;
                string systolic = "";
                string diastolic = "";

                if (description.Trim() != "-----")
                {
                    //Temperature
                    x = description.IndexOf('-');

                    this.Temperature = description.Substring(0, x ) + " ";

                    //Respiration
                    y = x + 1;  
                    x = description.IndexOf("-", y);

                    //pulse
                    y = x + 1;
                    x = description.IndexOf("-", y);

                    this.Pulse = Helper.Val(description.Substring(y, x - y)) + " ";

                    //SPO2
                    y = x + 1;
                    x = description.IndexOf("-", y);

                    //Systolic & Diastolic
                    y = x + 1;
                    x = description.IndexOf("-", y);

                    systolic = Helper.Val(description.Substring(y, x - y)) + " ";

                    y = x + 1;

                    diastolic = Helper.Val(description.Substring(y)).ToString();

                    this.BloodPressure = (systolic + " / " + diastolic);

                }
            }

        }

    }
}