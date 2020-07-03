using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model;
using DataLayer.Common;

namespace DataLayer.Data
{
    public class MCRSUserDB
    {
        DBHelper dbHelper = new DBHelper("MCSUser");

        public List<MCRSUser> getUser(string employeeId)
        {
            StringBuilder queryBuilder = new StringBuilder();
            var user = new List<MCRSUser>();
            try
            {
                //queryBuilder.Append("   ");

                //remove access checking
                // dbHelper.ExecuteSQLNonQuery(queryBuilder.ToString());
                
               // user = dbHelper.ExecuteSQLAndReturnDataTable("SELECT EmployeeId, Name, GroupId FROM MCRS_Users WHERE EmployeeId= '" + employeeId +"'").DataTableToList<MCRSUser>();
            }catch(Exception ex){

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            
            return user;

        }

    }
}
