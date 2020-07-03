using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Enumerations
{
    public enum MCRSGroupCategory
    {
        ARDOCTORS = 30,
        ARPOWERUSER = 28
    }

    public enum ARReportStatement
    {
        BYSUMMARY           = 0,
        BYCOMPANYWITHPOLICY = 1,
        WITHPATIENTSUMMARY  = 2

    }
    public enum EmployeeCategory
    {
        DOCTOR = 1
    }

    public enum PatientBillType
    {
        ALL     = 0,
        CASH    = 1,
        CHARGE  = 2
    }

    public enum BillType
    {
         
         NOTCANCELLED = 0,
         CANCELLED = 1,
         ALL = 2
    }

    public enum OPDSelectionCount
    {
        PATIENTVISITCOUNT = 0,
        PATIENTREVISITCOUNT=1,
        NEWPATIENTCOUNT =2
    }

    public enum ReportType
    {
        DEFAULT = 0,
        BARGRAPH = 1,
        LINEGRAPH = 2
    }

    public enum DoctorSchedulePatientType
    {
        ALL = 0,
        IP  = 2,
        OP  = 3
    }

    public enum CompanyStatus
    {
        ALL     = 2,
        BLOCKED = 1,
        ACTIVE  = 0

    }

    public enum PatientStatus
    {
        ACTIVE = 1,
        NONACTIVE = 0,
        ALL =-1
    }
    public enum ReportCategoryType
    {
        ALLPATIENT = 0,
        GOVOFFICE = 1,
        DEPARTMENT = 2
    }
    public enum ReportTypeByDoctor
    {
        BYOFFICE = 0,
        BYDEPARTMENT = 1,
        BYDOCTOR = 2
    }

    public enum Bi_Site
    {
        SGH_JEDDAH = 0,
        SGH_ASEER = 1,
        SGH_RIYADH = 2,
        SGH_MADINAH = 3,
        SGH_SANAA = 4,
        SGH_DUBAI = 5,
        SGH_CAIRO = 6,
    }

    public enum SalesPromotion_ReportType
    { 
        BYDOCTOR = 0,
        BYDEPT = 1
    }
    public enum AuditReport_ChargeType
    {
        CHARGED = 0,
        BILLED = 1
    }
    public enum AuditReport_PackageType
    {
        NONPACKAGEDEAL = 0,
        PACKAGEDEAL = 1
    }
    public enum AuditReport_IpCancelledByDept
    { 
        CASH = 0,
        CHARGE = 1,
        BOTH = 2
    }

    public enum FinanceReport_PendingServicesPatientType
    {
        ALL = 0,
        INPATIENT = 1,
        OUTPATIENT = 2
    }
    public enum FinanceReport_CoveringLetterType
    {
        ALL = 0,
        BEFORE = 1,
        AFTER = 2
    }



    public enum FinanceReport_NetRevenue
    {     
        ALL = 0,
        IP = 1,
        OP = 2
    }

    public enum FinanceReport_NetRevenueBillType
    {
        ALL = 0,
        CASH = 1,
        CHARGE = 2
    }

    public enum FinanceReport_NetRevenueBillFinalize
    {
        YES = 1,NO = 0
    }
    public enum FinanceReport_NetRevenueGroupByCategory
    {
        YES = 1, NO = 0
    }
    

}