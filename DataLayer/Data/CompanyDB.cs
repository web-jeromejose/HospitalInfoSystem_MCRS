using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Common;
using System.Data;
using System.Data.SqlClient;
namespace DataLayer.Data
{
    public class CompanyDB
    {
        DBHelper dbHelper = new DBHelper();

        public List<CompanyModel> getCompaniesWithOpBillDetail(DateTime startDate, DateTime endDate, int id)
        {
            var companies = new List<CompanyModel>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append(" SELECT DISTINCT a.CompanyId Id, b.Code, b.name Name FROM opcompanybilldetail a");
                query.Append(" LEFT JOIN Company b on a.CompanyId=b.id");
                query.Append(" WHERE a.BillDateTime >= '" + startDate+"'");
                query.Append(" AND a.BillDateTime < '" + endDate + "'");
                query.Append(" AND a.CategoryId=" + id);
                query.Append(" ORDER BY b.Code, b.name ");

                companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<CompanyModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return companies;
        }

        public List<CompanyModel> getAllCompany()
        {

            var companies = new List<CompanyModel>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append(" SELECT DISTINCT Id,  Code,Name FROM Company");
                companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<CompanyModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return companies;
        }

        public List<CompanyModel> findCompanies(string searchString)
        {

            var companies = new List<CompanyModel>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append("SELECT DISTINCT TOP 10  Id,  Code,Name FROM Company");
                query.Append(" WHERE Code Like '%" + searchString + "%'");
                query.Append(" OR Name like '%" + searchString + "%'");

                companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<CompanyModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return companies;
        }

        public DataTable getCompanyByValidityDate(DateTime validityDate, int categoryId, int active)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@validUntilDate", validityDate),
                                   new SqlParameter("@categoryId", categoryId),
                                   new SqlParameter("@active", active)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReport_GetCompanyByValidity]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getCompanyWithTransactions(DateTime startDate,DateTime endDate, int categoryId)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate),
                                   new SqlParameter("@endDate", endDate),
                                   new SqlParameter("@categoryId", categoryId)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetCompanyWithTransactions]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getCompanyBillingEfficiency(DateTime startDate, DateTime endDate, int categoryId)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate),
                                   new SqlParameter("@endDate", endDate),
                                   new SqlParameter("@categoryId", categoryId)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetBillingEfficiency]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public List<CompanyModel> getCompanyByCategory(int categoryId)
        {

            var companies = new List<CompanyModel>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append(" SELECT Id,  Code, Name FROM Company WHERE categoryid = " + categoryId + " and deleted = 0 ORDER BY name");
                companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<CompanyModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return companies;
        }

        public List<CompanyTariff> getCompanyTariffByCategory(int categoryId)
        {

            var companies = new List<CompanyTariff>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append(" SELECT a.Id,  a.Code, a.Name , b.Name TariffCode FROM Company a");
                query.Append(" INNER JOIN tariff b ON  a.tariffid = b.id");
                query.Append(" INNER JOIN category c ON  a.categoryid = c.id");
                query.Append(" WHERE c.id = " + categoryId + " ORDER BY a.name");
                companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<CompanyTariff>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return companies;
        }

        public DataTable getCompanyAuditTrail(DateTime startDate, DateTime endDate, int categoryId)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", startDate.ToString()),
                                   new SqlParameter("@endate", endDate.ToString()),
                                   new SqlParameter("@catID", categoryId.ToString())
                                };



                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[dbo].[SP_CompanyAuditTrail]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getCompanyCharges(DateTime startDate, DateTime endDate, int categoryId, int serviceId, bool hasBaseCharge, decimal baseCharge)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@serviceId", serviceId.ToString()),
                                   new SqlParameter("@categoryId", categoryId.ToString()),
                                   new SqlParameter("@hasBaseCharge",Convert.ToInt32(hasBaseCharge).ToString()),
                                   new SqlParameter("@baseCharge", baseCharge.ToString())
                                 };



                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetCompanyCharges]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getAverageClaim(DateTime startDate, DateTime endDate, int categoryId)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@CatID", categoryId.ToString())
                                 };



                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[dbo].[SP_Get_CompanyAverageClaim]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public List<CompanyModel> getCashCategory()
        {

            var companies = new List<CompanyModel>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append(" select id,code+'-'+name name from category where id=1 and deleted = 0 order by name");
                companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<CompanyModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return companies;
        }

    }
}
