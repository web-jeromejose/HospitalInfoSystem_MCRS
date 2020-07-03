using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class OPBillCorrectionModel
    {

    }

    public class OPBillItemPriceModel {
        public float Price { get; set; }
        public int BatchId { get; set; }
        public int ItemDept { get; set; }
        public float MarkPercent { get; set; }
        public float MarkAmount { get; set; }
        public int DiscType { get; set; }
        public int DedType { get; set; }
        public string DedDesc { get; set; }
        public string PolicyNo { get; set; }
    }

    public class OPBillItemsModel {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
    }

    public class OPBillCorrectionDetailsModel {
        public long OpBillId { get; set; }
        public string BillNo { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public long CompanyId { get; set; }
        public string Company { get; set; }
        public long GradeId { get; set; }
        public string Grade { get; set; }
        public long DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string BillDate { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long BatchId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public float BillAmount { get; set; }
        public float PaidAmount { get; set; }
        public float Balance { get; set; }
        public float Discount { get; set; }
        public int Deductable { get; set; }
        public string DeductableName { get; set; }
        public int DepartmentId { get; set; }
        public int DiscountType { get; set; }
        public int ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public int IssueQty { get; set; }
        public int IssueUnit { get; set; }

        public string IssueUnitName { get; set; }

        public float EBillAmount { get; set; }
        public float EPaidAmount { get; set; }
        public float EDiscount { get; set; }
        public float EBalance { get; set; }
        public float EQuantity { get; set; }
        public string EBillDate { get; set; }
        public long AuthorityId { get; set; }
        public string PTName { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Agetype { get; set; }
        public string PolicyNo { get; set; }
        public string MedIDNumber { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceDateTime { get; set; }
        public string IOperatorId { get; set; }
        public string AAuthorityid { get; set; }
        public string AInvoiceId { get; set; }
        public string AInvoiceBillNo { get; set; }
        public string AInvoiceDateTime { get; set; }
        public string ARegistrationNo { get; set; }
        public string AIssueAuthorityCode { get; set; }
        public string ACategoryId { get; set; }
        public string ACompanyId { get; set; }
        public string AGradeId { get; set; }
        public string ADoctorId { get; set; }
        public string Posted { get; set; }
        public string ModifiedOperatorId { get; set; }
        public string ModifiedDateTime { get; set; }
        public string BillTypeId { get; set; }
        public string InvoiceBillNo { get; set; }
        public string ActualMedId { get; set; }
    }

    public class OPBillCorrectionParams{ 
        public long RegistrationNo { get; set; }
        public long OPBillId { get; set; }
        public string BillNo { get; set; }
        public long CategoryId { get; set; }
        public long CompanyId { get; set; }
        public long GradeId { get; set; }
        public long DoctorId { get; set; }
        public int ServiceId { get; set; }
        public long ItemId { get; set; }
        public int DiscountType { get; set; }
        public float Deductable { get; set; }
        public float BillAmount { get; set; }
        public float PaidAmount { get; set; }
        public float Discount { get; set; }
        public float Balance { get; set; }
        public string Billdatetime { get; set; }
        public int DepartmentId { get; set; }
        public int Quantity { get; set; }
        public int OperatorId { get; set; }
	    public int BillTypeId { get; set; }
        public int StationId { get; set; }
        public long AuthorityId { get; set; }
	    public int Posted { get; set; }
        public long ARegistrationNo { get; set; }
        public string AIssueAuthorityCode { get; set; }
        public long ACategoryId { get; set; }
        public long ACompanyId { get; set; }
        public long AGradeId { get; set; }
        public long ADoctorId { get; set; }
        public float ABillAmount { get; set; }
        public float APaidAmount { get; set; }
        public float ADiscount { get; set; }
        public float ABalance { get; set; }
        public int AQuantity { get; set; }
	    public long AAuthorityId { get; set; }
        public long ModifiedOperatorId { get; set; } 
        public string ModifiedDateTime { get; set; }
        public string ABillDateTime { get; set; }
        public int BatchId { get; set; } 
	    public string ItemCode { get; set; } 
        public string ItemName { get; set; }
        public string PolicyNo { get; set; }
        public string MedIdNumber { get; set; }
        public int CorType { get; set; }
        public int ReasonId { get; set; }
        public int ModifyType { get; set; }
        public int IssueQty { get; set; }
        public int IssueUnit { get; set; }
    }

    public class OPBillCorrectionSaveModel {
        public List<OPBillCorrectionParams> listofbill { get; set; }
    }

    //UOM
    public class PharItemsUOMModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ConversionQty { get; set; }
    }

    public class BillRecalculationModel {
        public float Discount { get; set; }
        public float Deductable { get; set; }
        public int DedType { get; set; }
        public string DedDesc { get; set; }
    }

}
