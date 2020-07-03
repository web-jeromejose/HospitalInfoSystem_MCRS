using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
   public class DepartmentDB
    {
       DBHelper dbHelper = new DBHelper();

       public List<DepartmentModel> getAllDepartment()
       {


           var departments = new List<DepartmentModel>();
           try
           {
               StringBuilder query = new StringBuilder();

               query.Append("SELECT Id,  DeptCode, Name, AccountCode FROM Department");
               departments = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<DepartmentModel>();
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }

           return departments;
       
       }

       public List<DepartmentModel> getDepartmentByCategory(int categoryId)
       {


           var departments = new List<DepartmentModel>();
           try
           {
               StringBuilder query = new StringBuilder();

               query.Append(" SELECT DISTINCT b.Id,  b.DeptCode, b.Name, b.AccountCode FROM Employee a");
               query.Append(" LEFT JOIN Department b");
               query.Append(" ON a.DepartmentID = b.id");
               query.Append(" where a.CategoryID = " + categoryId);
               query.Append(" ORDER by  b.name");
               departments = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<DepartmentModel>();
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }

           return departments;

       }
       public List<DepartmentModel> getAllDepartmentByDistinct()
       {


           var departments = new List<DepartmentModel>();
           try
           {
               StringBuilder query = new StringBuilder();


                query.Append(" select * from ( ");
                 query.Append(" select '0' as id ,'ALL'  as name  union  ");
                 query.Append(" select distinct b.id,b.name from doctor a  ");
                 query.Append(" left join department b on a.departmentid = b.id where(a.deleted = 0) and b.id not in (3,25,41,134)  ");
                 query.Append(" ) x order by x.name ");


               //query.Append("select distinct b.id,b.name from doctor a left join department b on a.departmentid = b.id where(a.deleted = 0) and b.id not in (3,25,41,134) order by b.name");
               departments = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<DepartmentModel>();
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }

           return departments;

       }

       public List<DepartmentModel> getAllHRCategory()
       {


           var departments = new List<DepartmentModel>();
           try
           {
               StringBuilder query = new StringBuilder();

               query.Append("SELECT Id, ''as  DeptCode, Name,''as AccountCode  from HRCategory");
               departments = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<DepartmentModel>();
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }

           return departments;

       }

       public List<DepartmentModel> getAllContractType()
       {


           var departments = new List<DepartmentModel>();
           try
           {
               StringBuilder query = new StringBuilder();

               query.Append("select id,name from contract where deleted = 0 order by name");
               departments = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<DepartmentModel>();
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }

           return departments;

       }


       public DepartmentModel getById(int id)
       {


           var department = new DepartmentModel();
           try
           {
               StringBuilder query = new StringBuilder();

               query.Append("select Id,  DeptCode, Name, AccountCode from department where id = " + id.ToString());
               department = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToModel<DepartmentModel>();
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }

           return department;

       }

    }
}
