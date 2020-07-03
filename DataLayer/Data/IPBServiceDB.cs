using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class IPBServiceDB
    {
        DBHelper dbHelper = new DBHelper();

        public List<IPBService> getServices()
        {
            var services = new List<IPBService>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append(" SELECT Id, ServiceName FROM IPBService where deleted = 0");


                services = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<IPBService>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return services;
        }
    }
}
