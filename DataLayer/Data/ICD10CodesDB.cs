using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model;
using System.Data;

namespace DataLayer.Data
{
    public class ICD10CodesDB
    {
        DBHelper dbHelper = new DBHelper("ICD10Codes");

        public List<ICD10CodeModel> getICD10Codes(string description)
        {
            var codes = new List<ICD10CodeModel>();

            try
            {
                codes = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Code,Description from ICD10CODES WHERE Description like '%" + description + "%' order by description ").DataTableToList<ICD10CodeModel>();
            }
            catch (Exception ex) {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return codes;
        }


        public List<ICD10CodeModel> getICD10CodesTakeAndSkip(string description,int start, int end)
        {
            var codes = new List<ICD10CodeModel>();
            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.Append(";WITH TempTable AS");
            queryBuilder.Append(" ( SELECT Id, Code, Description, ROW_NUMBER() OVER (ORDER BY Description) AS RowNumber");
            queryBuilder.Append(" from ICD10CODES ");
            queryBuilder.Append(" WHERE  code like '%" + description + "%' or Description LIKE '%" + description + "%')");
            queryBuilder.Append(" SELECT * FROM TempTable ");
            queryBuilder.Append(" WHERE RowNumber >"+ start );
            queryBuilder.Append(" AND RowNumber <=" + end);

           try
            {

              codes = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<ICD10CodeModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return codes;
        }

        public List<ICD10CodeModel> getICD10Codes()
        {
            var codes = new List<ICD10CodeModel>();

            try
            {
                codes = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Code,Description from ICD10CODES  order by description ").DataTableToList<ICD10CodeModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return codes;
        }

        public ICD10CodeModel getICD10Code(int Id)
        {
            var code = new ICD10CodeModel();

            try
            {
                code = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Code,Description from ICD10CODES  WHERE Id=" + Id).DataTableToModel<ICD10CodeModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return code;
        }

        public int countICD10Code(string description)
        {
            var codes = new DataTable();
            
             try
            {
                codes = dbHelper.ExecuteSQLAndReturnDataTable("SELECT COUNT(*)as 'count' From ICD10CODES WHERE  code like '%" + description + "%' or Description like '%" + description+"%'");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

             return int.Parse(codes.Rows[0]["count"].ToString());
        }

        
    }
}
