using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace DataLayer.Data
{
    public class BillingOfficerCompanyMappingDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public int ret = 0;
        public string retmsg = "";
        public string operatorId;

        public List<BillOfficerCompMap>get_billing_officer_list()
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@rtype", 1),
                    new SqlParameter("@opeid", 0),
                    new SqlParameter("@catid", 0)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_compbillofficer_mapping")
                    .DataTableToList<BillOfficerCompMap>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<BillOfficerCompMap> get_billing_officer_cat_list(long opeid)
        {
            try
            {
                int catid = 0;
                DB.param = new SqlParameter[]{
                    new SqlParameter("@rtype", 2),
                    new SqlParameter("@opeid", opeid),
                    new SqlParameter("@catid", catid)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_compbillofficer_mapping")
                    .DataTableToList<BillOfficerCompMap>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<BillOfficerCompMap> get_billing_officer_com_list(long opeid)
        {
            try
            {
                int catid = 0;
                DB.param = new SqlParameter[]{
                    new SqlParameter("@rtype", 3),
                    new SqlParameter("@opeid", opeid),
                    new SqlParameter("@catid", catid)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_compbillofficer_mapping")
                    .DataTableToList<BillOfficerCompMap>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public bool save_billing_officer_mapping(int opeid, CompanyListParams CL)
        {
            try
            {
                //spoken language
                DataTable dtXMLData = new DataTable();
                dtXMLData.Columns.AddRange(new[] { 
                    new DataColumn("CategoryId", typeof(int)),
                    new DataColumn("CompanyId", typeof(long))
                });
                System.IO.StringWriter dt = new System.IO.StringWriter();
                if (CL.clist != null && CL.clist.Count > 0)
                {
                    var items = CL.clist.Select(i => new CompanyListModel
                    {
                        CategoryId = i.CategoryId,
                        CompanyId = i.CompanyId
                    }).ToList();

                    foreach (var item in items)
                    {
                        DataRow newRow = dtXMLData.NewRow();
                        newRow["CategoryId"] = item.CategoryId;
                        newRow["CompanyId"] = item.CompanyId;

                        dtXMLData.Rows.Add(newRow);
                    }
                    dtXMLData.TableName = "DTXMLDATA";
                    dtXMLData.WriteXml(dt);
                }


                DB.param = new SqlParameter[]{
                   new SqlParameter("@opeid", opeid),
                   new SqlParameter("@XMLDATA", dt == null ? (object)DBNull.Value : dt.ToString()),
                };

                //DB.param[75].Direction = ParameterDirection.Output;
                DB.ExecuteSPAndReturnDataTable("aropbilling.proc_billingofficer_mapping");
                // this.QResult = DB.param[75].Value.ToString();
                //this.ErrorMessage = DB.param[7].Value.ToString();
                // if (!string.IsNullOrEmpty(DB.param[7].Value.ToString()))
                //  return false;
                ret = 0;
                retmsg = "Billing Officer Successfully Mapped!";
                return true;

            }
            catch (Exception ex)
            {
                ret = -1;
                retmsg = ex.Message;
                eLOG.LogError(ex);
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


    }
}
