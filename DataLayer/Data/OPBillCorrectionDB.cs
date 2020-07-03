using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class OPBillCorrectionDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public int ret = 0;
        public string retmsg = "";
        public List<OPBillItemsModel> get_opbservice_items(int serid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@serid", serid)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_opb_items")
                    .DataTableToList<OPBillItemsModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<OPBillCorrectionDetailsModel> get_opb_correction_details(long pin, string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@pin", pin),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate),
                    new SqlParameter("@retcode", SqlDbType.Int),
                    new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };

                DB.param[3].Direction = ParameterDirection.Output;
                DB.param[4].Direction = ParameterDirection.Output;
                List<OPBillCorrectionDetailsModel> DD = DB.ExecuteSPAndReturnDataTable("aropbilling.opbillcor_get_pt_info").DataTableToList<OPBillCorrectionDetailsModel>();
                this.ret = int.Parse(DB.param[3].Value.ToString());
                this.retmsg = DB.param[4].Value.ToString();
                return DD;
            }
            catch (Exception ex)
            {
                this.ret = -2;
                this.retmsg = ex.Message;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public OPBillItemPriceModel get_item_price(int serid, int itemid, int catid, long comid, long graid, long docid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@serid", serid),
                    new SqlParameter("@itemid", itemid),
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@graid", graid),
                    new SqlParameter("@docid", docid)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_aropitem_price")
                    .DataTableToList<OPBillItemPriceModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public PharItemsUOMModel get_item_uom(long itemid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@itemid", itemid)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_item_units")
                    .DataTableToList<PharItemsUOMModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public BillRecalculationModel recalculate_bill(int catid, long comid, long graid, int serid, long itemid, int depid, decimal price)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@graid", graid),
                    new SqlParameter("@serid", serid),
                    new SqlParameter("@itemid", itemid),
                    new SqlParameter("@depid", depid),
                    new SqlParameter("@price", price)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.recalculate")
                    .DataTableToList<BillRecalculationModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public bool save_opbill_correction(int opeid, int staid, OPBillCorrectionSaveModel OPB)
        {
            try
            {
                //spoken language
                DataTable dtOPBILL = new DataTable();
                dtOPBILL.Columns.AddRange(new[] { 
                    new DataColumn("RegistrationNo", typeof(long)),
                    new DataColumn("OPBillId", typeof(long)),
                    new DataColumn("BillNo", typeof(string)),
                    new DataColumn("CompanyId", typeof(long)),
                    new DataColumn("CategoryId", typeof(long)),
                    new DataColumn("GradeId", typeof(long)),
	                new DataColumn("DoctorId", typeof(long)),
                    new DataColumn("ServiceId", typeof(int)),
                    new DataColumn("ItemId", typeof(long)),
                    new DataColumn("DiscountType", typeof(int)),
                    new DataColumn("Deductable", typeof(float)),
                    new DataColumn("BillAmount", typeof(float)),
	                new DataColumn("PaidAmount", typeof(float)),
                    new DataColumn("Discount", typeof(float)),
                    new DataColumn("Balance", typeof(float)),
                    new DataColumn("Billdatetime", typeof(string)),
                    new DataColumn("DepartmentId", typeof(int)),
                    new DataColumn("Quantity", typeof(int)),
                    new DataColumn("StationId", typeof(long)),
	                new DataColumn("BillTypeId", typeof(int)),
                    new DataColumn("OperatorId", typeof(long)),
                    new DataColumn("AuthorityId", typeof(long)),
	                new DataColumn("Posted", typeof(int)),
                    new DataColumn("ARegistrationNo", typeof(long)),
                    new DataColumn("AIssueAuthorityCode", typeof(string)),
                    new DataColumn("ACategoryId", typeof(long)),
                    new DataColumn("ACompanyId", typeof(long)),
	                new DataColumn("AGradeId", typeof(long)),
                    new DataColumn("ADoctorId", typeof(long)),
                    new DataColumn("ABillAmount", typeof(float)),
                    new DataColumn("APaidAmount", typeof(float)),
                    new DataColumn("ADiscount", typeof(float)),
                    new DataColumn("ABalance", typeof(float)),
                    new DataColumn("AQuantity", typeof(int)),
	                new DataColumn("AAuthorityId", typeof(long)),
                    new DataColumn("ModifiedOperatorId", typeof(long)), 
                    new DataColumn("ModifiedDateTime", typeof(string)),
                    new DataColumn("ABillDateTime", typeof(string)),
                    new DataColumn("BatchId", typeof(int)), 
	                new DataColumn("ItemCode", typeof(string)), 
                    new DataColumn("ItemName", typeof(string)),
                    new DataColumn("PolicyNo", typeof(string)),
                    new DataColumn("MedIdNumber", typeof(string)),
                    new DataColumn("CorType", typeof(int)),
                    new DataColumn("ReasonId", typeof(int)),
                    new DataColumn("ModifyType", typeof(int)),
                    new DataColumn("IssueQty", typeof(int)),
                    new DataColumn("IssueUnit", typeof(int))
                });
                System.IO.StringWriter dt = new System.IO.StringWriter();
                if (OPB.listofbill != null && OPB.listofbill.Count > 0)
                {
                    var items = OPB.listofbill.Select(i => new OPBillCorrectionParams
                    {
                        RegistrationNo = i.RegistrationNo,
                        OPBillId = i.OPBillId,
                        BillNo = i.BillNo,
                        CompanyId = i.CompanyId,
                        CategoryId = i.CategoryId,
                        GradeId = i.GradeId,
                        DoctorId = i.DoctorId,
                        ServiceId = i.ServiceId,
                        ItemId = i.ItemId,
                        DiscountType = i.DiscountType,
                        Deductable = i.Deductable,
                        BillAmount = i.BillAmount,
                        PaidAmount = i.PaidAmount,
                        Discount = i.Discount,
                        Balance = i.Balance,
                        Billdatetime = i.Billdatetime,
                        DepartmentId = i.DepartmentId,
                        Quantity = i.Quantity,
                        StationId = i.StationId > 0? i.StationId : staid,
                        BillTypeId = i.BillTypeId,
                        OperatorId = i.OperatorId > 0 ? i.OperatorId : opeid,
                        AuthorityId = i.AuthorityId,
                        Posted = i.Posted,
                        ARegistrationNo = i.ARegistrationNo,
                        AIssueAuthorityCode = i.AIssueAuthorityCode,
                        ACategoryId = i.ACategoryId,
                        ACompanyId = i.ACompanyId,
                        AGradeId = i.AGradeId,
                        ADoctorId = i.ADoctorId,
                        ABillAmount = i.ABillAmount,
                        APaidAmount = i.APaidAmount,
                        ADiscount = i.ADiscount,
                        ABalance = i.ABalance,
                        AQuantity = i.AQuantity,
                        AAuthorityId = i.AAuthorityId,
                        ModifiedOperatorId = i.ModifiedOperatorId,
                        ModifiedDateTime = i.ModifiedDateTime,
                        ABillDateTime = i.ABillDateTime,
                        BatchId = i.BatchId,
                        ItemCode = i.ItemCode,
                        ItemName = i.ItemName,
                        PolicyNo = i.PolicyNo,
                        MedIdNumber = i.MedIdNumber,
                        CorType = i.CorType,
                        ReasonId = i.ReasonId,
                        ModifyType = i.ModifyType,
                        IssueQty = i.IssueQty,
                        IssueUnit = i.IssueUnit
                    }).ToList();

                    foreach (var item in items)
                    {
                        DataRow newRow = dtOPBILL.NewRow();
                        newRow["RegistrationNo"] = item.RegistrationNo;
                        newRow["OPBillId"] = item.OPBillId;
                        newRow["BillNo"] = item.BillNo;
                        newRow["CompanyId"] = item.CompanyId;
                        newRow["CategoryId"] = item.CategoryId;
                        newRow["GradeId"] = item.GradeId;
                        newRow["DoctorId"] = item.DoctorId;
                        newRow["ServiceId"] = item.ServiceId;
                        newRow["ItemId"] = item.ItemId;
                        newRow["DiscountType"] = item.DiscountType;
                        newRow["Deductable"] = item.Deductable;
                        newRow["BillAmount"] = item.BillAmount;
                        newRow["PaidAmount"] = item.PaidAmount;
                        newRow["Discount"] = item.Discount;
                        newRow["Balance"] = item.Balance;
                        newRow["Billdatetime"] = item.Billdatetime;
                        newRow["DepartmentId"] = item.DepartmentId;
                        newRow["Quantity"] = item.Quantity;
                        newRow["BillTypeId"] = item.BillTypeId;
                        newRow["StationId"] = item.StationId;
                        newRow["OperatorId"] = item.OperatorId;
                        
                        newRow["AuthorityId"] = item.AuthorityId;
                        newRow["Posted"] = item.Posted;
                        newRow["ARegistrationNo"] = item.ARegistrationNo;
                        newRow["AIssueAuthorityCode"] = item.AIssueAuthorityCode;
                        newRow["ACategoryId"] = item.ACategoryId;
                        newRow["ACompanyId"] = item.ACompanyId;
                        newRow["AGradeId"] = item.AGradeId;
                        newRow["ADoctorId"] = item.ADoctorId;
                        newRow["ABillAmount"] = item.ABillAmount;
                        newRow["APaidAmount"] = item.APaidAmount;
                        newRow["ADiscount"] = item.ADiscount;
                        newRow["ABalance"] = item.ABalance;
                        newRow["AQuantity"] = item.AQuantity;
                        newRow["AAuthorityId"] = item.AAuthorityId;
                        newRow["ModifiedOperatorId"] = item.ModifiedOperatorId;
                        newRow["ModifiedDateTime"] = item.ModifiedDateTime;
                        newRow["ABillDateTime"] = item.ABillDateTime;
                        newRow["BatchId"] = item.BatchId;
                        newRow["ItemCode"] = item.ItemCode;
                        newRow["ItemName"] = item.ItemName;
                        newRow["PolicyNo"] = item.PolicyNo;
                        newRow["MedIdNumber"] = item.MedIdNumber;
                        newRow["CorType"] = item.CorType;
                        newRow["ReasonId"] = item.ReasonId;
                        newRow["ModifyType"] = item.ModifyType;
                        newRow["IssueQty"] = item.IssueQty;
                        newRow["IssueUnit"] = item.IssueUnit;
                        
                        dtOPBILL.Rows.Add(newRow);
                    }
                    dtOPBILL.TableName = "DTXMLDATA";
                    dtOPBILL.WriteXml(dt);
                }

                
                DB.param = new SqlParameter[]{
                   new SqlParameter("@staid", staid),
                   new SqlParameter("@opeid", opeid),
                   new SqlParameter("@XMLDATA", dt == null ? (object)DBNull.Value : dt.ToString()),
                };

                //DB.param[75].Direction = ParameterDirection.Output;
                DB.ExecuteSPAndReturnDataTable("aropbilling.save_opbill_correction");
               // this.QResult = DB.param[75].Value.ToString();
                //this.ErrorMessage = DB.param[7].Value.ToString();
                // if (!string.IsNullOrEmpty(DB.param[7].Value.ToString()))
                //  return false;
                ret = 0;
                retmsg = "Data Successfully Modified!";
                return true;

            }
            catch (Exception ex)
            {
                ret = -1;
                retmsg = ex.Message;
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    }
}
