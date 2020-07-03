using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class ItemDB
    {
        DBHelper dbHelper = new DBHelper("Items");

        public List<ServiceItem> searchServiceItem(string searchString, string serviceId)
        {
            searchString = searchString.Trim();
            var serviceItems = new List<ServiceItem>();
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append(" SELECT  TOP 100 ServiceId, ItemId, ItemCode, ItemName");
                queryBuilder.Append(" FROM ARADMIN.Opmasterserviceitem ");
                queryBuilder.Append(" WHERE ServiceId = '" + serviceId + "'  AND ItemCode LIKE '%" + searchString + "%' OR ItemName LIKE '%" + searchString + "%'   ");
                serviceItems = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<ServiceItem>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return serviceItems;
        }

        public DataTable getActiveServiceItems(DateTime startDate, DateTime endDate)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@EndDate ", endDate.ToString("dd-MMM-yyyy"))
                                  
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[dbo].[SP_Active_ServiceItems]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
    }
}
