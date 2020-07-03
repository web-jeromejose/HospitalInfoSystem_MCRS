using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class GeneralInformationDB
    {
        DBHelper db = new DBHelper("GeneralInformationDB");
        public ARGeneralInformation getARGeneralInformation()
        {
            var info = new ARGeneralInformation();
            try
            {

                info = db.ExecuteSQLAndReturnDataTable("Select top 1 * From ARGENERALINFORMATION").DataTableToModel<ARGeneralInformation>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return info;
        }
    }
}
