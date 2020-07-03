using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace DataLayer.Model
{
    class PatientStatisticsModel
    {
    }

    public class DailyDashboardMonModel
    {
        public int IPCurrentlyIn { get; set; }
        public int IPCurrentlyInFTD { get; set; }
        public int IPAdmit { get; set; }
        public int IPAdmitFTD { get; set; }
        public int IPDis { get; set; }
        public int IPDisFTD { get; set; }
        public int DEForTheMonth { get; set; }
        public int DEForTheDay { get; set; }
        public int DEToday { get; set; }
        public int ERCons { get; set; }
        public int ERConsForToday { get; set; }
        public int ERAdmit { get; set; }
        public int ERAdmitForToday { get; set; }
        public int OPCurTotal { get; set; }
        public int OPCurPaid { get; set; }
        public int OPCurFree { get; set; }
        public int OPCashTotal { get; set; }
        public int OPCashPaidCount { get; set; }
        public int OPCashFreeCount { get; set; }
        public int OPChargeTotal { get; set; }
        public int OPChargePaidCount { get; set; }
        public int OPChargeFreeCount { get; set; }
        public int OPAramcoTotal { get; set; }
        public int OPAramcoPaidCount { get; set; }
        public int OPAramcoFreeCount { get; set; }
        public int ORForTheDay { get; set; }
        public int ORForTheDayFTD { get; set; }

    }

    public class ORDailyDashORA {
        public int ORRequest { get; set; }
        
    }
    public class GraphList
    {
        public List<CashList> CASH { get; set; }
        public List<CashList> CHARGE { get; set; }
        public List<CashList> ARAMCO { get; set; }
      
    }
    public class CashList
    {
        public int NO_PATIENT_CASH { get; set; }
        public string DeptName_CASH { get; set; }

    }

    public class ORDailyDashORAFTD{
        public int ORRequestFTD { get; set; }
    }

    public class ORDailyDashORAOrig{
        public int OrigORRequest { get; set; }
    }


    public class CANServiceType {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CANResultModel{
        public string ServiceType { get; set; }
        public int TCount { get; set; }
        public float TAmount { get; set; }
        public string Station { get; set; }
        public string Operator { get; set; }
        public string Reason { get; set; }

    }

    public class OPBILLActualCount {
        public int opactualcount { get; set; } 
    }

    public class OPBILLActualAmount
    {
        public float opactualamount { get; set; }
    }

    public class OPBILLCanCount
    {
        public int opcancount { get; set; }
    }

    public class OPBILLCanAmount
    {
        public float opcanamount { get; set; }
    }

    public class GenericListModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string text { get; set; }
    }

    public class AgeRange
    {
        public int id { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string startage { get; set; }
        public string endage { get; set; }
    }
}
