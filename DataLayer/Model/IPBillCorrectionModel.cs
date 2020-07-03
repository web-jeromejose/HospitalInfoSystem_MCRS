using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class IPBillCorrectionModel
    {
    }

    public class IPBillADModel {
        public long BillNo { get; set; }
        public string AdmissionDate { get; set; }
    }

    public class IPBillInfo {
        public long IPID { get; set; }
        public long BillNo { get; set; }
        public string PTName { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public long CompanyId { get; set; }
        public string Company { get; set; }
        public int TariffId { get; set; }
        public int FollowRules { get; set; }
        public long GradeId { get; set; }
        public string GradeName { get; set; }
        public float BillAmount { get; set; }
        public float EditBillAmount { get; set; }
        public long SlNo { get; set; }
        public string BillType { get; set; }
        public string BillDate { get; set; }
        public int DeductableType { get; set; }
        public int BedTypeId { get; set; }
        public int IsInvoiced { get; set; }

    }

    public class PosNegAdj {
        public float NegativeAdj { get; set; }
        public float PositiveAdj { get; set; }
    }

    public class BillServices
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PHItemUOM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class IPServicesItemsModel {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
    }

    public class BillItemListModel {
          public int ItemId { get; set; }
           public string Code { get; set; }
		   public string Name { get; set; }
		   public string ItemName { get; set; }
		   public long BillNo { get; set; }
		   public long SerialNo { get; set; }
           public long OrderId { get; set; }
           public long EditItemId { get; set; }
		   public int Quantity { get; set; }
		   public int EditQuantity { get; set; }
		   public string EditOrderDateTime { get; set; }
		   public float EditPrice { get; set; }
           public float Price { get; set; }
           public float EditBillAmount { get; set; }
		   public string EditFromDateTime { get; set; }
		   public string EditToDateTime { get; set; }
		   public string Datetime { get; set; }
		   public string FromDateTime { get; set; }
		   public string ToDateTime { get; set; }
           public float DeductableAmount { get; set; }
           public float Discount { get; set; }
		   public int DeductableType { get; set; }
		   public int DiscountLevel { get; set; }
		   public int DeductableLevel { get; set; }
           public float MarkUpAmount { get; set; }
           public int BedTypeId { get; set; }
    }

    public class IPItemPriceModel {
        public int Qty { get; set; }
        public int ConQty { get; set; }
        public int DepId { get; set; }
        public float Price { get; set; }
        public float MarkUp { get; set; }
        public float Discount { get; set; }
        public float Deductable { get;set;}
        public float NetAmount { get; set; }
        public float MUPer { get; set; }
        public float MUAmt { get; set; }
        public float DIPer { get; set; }
        public float DIAmt { get; set; }
        public float DEPer { get; set; }
        public float DEAmt { get; set; }
        public int DEType { get; set; }
        public int DILevel { get; set; }
        public int DELevel { get; set; }
    }

    /* SAVING PARAMETER */
    public class BillAddSaveParams {
        public List<BillAddItemList> billaddparam { get; set; }
    
    }

    public class BillAddItemList { 
       public long ItemId { get; set; }
       public int Quantity { get; set; }
       public long MarkUpAmount { get; set; }
       public long Discount { get; set; }
       public int DiscLevel { get; set; }
       public long DeductableAmount { get; set; }
       public int DedLevel { get; set; }
       public int DeptId { get; set; }
       public string BillDate { get; set; }
       public long Price { get; set; }
       public string ItemCode { get; set; }
       public string ItemName { get; set; }
    }

    /* DELETE ITEM */
    public class ARIPBillDelParams {
        public List<ARIPBillDelItemList> dellistparams { get; set; }
    }

    public class ARIPBillDelItemList {
        public long ItemId { get; set; }
        public long SLNo { get; set; }
        public int CanRe { get; set; }
    }

}
