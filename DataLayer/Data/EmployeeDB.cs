using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model;

namespace DataLayer.Data
{
    public class EmployeeDB
    {
        DBHelper dbHelper = new DBHelper("EmployeeDB");

        public EmployeeModel getEmployeeByOperatorId(int  operatorId)
        {
            var employee = new EmployeeModel();
            try
            {
                employee = dbHelper.ExecuteSQLAndReturnDataTable("SELECT ID AS OperatorId, EmployeeId, EmpCode, FirstName, MiddleName, LastName , Name AS FullName FROM Employee WHERE Id = " + operatorId).DataTableToModel<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return employee;

        }

        public List<EmployeeModel> getEmployeeByCategory(int categoryId)
        {
            var employees = new List<EmployeeModel>();
            try
            {
                employees = dbHelper.ExecuteSQLAndReturnDataTable("SELECT ID AS OperatorId, EmployeeId, EmpCode, FirstName, MiddleName, LastName , Name AS FullName FROM Employee WHERE CategoryID=" + categoryId + " AND Deleted=0 AND LEN(EMPCODE) = 4  ORDER BY EMPCODE").DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return employees;

        }

        public List<EmployeeModel> getAllDoctors()
        {
           
            var doctors = new List<EmployeeModel>(); 
           
            try
            {
                doctors = dbHelper.ExecuteSQLAndReturnDataTable("SELECT emp.ID AS OperatorId, emp.EmployeeId, doc.EmpCode, emp.FirstName, emp.MiddleName, emp.LastName , emp.Name AS FullName from Employee emp JOIN doctor doc ON emp.EmployeeID = doc.EmployeeID").DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }
       
         public List<EmployeeModel> getAllDoctosbyID()
        {
           
            var doctors = new List<EmployeeModel>(); 
           
            try
            {
                doctors = dbHelper.ExecuteSQLAndReturnDataTable("SELECT doc.ID AS OperatorId, doc.EmployeeId, doc.EmpCode, doc.empcode+ '-' + doc.FirstName + ' ' + doc.MiddleName " +
                " + ' ' +doc.LastName  AS FullName FROM Doctor doc where deleted = 0 Order by doc.empcode ").DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }
        public List<EmployeeModel> findDoctors(string searchString)
        {

            var doctors = new List<EmployeeModel>();

            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(" SELECT TOP 10 emp.ID AS OperatorId, emp.EmployeeId, doc.EmpCode, emp.FirstName, emp.MiddleName, emp.LastName , emp.Name AS FullName from  Employee Emp");
                query.Append(" JOIN doctor doc ON emp.EmployeeID = doc.EmployeeID");
                query.Append(" WHERE (doc.EmpCode LIKE '%" + searchString + "%'");
                query.Append(" OR emp.Name LIKE '%" + searchString + "%')");
                query.Append(" AND emp.Deleted = 0");
                query.Append(" ORDER BY doc.EmpCode");
                
                doctors = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }

        public List<EmployeeModel> findEmployee(string searchString)
        {

            var doctors = new List<EmployeeModel>();

            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(" SELECT TOP 10 emp.ID AS OperatorId, emp.EmployeeId, emp.EmpCode, emp.FirstName, emp.MiddleName, emp.LastName , emp.Name AS FullName from  Employee Emp");
                query.Append(" WHERE (emp.EmployeeId LIKE '%" + searchString + "%'");
                query.Append(" OR emp.Name LIKE '%" + searchString + "%')");
                query.Append(" AND emp.Deleted = 0");
                query.Append(" ORDER BY emp.Name");

                doctors = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }

        public List<EmployeeModel> findDoctorByDepartmentId(string searchString)
        {
            var doctors = new List<EmployeeModel>();
            try {
                StringBuilder query = new StringBuilder();
                if (searchString ==  "0")
                {

                    query.Append(" select * from (");
                     query.Append(" select '0' as OperatorId ,'ALL'  as FullName  union ");
                     query.Append(" Select ID as OperatorId ,(empcode+'-'+name) as FullName FROM Doctor ");
                     query.Append(" where deleted = 0 ");
                     query.Append(" ) x");
                     query.Append(" Order by x.FullName");

           
                    doctors = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<EmployeeModel>();
            
                }
                else {
                   
                    query.Append("Select ID as OperatorId ,(empcode+'-'+name) as FullName FROM Doctor  ");
                    query.Append("where deleted = 0 and  departmentid = '" + searchString + "' ");
                    query.Append("Order by empcode");
                    doctors = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<EmployeeModel>();
            
                }
               
         

              
            }
            catch (Exception ex) { throw new ApplicationException(Errors.ExemptionMessage(ex)); }
            return doctors;
        }

        public List<EmployeeModel> getAllEmpByDeptId()
        {

            var doctors = new List<EmployeeModel>();

            try
            {
                doctors = dbHelper.ExecuteSQLAndReturnDataTable("SELECT emp.ID AS OperatorId, emp.EmployeeId, emp.EmpCode, emp.FirstName, emp.MiddleName, emp.LastName , emp.EmployeeId + ' - ' +  emp.Name AS FullName FROM employee emp where emp.departmentid = 92  and deleted = 0 Order by emp.employeeid").DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }

        public List<EmployeeModel> getDesignation()
        {

            var doctors = new List<EmployeeModel>();

            try
            {
                doctors = dbHelper.ExecuteSQLAndReturnDataTable("select id as OperatorId,name as FullName from designation where deleted=0 order by name").DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }

           public List<EmployeeModel> getEmployeedetails()
        {

            var doctors = new List<EmployeeModel>();

            try
            {
                doctors = dbHelper.ExecuteSQLAndReturnDataTable("select a.employeeid, a.employeeid +'-'+ a.name as fullname from employee a right join salarydetail b on a.id=b.id where a.deleted = 0 order by a.employeeid ").DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }
 
           public List<EmployeeModel> getHREmployeeDetails()
        {

            var doctors = new List<EmployeeModel>();

            try
            {
                doctors = dbHelper.ExecuteSQLAndReturnDataTable("select distinct a.employeeid,a.name as FullName from employee a right join salarydetail b on a.id=b.id left join department c on a.departmentid = c.id  where a.deleted=0   ").DataTableToList<EmployeeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return doctors;

        }
           public List<HDREmployeeWiseModel> getEvalByEmpId(int empId)
           {

               var companies = new List<HDREmployeeWiseModel>();
               try
               {
                   StringBuilder query = new StringBuilder();

                   query.Append(" select b.employeeid ,a.id,CONVERT(varchar(11), a.frommonth, 106) +' to '+ CONVERT(varchar(11), a.tomonth, 106) + ' ('+isnull(c.name,'')+')' as Evaluation ,a.frommonth,a.tomonth from performance a  left join employee b on a.empid = b.id left join mcrs_evaluationtype c on a.evaluationtype = c.id  where b.employeeid = '" + empId + "'  ");
                   companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<HDREmployeeWiseModel>();
               }
               catch (Exception ex)
               {
                   throw new ApplicationException(Errors.ExemptionMessage(ex));
               }

               return companies;
           }

           public List<EmployeeModel> getHRdepartmentDetails()
           {

               var doctors = new List<EmployeeModel>();

               try
               {
                   doctors = dbHelper.ExecuteSQLAndReturnDataTable("select distinct a.departmentid as EmployeeId ,c.name as FullName from employee a                right join salarydetail b on a.id=b.id left join department c on a.departmentid = c.id  where a.deleted=0 order by c.name  ").DataTableToList<EmployeeModel>();
               }
               catch (Exception ex)
               {

                   throw new ApplicationException(Errors.ExemptionMessage(ex));
               }

               return doctors;

           }


           public List<EmployeeModel> getEmployebyDeptId(int deptId)
           {

               var companies = new List<EmployeeModel>();
               try
               {
                   StringBuilder query = new StringBuilder();

                   query.Append(" select a.employeeid as EmployeeId,b.name as FullName from MCRS_PEMapping a left join employee b on convert(nvarchar,a.employeeid) = b.employeeid  where  b.deleted= 0 and  a.departmentid = '" + deptId + "'  ");
                   companies = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<EmployeeModel>();
               }
               catch (Exception ex)
               {
                   throw new ApplicationException(Errors.ExemptionMessage(ex));
               }

               return companies;
           }



        

    }
}
