using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class AramcoScreeningDB
    {
        DBHelper dbhelper = new DBHelper("DoctorAnalysisDB");


        public List<AramcoScreeningModel> getMCRS_AramcoScreenFile()
        {
            var sex = new List<AramcoScreeningModel>();
            try
            {
                sex = dbhelper.ExecuteSQLAndReturnDataTable("select * from MCRS_AramcoScreenFile where deleted=0 order by id").DataTableToList<AramcoScreeningModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return sex;

        }

    }
}
