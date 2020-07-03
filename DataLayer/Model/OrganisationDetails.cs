using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class OrganizationDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddInformation { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }
        public string EMail { get; set; }
        public string FaxNo { get; set; }
        public string PagerNo { get; set; }
        public string CellPhoneNo { get; set; }
        public string PinCode { get; set; }
        public string District { get; set; }
        public string CompanyId { get; set; }
        public string IssueAuthorityCode { get; set; }
        public string ContactPerson { get; set; }
        public string CASHDEFAULTMARKUP { get; set; }
        public string ORA_BRANCHID { get; set; }
        public string ORA_COUNTRYCODE { get; set; }
        public string SORA_CURRENCYCODE { get; set; }
        public string ORA_COMPANYCODE { get; set; }
        public DateTime movStartTime { get; set; }
        public DateTime movEndTime { get; set; }
        public int  movEnable { get; set; }
        public int IsSMSEnable { get; set; }
        public int PicPerPage { get; set; }    
        public int DelayPerPage { get; set; }
        public int new_rule_for_leave { get; set; }
        public int Tax_Enabled { get; set; }



    }
}
