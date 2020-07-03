using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class ServicesDB
    {
        DBHelper dbHelper = new DBHelper("ServicesDB");

        public List<OPBService> getALLOPBServices()
        {
            var services = new List<OPBService>();
            try
            {
                services = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,ltrim(name) Name FROM opbservice WHERE Deleted=0").DataTableToList<OPBService>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return services;

        }

       

      
    }
}
