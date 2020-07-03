using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class OPBServiceDB
    {
        DBHelper dbHelper = new DBHelper("OPBService");

        public List<OPBService> getServices()
        {
            var service = new List<OPBService>();
            try
            {
                service = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,UPPER(Name) Name FROM OPBService WHERE DELETED = 0 ORDER BY name").DataTableToList<OPBService>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return service;

        }

        public OPBService getServiceById(int serviceId)
        {
            var service = new OPBService();
            try
            {
                service = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,UPPER(Name) Name FROM OPBService WHERE Id =" + serviceId).DataTableToModel<OPBService>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return service;

        }
    }
}
