using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model;

namespace DataLayer.Data
{
   public class CancelBillReasonDB
    {
       DBHelper dbHelper = new DBHelper("CancelBillReasonDB");
       
       public List<Cancelbillreason> getReasons()
       {
           var reasons = new List<Cancelbillreason>();
           try
           {
               StringBuilder queryBuilder = new StringBuilder();
               queryBuilder.Append("SELECT Id, Code, Name from  Cancelbillreason ");


               reasons = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<Cancelbillreason>();

           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }


           return reasons;
       }
    }
}
