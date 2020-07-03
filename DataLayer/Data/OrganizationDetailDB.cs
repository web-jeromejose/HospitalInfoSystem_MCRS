using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataLayer.Model;

namespace DataLayer.Data
{
    public class OrganizationDetailDB
    {

        public OrganizationDetails getOrganizationDetails()
        {
            DBHelper dbHelper = new DBHelper();
            try
            {
                var query = "SELECT TOP 1 * FROM OrganisationDetails";
                return dbHelper.ExecuteSQLAndReturnDataTable(query).DataTableToModel<OrganizationDetails>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }

    }
}
