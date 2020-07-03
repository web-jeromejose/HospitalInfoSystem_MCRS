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
    public class IPBillCorrectionDB
    {
        DBHelper DB = new DBHelper("ARIPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public int ret = 0;
        public string retmsg = "";
        public List<IPBillADModel> get_IPBill_Admission_List(long pin)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@pin", pin),
                    new SqlParameter("@retcode", SqlDbType.Int),
                    new SqlParameter("@retmsgs", SqlDbType.VarChar, 500)
                };
                DB.param[1].Direction = ParameterDirection.Output;
                DB.param[2].Direction = ParameterDirection.Output;
                List<IPBillADModel> DD = DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_admitdate")
                    .DataTableToList<IPBillADModel>();
                this.ret = int.Parse(DB.param[1].Value.ToString());
                this.retmsg = DB.param[2].Value.ToString();
                return DD;
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public IPBillInfo get_IPBill_PT_Information(long billno)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@billno", billno),
                    new SqlParameter("@retcode", SqlDbType.Int),
                    new SqlParameter("@retmsgs", SqlDbType.VarChar, 500)
                };
                DB.param[1].Direction = ParameterDirection.Output;
                DB.param[2].Direction = ParameterDirection.Output;
               IPBillInfo DD =  DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_info")
                    .DataTableToList<IPBillInfo>().FirstOrDefault();
               this.ret = int.Parse(DB.param[1].Value.ToString());
               this.retmsg = DB.param[2].Value.ToString();
               return DD;
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public PosNegAdj get_IPBill_PosNeg_Adj(long billno)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@billno", billno)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_posneg_adj")
                    .DataTableToList<PosNegAdj>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<BillServices> get_IPBill_Services(long billno)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@billno", billno)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_services")
                    .DataTableToList<BillServices>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<BillServices> get_IPBill_Services_All(int catid, long comid, long graid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@graid", graid)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_ipbservice")
                    .DataTableToList<BillServices>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


        public List<BillItemListModel> get_IPBill_Items(long billno, int serid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@billno", billno),
                    new SqlParameter("@serid", serid),
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_items")
                    .DataTableToList<BillItemListModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<IPServicesItemsModel> get_IPBill_ServiceItems(int serid, long comid, long graid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@serid", serid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@graid", graid),
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_service_items")
                    .DataTableToList<IPServicesItemsModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);

            }
        }

        public IPItemPriceModel get_IPBill_ServiceItems_Price(int serid, long itemid, int bedid, int dedtype, int packid, int tariffid, int catid, long comid, long graid)
        {
            try
            {
                if ((serid == 5 || serid == 37) && packid > 0)
                {
                    DB.param = new SqlParameter[]{
                        new SqlParameter("@catid",catid),
	                    new SqlParameter("@comid ", comid),
	                    new SqlParameter("@graid", graid),
	                    new SqlParameter("@serid", serid),
	                    new SqlParameter("@itemid", itemid),
	                    new SqlParameter("@packid", packid),
	                    new SqlParameter("@dedtype", dedtype)

                    };
                    return DB
                        .ExecuteSPAndReturnDataTable("aripbilling.get_ipbill_pharitem_price")
                        .DataTableToList<IPItemPriceModel>().FirstOrDefault();

                }
                else {
                    DB.param = new SqlParameter[]{
                        new SqlParameter("@catid",catid),
	                    new SqlParameter("@comid ", comid),
	                    new SqlParameter("@graid", graid),
	                    new SqlParameter("@serid", serid),
	                    new SqlParameter("@itemid", itemid),
	                    new SqlParameter("@bedid", bedid),
	                    new SqlParameter("@tariffid", tariffid),
	                    new SqlParameter("@dedtype", dedtype)

                    };
                    return DB
                        .ExecuteSPAndReturnDataTable("aripbilling.get_IPBill_ItemPrice")
                        .DataTableToList<IPItemPriceModel>().FirstOrDefault();
                }
                
                
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);

            }
        }

        public List<PHItemUOM> get_PHItem_UOM(long itemid)
        {
            try
            {
                DB.param = new SqlParameter[]{
	                new SqlParameter("@itemid", itemid)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_item_units")
                    .DataTableToList<PHItemUOM>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);

            }
        }

        /* Saving IP Bill Added Items */

        public bool save_ipbill_corrections(int opeid, int serid, long billno, BillAddSaveParams IPB)
        {
            try
            {
                //spoken language
                DataTable dtIPBILL = new DataTable();
                dtIPBILL.Columns.AddRange(new[] { 
                    new DataColumn("ItemId", typeof(long)),
                    new DataColumn("Quantity", typeof(int)),
                    new DataColumn("MarkUpAmount", typeof(long)),
                    new DataColumn("Discount", typeof(long)),
                    new DataColumn("DiscLevel", typeof(int)),
                    new DataColumn("DeductableAmount", typeof(long)),
                    new DataColumn("DedLevel", typeof(int)),
                    new DataColumn("DeptId", typeof(int)),
                    new DataColumn("BillDate", typeof(string)),
                    new DataColumn("Price", typeof(long)),
                    new DataColumn("ItemCode", typeof(string)),
                    new DataColumn("ItemName", typeof(string))
                });
                System.IO.StringWriter dt = new System.IO.StringWriter();
                if (IPB.billaddparam != null && IPB.billaddparam.Count > 0)
                {
                    var items = IPB.billaddparam.Select(i => new BillAddItemList
                    {
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                        MarkUpAmount = i.MarkUpAmount,
                        Discount = i.Discount,
                        DiscLevel = i.DiscLevel,
                        DeductableAmount = i.DeductableAmount,
                        DedLevel = i.DedLevel,
                        DeptId = i.DeptId,
                        BillDate = i.BillDate,
                        Price = i.Price,
                        ItemCode = i.ItemCode,
                        ItemName = i.ItemName
                    }).ToList();

                    foreach (var item in items)
                    {
                        DataRow newRow = dtIPBILL.NewRow();
                        newRow["ItemId"] = item.ItemId;
                        newRow["Quantity"] = item.Quantity;
                        newRow["MarkUpAmount"] = item.MarkUpAmount;
                        newRow["Discount"] = item.Discount;
                        newRow["DiscLevel"] = item.DiscLevel;
                        newRow["DeductableAmount"] = item.DeductableAmount;
                        newRow["DedLevel"] = item.DedLevel;
                        newRow["DeptId"] = item.DeptId;
                        newRow["BillDate"] = item.BillDate;
                        newRow["Price"] = item.Price;
                        newRow["ItemCode"] = item.ItemCode;
                        newRow["ItemName"] = item.ItemName;
                        dtIPBILL.Rows.Add(newRow);
                    }
                    dtIPBILL.TableName = "DTXMLDATA";
                    dtIPBILL.WriteXml(dt);
                }


                DB.param = new SqlParameter[]{
                   new SqlParameter("@serid", serid),
                   new SqlParameter("@billno", billno),
                   new SqlParameter("@opeid", opeid),
                   new SqlParameter("@XMLDATA", dt == null ? (object)DBNull.Value : dt.ToString()),
                };

                //DB.param[75].Direction = ParameterDirection.Output;
                DB.ExecuteSPAndReturnDataTable("ARIPBILLING.save_ipbill_additem");
                // this.QResult = DB.param[75].Value.ToString();
                //this.ErrorMessage = DB.param[7].Value.ToString();
                // if (!string.IsNullOrEmpty(DB.param[7].Value.ToString()))
                //  return false;
                ret = 0;
                retmsg = "Data Successfully Saved!";
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

        /* Update IP Bill Item */

        public bool update_ipbill_corrections(int opeid, int serid, long billno, 
            long slno, float disc, float ded, int eqty, float eprice, string code, string name,
            long itemid, string dtime)
        {
            try
            {
               
                DB.param = new SqlParameter[]{
                    new SqlParameter("@serid",serid),
	                new SqlParameter("@slno",slno),
	                new SqlParameter("@disc",disc),
	                new SqlParameter("@ded",ded),
	                new SqlParameter("@eqty",eqty),
	                new SqlParameter("@eprice",eprice),
	                new SqlParameter("@code",code),
	                new SqlParameter("@name",name),
	                new SqlParameter("@itemid",itemid),
	                new SqlParameter("@dtime",dtime),
	                new SqlParameter("@billno",billno),
	                new SqlParameter("@opeid",opeid)
                };

                DB.ExecuteSPAndReturnDataTable("ARIPBILLING.update_ipbill_item");

                ret = 0;
                retmsg = "Data Successfully Updated!";
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

        /* Delete IP Bill Item */

        public bool delete_ipbill_corrections(int opeid, int serid, long billno, ARIPBillDelParams IPB)
        {
            try
            {
                //spoken language
                DataTable dtIPBILL = new DataTable();
                dtIPBILL.Columns.AddRange(new[] { 
                    new DataColumn("ItemId", typeof(long)),
                    new DataColumn("SLNo", typeof(long)),
                    new DataColumn("CanRe", typeof(int))

                });
                System.IO.StringWriter dt = new System.IO.StringWriter();
                if (IPB.dellistparams != null && IPB.dellistparams.Count > 0)
                {
                    var items = IPB.dellistparams.Select(i => new ARIPBillDelItemList
                    {
                        ItemId = i.ItemId,
                        SLNo = i.SLNo,
                        CanRe = i.CanRe
                    }).ToList();

                    foreach (var item in items)
                    {
                        DataRow newRow = dtIPBILL.NewRow();
                        newRow["ItemId"] = item.ItemId;
                        newRow["SLNo"] = item.SLNo;
                        newRow["CanRe"] = item.CanRe;
                        dtIPBILL.Rows.Add(newRow);
                    }
                    dtIPBILL.TableName = "DTXMLDATA";
                    dtIPBILL.WriteXml(dt);
                }


                DB.param = new SqlParameter[]{
                   new SqlParameter("@serid", serid),
                   new SqlParameter("@billno", billno),
                   new SqlParameter("@opeid", opeid),
                   new SqlParameter("@XMLDATA", dt == null ? (object)DBNull.Value : dt.ToString()),
                };

                //DB.param[75].Direction = ParameterDirection.Output;
                DB.ExecuteSPAndReturnDataTable("ARIPBILLING.delete_ipbill_item");
                // this.QResult = DB.param[75].Value.ToString();
                //this.ErrorMessage = DB.param[7].Value.ToString();
                // if (!string.IsNullOrEmpty(DB.param[7].Value.ToString()))
                //  return false;
                ret = 0;
                retmsg = "Item Successfully Deleted!";
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
