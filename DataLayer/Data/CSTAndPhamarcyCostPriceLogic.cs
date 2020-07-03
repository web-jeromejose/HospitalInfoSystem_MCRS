using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class Select2ListModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string name { get; set; }

    }

    public class KeyValuePair
    {
        public string billno { get; set; }
        public string admissiondate { get; set; }
    }
    public class VMCSTAndPharmacyCostPrice
    {
        [Display(Name = "PIN")]
        public string PIN { get; set; }
        [Display(Name = "Admission Date")]
        public DateTime AdmissionDate { get; set; }
        public int BillNo { get; set; }
        public int ReportType { get; set; }
    }
    public class CSTAndPhamarcyCostPriceLogic
    {
        public bool Exists = false;

        DBHelper db = new DBHelper("CSTAndPhamarcyCostPrice");

        public List<Select2ListModel> Generic_S2AllPIN(string search)
        {
            db.param = new SqlParameter[] { new SqlParameter("@Search", search.HandleNull()) };
            DataTable dt = new DataTable();
            dt = db.ExecuteSPAndReturnDataTable("MCRS.Generic_S2AllPIN");
            List<Select2ListModel> lst = dt.ToList<Select2ListModel>();
            return lst.ToList();
        }

        public List<Select2ListModel> Generic_S2AdmissionDate(string search)
        {
            db.param = new SqlParameter[] { new SqlParameter("@Search", search.HandleNull()) };
            DataTable dt = new DataTable();
            dt = db.ExecuteSPAndReturnDataTable("MCRS.Generic_S2AdmissionDate");
            List<Select2ListModel> lst = dt.ToList<Select2ListModel>();
            return lst.ToList();
        }

        public DataTable RptCSTAndPharmacyCostPrice(string from, string to, string ListOfRegistrationNo)
        {
            db.param = new SqlParameter[] { 
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@ListOfRegistrationNo", ListOfRegistrationNo)
            };
            DataTable dt = new DataTable();
            dt = db.ExecuteSPAndReturnDataTable("MCRS.RptCSTAndPharmacyCostPrice");
            return dt;
        }
        public DataTable RptCSTCostPriceItemLevel(string BillNo)
        {
            db.param = new SqlParameter[] { 
                new SqlParameter("@BillNo", BillNo)
            };
            DataTable dt = new DataTable();
            dt = db.ExecuteSPAndReturnDataTable("MCRS.RptCSTCostPriceItemLevel");
            return dt;
        }
    }

   
}
