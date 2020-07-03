using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
   public class InvoiceHeader
   {
       public int InvoiceNo { get; set; }
       public int SexId { get; set; }
       public int AgeId { get; set; }
       public int RegistrationNumber { get; set; }
       public int CityId { get; set; }
       public int StateId { get; set; }
       public int CompanyId { get; set; }
       public int CategoryId { get; set; }
       public int TariffId { get; set; }
       public int GradeId { get; set; }
       public int ElegibleType { get; set; }
       public int Block { get; set; }
       public int Billtype { get; set; }
       public int PatientType { get; set; }
       public int DeductibleType { get; set; }

       public string MedIdNumber { get; set; }
       public string Title { get; set; }
       public string Name { get; set; }
       public string FamilyName { get; set; }
       public string FirstName { get; set; }
       public string MiddleName { get; set; }
       public string LastName { get; set; }
       public string AgeType { get; set; }
       public string OtherSex { get; set; }
       public string PPhone   { get; set; }
       public string Address1 { get; set; }
       public string Address2 { get; set; }
       public string PCity     { get; set; }
       public string PCountry { get; set; }
       public string PCode { get; set; }
       public string CName { get; set; }
       public string CompanyCode { get; set; }
       public string DoctorName { get; set; }
       public string GradeName { get; set; }
       public string CompanyName { get; set; }
       public string BedName { get; set; }
       public string WardName { get; set; }
       public string CompanyCity { get; set; }
       public string CompanyAddress { get; set; }
       public string CategoryName { get; set; }
       public string CategoryCode { get; set; }
       public string CategoryAddress { get; set; }
       public string CategoryCity { get; set; }
       public string PrintAddress { get; set; }

       public DateTime AdmitDateTime { get; set; }
       public DateTime DischargeDateTime { get; set; }
       public DateTime? ExpiryDateTime { get; set; }

   }
}
