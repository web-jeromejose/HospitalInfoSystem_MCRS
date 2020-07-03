using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class InvoiceDB
    {
        DBHelper dbHelper = new DBHelper("InvoiceDB");


        public InvoiceHeader getInvoiceHeaderByBillNo(int billNo)
        {

            var header = new InvoiceHeader();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@billNo", billNo.ToString()),
                                   
                                 };

                header = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetInvoiceHeader]").DataTableToModel <InvoiceHeader>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return header;
        }

    }
}
