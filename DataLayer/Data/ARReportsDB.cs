using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model;
using System.Data;
using System.Data.SqlClient;
namespace DataLayer.Data
{
    public class ARReportsDB
    {
        DBHelper dbHelper = new DBHelper("AR");

        #region[UCAF Single View]
       
        public List<ARDiagnosisModel> getARDiagnosis(long visitId)
        {
            try
            {
                var query = "SELECT VisitId, ICDId, ICDCode, ICDDescription ,TrnDateTime AS TransactionDate, OperatorId FROM ARConsultationICDDetail WHERE visitid = " + visitId;
                return dbHelper.ExecuteSQLAndReturnDataTable(query).DataTableToList<ARDiagnosisModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        
        public List<ARDiagnosisModel> getICDDiagnosis(long visitId)
        {
            DBHelper dbHelper = new DBHelper();
            try
            {
                var query = "SELECT VisitId, ICDId,ICDCode,ICDDescription, Isnull(DateTime,0) AS TransactionDate, OperatorId FROM Icddetail WHERE visitid = " + visitId;
                return dbHelper.ExecuteSQLAndReturnDataTable(query).DataTableToList<ARDiagnosisModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }

        public ARTreatmentPlanModel   getARTreatmentPlan(long visitId)
        {
            try
            {
                var query = "SELECT TreatmentPlan  FROM ARConsultationVisitDetails WHERE visitid=" + visitId;
                return dbHelper.ExecuteSQLAndReturnDataTable(query).DataTableToModel<ARTreatmentPlanModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
        }

        public List<ARClinicExamModel>  getClinicExamination(long visitId)
        {
            try
            {
                var query = "SELECT ICId,ICDCode,ICDDescription FROM ARConsultationICDDetail WHERE visitid = " + visitId;
                return dbHelper.ExecuteSQLAndReturnDataTable(query).DataTableToList<ARClinicExamModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
        }

        public List<ARVisitDateModel> getVisitDates(int regNumber)
        {

            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(" SELECT d.id Id, a.Datetime, CASE WHEN c.type=1 THEN '(New Visit)' ELSE '(Revisit)' END VisitType,e.empcode + ' - ' + e.name DoctorName ");
                query.Append(" FROM opbill a RIGHT JOIN opdoctororder b ON a.id=b.opbillid LEFT JOIN opdoctororderdetail c ON b.id=c.opdoctororderid INNER JOIN clinicalvisit d ON c.opdoctororderid = d.scheduleid LEFT JOIN employee e ON c.doctorid = e.id");
                query.Append(" WHERE a.registrationno = " + regNumber);
                query.Append(" ORDER BY a.datetime DESC");
                return dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<ARVisitDateModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
        }

        public DataTable getClinicalTestOrdetUCAF(int visitId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@visitID", visitId)
                                };

                 dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_SP_Get_ClinicalTestOrdetUCAF]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
           public DataTable getClinicalTestOrdetUCAF_version2(int visitId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@visitID", visitId)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_SP_Get_ClinicalTestOrdetUCAF_v2]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

       

        public HDRUCAFModel getHDRUCAF(int visitId)
        {

            var hdrUCAFList = new HDRUCAFModel();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@vid", visitId)
                                };

                hdrUCAFList = dbHelper.ExecuteSPAndReturnDataTable("SP_GetHDR_UCAF").DataTableToModel<HDRUCAFModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return hdrUCAFList;
        }


        public ARConsultationVisitDetailModel getARConsultationVisitDetail(int visitId)
        {
            var consultationDetail = new ARConsultationVisitDetailModel();

            try{
                StringBuilder query = new StringBuilder();
                query.Append("SELECT VisitId, ISNULL(ChiefComplaints,'') ChiefComplaints, ISNULL(TreatmentPlan,'Nothing') TreatmentPlan");
                query.Append(" FROM ARConsultationVisitDetails");
                query.Append(" WHERE VisitId = " + visitId);


                consultationDetail = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToModel<ARConsultationVisitDetailModel>();

            }
            catch(Exception ex){
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return consultationDetail;
        }

        public ARConsultationVisitDetailModel getClinicVisitDetails(int visitId)
        {
            var consultationDetail = new ARConsultationVisitDetailModel();

            try
            {
                StringBuilder query = new StringBuilder();
                //query.Append(" SELECT cd.VisitId, ISNULL(cd.Description,'') ChiefComplaints, ISNULL(cv.TreatmentPlan,'Nothing') TreatmentPlan ");
                //query.Append(" FROM Clinicaldetails cd");
                //query.Append(" JOIN ClinicalVisit cv on cd.VisitId = cv.ID ");
                //query.Append(" WHERE cd.VisitId = " + visitId);
                ////query.Append(" AND cd.TableId=1 AND cd.MainSymptomId=0 ");
                //query.Append(" AND cd.TableId=1  ");

                query.Append("  declare @rn int  ");
                query.Append("  declare @loop int = 1 ");
                query.Append("  declare @VisitId int = " + visitId);
                query.Append("  declare @ChiefComplaintsLoop nvarchar(max)  ");
                query.Append("  declare @ChiefComplaints nvarchar(max)  ");
                query.Append("  declare @TreatmentPlanLoop nvarchar(max)  ");
                query.Append("  declare @TreatmentPlan nvarchar(max);  ");
                query.Append("   ");
                query.Append("  with Dups As ");
                query.Append("  ( ");
                query.Append("  SELECT cd.VisitId, ISNULL(cd.Description,'') ChiefComplaints ");
                query.Append("  , ISNULL(cv.TreatmentPlan,'Nothing') TreatmentPlan   ");
                query.Append("  ,ROW_NUMBER() OVER(PARTITION By cd.VisitId  order by (select 0)) as rn ");
                query.Append("  FROM Clinicaldetails cd JOIN ClinicalVisit cv on cd.VisitId = cv.ID  ");
                query.Append("  WHERE cd.VisitId = @VisitId AND cd.TableId=1  ");
                query.Append("  ) ");
                query.Append("  select * into #tableArChiefComplaint from dups; ");
                query.Append("     ");
                query.Append("  Declare @MainTBArChiefComplaint as TABLE (VisitId INT IDENTITY, ChiefComplaints  nvarchar(max) ,TreatmentPlan nvarchar(max)) ");
                query.Append("  set  @ChiefComplaints = ' ' ");
                query.Append("  set  @TreatmentPlan = ' ' ");
                query.Append("  WHILE EXISTS( select *  from #tableArChiefComplaint)   ");
                query.Append("  BEGIN ");
                query.Append("  select @rn = rn from #tableArChiefComplaint order by rn desc ");
                query.Append("  select @ChiefComplaintsLoop = ChiefComplaints,@TreatmentPlanLoop = TreatmentPlan   from #tableArChiefComplaint where rn = @rn	 ");
                query.Append("   SET @ChiefComplaints = ' '+@ChiefComplaints+'('+cast(@loop as varchar(max))+') '+ @ChiefComplaintsLoop + ' '     ");
                query.Append("  SET @TreatmentPlan = '  '+@TreatmentPlan + @TreatmentPlanLoop + ' ' ");
                query.Append("  SET @loop = @loop + 1  ");
                query.Append("  delete from #tableArChiefComplaint where rn = @rn ");
                query.Append("  END  ");
                query.Append("   ");
                query.Append("  select @VisitId as VisitId ,@ChiefComplaints as ChiefComplaints ,IsNULL(NULLIF(@TreatmentPlan,''),'Nothing') as TreatmentPlan ");
                query.Append("   ");
                query.Append("  drop table #tableArChiefComplaint ");



                consultationDetail = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToModel<ARConsultationVisitDetailModel>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return consultationDetail;
        }

        public List<ARExaminationModel> getARExamination(int visitId)
        {
            var arexam = new List<ARExaminationModel>();
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT VisitId, b.name AS MainSymptom, c.name AS SubSymptom, a.Description");
                query.Append(" FROM Clinicaldetails a");
                query.Append(" LEFT JOIN Examination b ON a.mainsymptomid = b.id");
                query.Append(" LEFT JOIN Examination c on a.subsymptomid = c.id ");
                query.Append(" WHERE VisitId = " + visitId);
                query.Append(" AND tableid = 4 And mainsymptomid <> 0");
                query.Append(" ORDER by a.mainsymptomid ");

                arexam = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<ARExaminationModel>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return arexam;
        }

        public ARClinicalDetailModel getClinicalDetail(int visitId)
        {
             var clinicalDetail = new ARClinicalDetailModel();
             try
             {
                 StringBuilder queryBuilder = new StringBuilder();
                 queryBuilder.Append("SELECT Description from Clinicaldetails");
                 queryBuilder.Append(" Where visitid = " + visitId);
                 queryBuilder.Append(" And tableid=5 ");

                 clinicalDetail = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToModel<ARClinicalDetailModel>();

             }
             catch (Exception ex)
             {
                 throw new ApplicationException(Errors.ExemptionMessage(ex));
             }


            return clinicalDetail;
        }

        public List<ARIllnessTypeModel> getIllnessType(int visitId)
        {

            var illnessType = new List<ARIllnessTypeModel>();
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                //queryBuilder.Append("SELECT b.Name,CASE WHEN a.Condition=1 THEN 'YES' ELSE 'NO' END Condition  ");
                //queryBuilder.Append("FROM ARClinicaldetails a,ARClinicalcondition b ");
                //queryBuilder.Append("WHERE a.Arclinicalid=*b.id AND  a.visitid = " + visitId  + " and b.deleted=0");


                queryBuilder.Append("SELECT b.Name,CASE WHEN a.Condition=1 THEN 'YES' ELSE 'NO' END Condition  ");
                queryBuilder.Append("FROM ARClinicaldetails a left join ARClinicalcondition b on a.Arclinicalid = b.id ");
                queryBuilder.Append("WHERE a.visitid = " + visitId + " and b.deleted=0");


                illnessType = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<ARIllnessTypeModel>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }


            return illnessType;
        }



        public ARClinicalVisit getClinicalVisit(int visitId)
        {

           var clinicalVisit = new ARClinicalVisit();
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append(" SELECT a.ID,   c.deptcode  DepartmentCode FROM clinicalvisit a");
                queryBuilder.Append(" LEFT JOIN employee b ON a.DoctorID = b.ID");
                queryBuilder.Append(" LEFT JOIN Department c ON b.DepartmentID = c.id ");
                queryBuilder.Append(" WHERE a.ID= "  + visitId  );

                clinicalVisit = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToModel<ARClinicalVisit>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }


            return clinicalVisit;
           
        }

        public bool insertOrUpdateARConsultationVisitDetails(int visitId, int operatorId , string treatmentPlan, string chiefComplaints)
        {
            bool success = false;
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append(" IF NOT EXISTS(SELECT * FROM ARConsultationVisitDetails WHERE visitid = " + visitId + ") ");
                queryBuilder.Append("   BEGIN ");
                queryBuilder.Append("       INSERT INTO ARConsultationVisitDetails(Visitid,ChiefComplaints,TreatmentPlan,OperatorID,trnDateTime)  ");
                queryBuilder.Append("       VALUES(" + visitId + ",'" + chiefComplaints+ "','" +treatmentPlan + "'," + operatorId +", getdate()) ");
                queryBuilder.Append("   END");
                queryBuilder.Append(" ELSE ");
                queryBuilder.Append("   BEGIN ");
                queryBuilder.Append("        UPDATE ARConsultationVisitDetails SET ChiefComplaints = '" + chiefComplaints + "', ");
                queryBuilder.Append("        TreatmentPlan = '"+ treatmentPlan +"', ");
                queryBuilder.Append("        Operatorid = " + operatorId +", ");
                queryBuilder.Append("        trnDateTime = getdate()");
                queryBuilder.Append("        WHERE visitid = "+ visitId);
                queryBuilder.Append("   END");
                //create log Jan 18 -2018
                queryBuilder.Append(" if not exists(select * from sys.tables where name = 'AR_ChiefComplaints') ");
                queryBuilder.Append(" BEGIN ");
                queryBuilder.Append(" CREATE TABLE [MCRS].[AR_ChiefComplaints]( ");
                queryBuilder.Append(" [Id] [int] IDENTITY(1,1) NOT NULL, ");
                queryBuilder.Append(" [EmployeeId] [int] NULL, ");
                queryBuilder.Append(" [ChiefComplaints] [ntext] NULL, ");
                queryBuilder.Append(" [TreatmentPlan] [ntext] NULL, ");
                queryBuilder.Append(" [visitid] [int] NULL, ");
                queryBuilder.Append(" [DateCreated] [datetime] NULL ");
                queryBuilder.Append(" ) ON [MasterFile] TEXTIMAGE_ON [MasterFile] ");
                queryBuilder.Append(" END ");
                queryBuilder.Append("  ");
                queryBuilder.Append(" insert into  [MCRS].[AR_ChiefComplaints]  ");
                queryBuilder.Append(" (EmployeeId,ChiefComplaints,TreatmentPlan,visitid,DateCreated)  ");
                queryBuilder.Append(" values('" + operatorId + "','" + chiefComplaints + "','" + treatmentPlan + "','" + visitId + "',GetDate())  ");


                success = dbHelper.ExecuteSQLNonQuery(queryBuilder.ToString());

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return success;
        }


        public bool updateARConsultationICDDetail(int icdId, int visitId, string icdCode, string icdDescription , int operatorId)
        {
            bool success = false;
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append(" INSERT INTO ARConsultationICDDetail");
                queryBuilder.Append(" (VisitId, IcdId, IcdCode, IcdDescription, TrnDateTime, OperatorId)");
                queryBuilder.Append("  VALUES");
                queryBuilder.Append(" (" +visitId+","+ icdId +",'" + icdCode+"','"+ icdDescription +"', GETDATE(),"+ operatorId+")");
               
                success = dbHelper.ExecuteSQLNonQuery(queryBuilder.ToString());

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return success;

        }

        public bool deleteARDiagnosis(long visitId)
        {
            bool success = false;
            try
            {
                success = dbHelper.ExecuteSQLNonQuery("DELETE FROM ARConsultationICDDetail WHERE visitid = '" + visitId+"'");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return success;

        }

        #endregion

        #region[Statement Summary by Category]
        public DataTable getStatementByCategory(DateTime fromDate, DateTime toDate, int categoryId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", fromDate),
                                   new SqlParameter("@enDate", toDate),
                                   new SqlParameter("@xcatID", categoryId)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("GetStatementSummary_ByCategory");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }
        public DataTable getStatementByCompany(DateTime fromDate, DateTime toDate, int categoryId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", fromDate),
                                   new SqlParameter("@enDate", toDate),
                                   new SqlParameter("@xcatID", categoryId)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("GetStatementSummary_ByCompany");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable getStatementByCompanyCategory(DateTime fromDate, DateTime toDate, int categoryId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", fromDate),
                                   new SqlParameter("@enDate", toDate),
                                   new SqlParameter("@xcatID", categoryId)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("GetStatementSummary_ByCompanyCategory");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }
        #endregion[Statement Summary by Category]

        #region[Statement Summary by All Category]
        public DataTable getStatementAllCategory(DateTime fromDate, DateTime toDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", fromDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", toDate.ToString("dd-MMM-yyyy"))
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("GetStatementSummary_ByCategory_All");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }
        #endregion[Statement Summary by All Category]

        #region[UCAF Record For Batch Printing]
        public List<ARUCAFBatchPrintRecord> getUCAFRecords(int categorId, int doctorId, DateTime fromDate, DateTime toDate, List<CompanyModel> selectedCompanies)
        {
            var records = new List<ARUCAFBatchPrintRecord>();   
            
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append(" DECLARE @issueAuthorityCode  VARCHAR(50)");
                queryBuilder.Append(" SET @issueAuthorityCode= (SELECT  IssueAuthorityCode FROM OrganisationDetails);");
                queryBuilder.Append(" SELECT isnull(d.id,0) VisitId,");
                queryBuilder.Append(" @issueAuthorityCode +'.' + (REPLICATE('0', (10-LEN(a.RegistrationNo))) + CONVERT(VARCHAR,a.registrationno)) PIN,");
                queryBuilder.Append(" f.FamilyName + ' ' + f.FirstName + ' ' + f.middlename + ' ' + f.LastName AS PatientName,"); 
                queryBuilder.Append(" a.datetime, CASE WHEN c.type=1 THEN '(New Visit)' ELSE '(Revisit)' END VisitType,");
                queryBuilder.Append(" e.empcode + ' - ' + e.name Doctorname,");
                queryBuilder.Append(" g.code CompanyCode");
                queryBuilder.Append(" FROM opbill a"); 
                queryBuilder.Append(" RIGHT JOIN opdoctororder b ON a.id=b.opbillid");
                queryBuilder.Append(" LEFT  JOIN opdoctororderdetail c ON b.id=c.opdoctororderid");
                queryBuilder.Append(" LEFT  JOIN clinicalvisit d ON c.opdoctororderid = d.scheduleid");
                queryBuilder.Append(" LEFT  JOIN employee e ON c.doctorid = e.id");

                queryBuilder.Append(" LEFT  JOIN patient f ON a.RegistrationNo = f.registrationno");
                queryBuilder.Append(" LEFT  JOIN company g ON a.companyid = g.id");
                queryBuilder.Append(" WHERE a.CategoryId = "+ categorId);
                queryBuilder.Append(" AND a.DateTime >='"+ fromDate+"'");
                queryBuilder.Append(" AND a.DateTime < '" + toDate.AddDays(1)+"'");

                if (selectedCompanies.Count() > 0)
                {
                    var companyIdsStr = "";
                    foreach(var company in selectedCompanies){
                        companyIdsStr += company.Id;
                        if (selectedCompanies.IndexOf(company) < selectedCompanies.Count() - 1)
                        {
                            companyIdsStr += ",";
                        }
                    }

                
                    queryBuilder.Append(" AND a.companyid IN (" + companyIdsStr + ") ");
                }
                if (doctorId > 0)
                {
                    queryBuilder.Append(" AND d.doctorid=" + doctorId);
                }

                queryBuilder.Append(" ORDER BY g.code, a.RegistrationNo");

                records = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<ARUCAFBatchPrintRecord>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return records;

        }
        #endregion[UCAF Record ForBatch Printing]

        public DataTable getARCompanyTotalVisit(DateTime startDate, DateTime endDate, int categoryId, int creationType)
        {

            var dataTable = new DataTable();

            var spName = creationType == 0 ? "[MCRS].[ARReports_getCompanyPatientOPCreated]" : "[MCRS].[ARReports_getCompanyPatientVisitARBilled]";
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@categoryId", categoryId.ToString()),
                                   
                                };



                dataTable = dbHelper.ExecuteSPAndReturnDataTable(spName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getSummaryOfAccounts(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, int subCategory)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),
                                   new SqlParameter("@SubCategory", subCategory.ToString())
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummary]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }
        public DataTable getSummaryOfAccountsORWithSubCategoryName(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName,int aftercoverletter)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),
                                 //  new SqlParameter("@HasCompanyLetter", aftercoverletter),
                                   new SqlParameter("@SubCategoryName", SubCategoryName.ToString())
                                };

               
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryName]");

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }
 
        public DataTable getSummaryOfAccountsORWithSubCategoryNameWithVAT(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName, int aftercoverletter)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),   
                                     new SqlParameter("@HasCompanyLetter", aftercoverletter),
                                   new SqlParameter("@SubCategoryName", SubCategoryName.ToString())
                                };


                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryNamewithVat]");

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public List<SummaryOfAccounts_DatatableLIST> SummaryOfAccount_EXCEL(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName, int aftercoverletter)
        {
            dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),
                                     new SqlParameter("@HasCompanyLetter", aftercoverletter),
                                   new SqlParameter("@SubCategoryName", SubCategoryName.ToString())
                                };


            DataTable dt = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryNamewithVat]");
             List<SummaryOfAccounts_DatatableLIST> list = new List<SummaryOfAccounts_DatatableLIST>();
            if (dt.Rows.Count > 0) list = dt.ToList<SummaryOfAccounts_DatatableLIST>();
            return list;
        }



        public DataTable getSummaryOfAccountsORWithSubCategoryNameWithVAT_Summary(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName, int aftercoverletter,int subCatId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),                             
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),    
                                    new SqlParameter("@HasCompanyLetter", aftercoverletter),
                                   new SqlParameter("@SubCategoryId", subCatId),
                                     
                              
                                };


                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryNamewithVat_SummaryType]");

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }


        public DataTable getSummaryOfAccountsORWithSubCategoryName_MedNet_green(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),
                                   new SqlParameter("@SubCategoryName", SubCategoryName.ToString())
                                };


                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryName_MedNetGREEN]");

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable getSummaryOfAccountsORWithSubCategoryName_MedNet(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),
                                   new SqlParameter("@SubCategoryName", SubCategoryName.ToString())
                                };


                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryName_MedNet]");

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable getSummaryOfAccountsBySubCategory(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, int subCategory)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),
                                   new SqlParameter("@SubCategory", subCategory.ToString())
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryWithSubCategory]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getUCAFRecordForPrinting(string visitIdJSONArray)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@visitIdJsonArray", visitIdJSONArray)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetUCAFRecords_JM]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getARAMCOPatientList(int patientStatus, int relationshipId)
        {

            var dataTable = new DataTable();

           try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@active", patientStatus.ToString()),
                                   new SqlParameter("@relationId", relationshipId.ToString())
                                };



                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_getARAMCOPatient]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }


        public DataTable getARPakageAndNonPackageInvoice(DateTime startDate, DateTime endDate,int categoryId, int deal, int reportOption)
        {
            //LEGEND
            //reportoption {0= bypin, 1= summary, 2 bycategorysummary}
            //deal {0 = package deal, 1= non package}

            var dataTable = new DataTable();
            var spName = "";
            var billType = deal == 0 ? "PD" : "NPD";

            if (reportOption == 0)
            {
                if(categoryId != 34)
                       spName = "SP_GET_IP_COMPANY_ARBILL";
                else
                       spName = "SP_GET_IP_COMPANY_ARBILL_CHARITY";
            }
            else if (reportOption == 1)
            {

                spName = "SP_GET_IP_COMPANY_ARBILL_SUMMARY";
            }
            else
            {
                spName = "SP_GET_IP_COMPANY_ARBILL_SUMMARY_CATEGORY";
            }



            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@BillType", billType),
                                   new SqlParameter("@coid", categoryId)
                                };



                dataTable = dbHelper.ExecuteSPAndReturnDataTable(spName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getARPatientPackage(DateTime startDate, DateTime endDate, int departmentId)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endate", endDate.ToString("dd-MMM-yyyy")),
                                    new SqlParameter("@deptid", departmentId.ToString())
                                 };



                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[dbo].[SP_GET_IPPACKAGE_DOCTOR]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getARTransactionDetails(DateTime startDate, DateTime endDate, int categoryId, int deptCategory)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@CatID", categoryId.ToString()),
                                   new SqlParameter("@DeptCat", deptCategory.ToString())
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[dbo].[SP_AROPTransactionDetails]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public List<ARPackageBill> getARPackageBills (string searchString) {
            var bills = new List<ARPackageBill>();
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT DISTINCT ");
                if (!String.IsNullOrEmpty(searchString))
                {
                queryBuilder.Append("TOP 100 ");
                }
                queryBuilder.Append("0 Id, REPLACE(LTRIM(RTRIM(PackageName)),'''','&#39;') PackageName FROM ARPackageBill ");
                queryBuilder.Append("WHERE NoofDays<=1 AND packagename <> '' ");
                if(!String.IsNullOrEmpty(searchString)){
                    queryBuilder.Append("AND packagename LIKE '%" + searchString.Trim()+ "%' ");
                }
                queryBuilder.Append("ORDER BY packagename ");

                bills = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<ARPackageBill>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }


            return bills;
        
        }

        public DataTable getARPackageDealDailyCases(DateTime startDate, DateTime endDate, string packages)
        {

            var dataTable = new DataTable();

            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT PACKAGENAME,COUNT(*) NOOFCASES, SUM(EditPackageAmount) amount from ARPackageBill ");
                queryBuilder.Append("WHERE Billdate >='" + startDate.ToString("dd-MMM-yyyy") + "' AND BillDate <'" + endDate.ToString("dd-MMM-yyyy") + "' ");
                queryBuilder.Append("and PACKAGENAME in (" );
                queryBuilder.Append("SELECT StringValue FROM fn_parseJSON('"+packages+"') WHERE parent_ID = 1 ");
                queryBuilder.Append(")and NoofDays<=1 group by PACKAGENAME ");
                dataTable = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getPhysiotherapyBilling(DateTime startDate, DateTime endDate, int categoryId)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@categoryId", categoryId.ToString())
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetPhysiotherapyBilling]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getIPServicesRequiredForApproval(DateTime startDate, DateTime endDate, int categoryId, string searchString)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@categoryId", categoryId.ToString()),
                                   new SqlParameter("@searchString", searchString)
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetIPServicesRequiredForApproval]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getBillingDepartmentWise(DateTime startDate, DateTime endDate, int categoryId, int patientTypeId)
        {

            var dataTable = new DataTable();
            var sp = "";
            var startDateString = startDate.ToString("dd-MMM-yyyy");
            var endDateString = endDate.ToString("dd-MMM-yyyy");

            if (patientTypeId == 0)
                sp = "[MCRS].[ARReports_ArBillDeptWiseNewIpOp]  '" + startDateString + "','" + endDateString + "'," + categoryId; //SP_GetARBill_DeptwiseNewIpOp
            else
                sp = "[MCRS].[ARReports_GetARBillDeptWiseNew] '" + startDateString + "','" + endDateString + "'," + categoryId + "," + patientTypeId; //SP_GetARBill_DeptwiseNew

            try
            {
                dataTable = dbHelper.ExecuteSQLAndReturnDataTable("exec " + sp);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public List<ARAdmission> getAdmissionByPin(int registrationNo) {

            var admission = new List<ARAdmission>();
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("Select c.Code, AdmitDateTime, a.IPID, b.BillNo,a.registrationno ");
                queryBuilder.Append("from OldInPatient a, ARIPBill B, Company c ");
                queryBuilder.Append("Where(a.IPID = b.IPID And b.CompanyId = c.id And a.Billtype = 2) ");
                queryBuilder.Append("and RegistrationNo = " + registrationNo+" ");
                queryBuilder.Append("Order BY AdmitDateTime Desc, a.IPID Desc");
                
                admission = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<ARAdmission>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }


            return admission;
        
        }

        public List<ARAdmission> getAdmissionBillNo(int billNo)
        {

            var admission = new List<ARAdmission>();
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("Select c.Code, AdmitDateTime, a.IPID, b.BillNo,a.registrationno ");
                queryBuilder.Append("from OldInPatient a, ARIPBill B, Company c ");
                queryBuilder.Append("Where(a.IPID = b.IPID And b.CompanyId = c.id And a.Billtype = 2) ");
                queryBuilder.Append("and b.slno = " + billNo + " ");
                queryBuilder.Append("Order BY AdmitDateTime Desc, a.IPID Desc");

                admission = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString()).DataTableToList<ARAdmission>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }


            return admission;

        }

        public DataTable getIPCRDiscount(int billNo)
        {

            var discounts = new DataTable();
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT TOP 7 ROUND(SUM(DISCOUNT)/SUM(AMOUNT),2) * 100 Percentage,C.DepartmentName,SUM(DISCOUNT) Discount ");
                queryBuilder.Append("FROM (SELECT CASE SERVICEID WHEN 2 THEN 'ROOM AND BOARD' ");
                queryBuilder.Append("WHEN 37 THEN 'TAKE HOME MEDICINE' ELSE B.NAME END AS  DepartmentName, ");
                queryBuilder.Append("SUM(EDITQUANTITY*DISCOUNT/EDITPRICE*EDITQUANTITY)*100 as DISCOUNTPERCENTAGE, ");
                queryBuilder.Append("SUM(EDITPRICE*EDITQUANTITY) AS AMOUNT,SUM(EDITQUANTITY*DISCOUNT) AS DISCOUNT, ");
                queryBuilder.Append("SUM(EDITQUANTITY*DEDUCTABLEAMOUNT) AS DEDUCTABLE ");
                queryBuilder.Append("FROM ARIPBILLITEMDETAIL A,DEPARTMENT b WHERE A.EDITPRICE<>0 AND EDITITEMID <> 0 ");
                queryBuilder.Append("AND A.DEPARTMENTID=B.ID AND Discount>0 And A.BILLNO= "+billNo+ " ");
                queryBuilder.Append("GROUP BY A.SERVICEID ,B.NAME ) C ");
                queryBuilder.Append("GROUP BY C.DEPARTMENTNAME ORDER BY C.DEPARTMENTNAME");

                discounts = dbHelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString());

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }


            return discounts;
        }

        public DataTable getNonPackageIPCR(int billNo)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@billNo", billNo.ToString())
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetNonPackageInPatientIPCR]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getPackageIPCR(int billNo)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@billNo", billNo.ToString())
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetPackageInPatientIPCR]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getSummarizedBillingByCategory(DateTime startDate, DateTime endDate, string patientType, string categoryIdsString) {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate.ToString()),
                                   new SqlParameter("@EndDate", endDate.ToString()),
                                   new SqlParameter("@PatientType", patientType),
                                   new SqlParameter("@CatToSearch", categoryIdsString)
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[dbo].[MCRS_GetARBill_DeptWise_ByCategory]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        
        }


        public DataTable getActiveServiceFrequency(DateTime startDate, DateTime endDate)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate.ToString()),
                                   new SqlParameter("@EndDate", endDate.ToString())
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("SP_Active_Service_FrequencyandPRice");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable getOPFrontlineTrack(DateTime startDate, DateTime endDate, string billNo, string searchCriteria)
        {
             var dataTable = new DataTable();

             billNo = billNo ==  null ? "" : billNo;

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@EndDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@BillNo", billNo),
                                   new SqlParameter("@Criteria", searchCriteria)
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[dbo].[SP_OP_Frontline_TrackerNew]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable getCancelledServices(DateTime startDate, DateTime endDate, int categoryId, int companyId, string patientType)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@categoryId", categoryId.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@patientType", patientType)
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetCancelledChargeServices]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable getOPBillPinWise(DateTime startDate, DateTime endDate, string pin, string choice, int category, int company)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@EndDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Choice", choice.ToString()),
                                   new SqlParameter("@Pin",pin.ToString()),
                                   new SqlParameter("@Category", category),
                                   new SqlParameter("@Company", company)
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_OPBillPinWise]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable getReferralPatients(DateTime startDate, DateTime endDate,  int CatId )
        {
            var dataTable = new DataTable();
         
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@EndDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@CatId", CatId),
                                  
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReport_ReferralPatients]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

         public DataTable getExpiringAccounts(DateTime startDate, int days )
        {
            var dataTable = new DataTable();
         
            try
       {
 
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startdate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@days", days),                                  
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReport_ExpiringAccounts]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }


         public DataTable getOPInvoiceBillPriting(DateTime stdate, DateTime endate, int catId, int regId)
         {
             var dataTable = new DataTable();

             try
             {
//                 [MCRS].[ARReport_getInvoicePrinting]
//@stdate datetime,@endate datetime,@catId varchar(max),@compId varchar(max), @regId varchar(max)

                 dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@stdate", stdate.ToString("dd-MMM-yyyy")),
                                    new SqlParameter("@endate", endate.ToString("dd-MMM-yyyy")),
                                    new SqlParameter("@catId", catId), 
                                    new SqlParameter("@compId", "0"),   
                                    new SqlParameter("@regId", regId)
                                 };

                 dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReport_getInvoicePrinting]");
             }
             catch (Exception ex)
             {
                 throw new ApplicationException(Errors.ExemptionMessage(ex));
             }
             return dataTable;

         }

         public DataTable getMonthlyIncomeCredit(DateTime startDate, DateTime endDate, DateTime yearDate, string categoryId, int companyId, string option, int gradeId)
         {
         
             var dataTable = new DataTable();
             try
             {

                 dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startdate", startDate.ToString("MM-dd-yyyy")),
                                   new SqlParameter("@endate",(option.ToString() == "yearly" ? "2016-10-01" :  endDate.ToString("MM-dd-yyyy"))),
                                   new SqlParameter("@yearDate", yearDate.ToString("MM-dd-yyyy")),
                                   new SqlParameter("@CategoryIds", (categoryId.Contains("\"0\"") == true || categoryId == "0"  ? "[0]" : categoryId.ToString())),
                                   new SqlParameter("@companyId", companyId ),
                                   new SqlParameter("@gradeId", gradeId ),
                                   new SqlParameter("@option", option.ToString())
                                };


                 dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetYearlyTotalSummaryAccounts]");

             }
             catch (Exception ex)
             {
                 throw new ApplicationException(Errors.ExemptionMessage(ex));
             }
             return dataTable;

         }

         public DataTable getMonthlyIncomeCreditSubCategoryWise(DateTime startDate, DateTime endDate, DateTime yearDate, int categoryId, int companyId, string option, int gradeId)
         {

             var dataTable = new DataTable();
             try
             {


                 //[ARReports_StatementSummaryperSubCategoryMonthly]

                 if (option.ToString() == "yearly")
                 {

                     dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startdate", startDate.ToString("dd-MM-yyyy")),
                                   new SqlParameter("@endDate",(option.ToString() == "yearly" ? "2016-10-01" :  endDate.ToString("dd-MM-yyyy"))),
                                   new SqlParameter("@yearDate", yearDate.ToString("dd-MM-yyyy")),
                                   new SqlParameter("@Category",  categoryId.ToString()),
                                   new SqlParameter("@companyId", companyId ),
                                      new SqlParameter("@IsYearly",(option.ToString() == "yearly" ? "1" : "0")),

                                };

                     dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_StatementSummaryperSubCategory]");
                 }
                 else {


                     dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startdate", startDate.ToString("yyyy-MM-dd")),//2013-02-01
                                   new SqlParameter("@endDate",(option.ToString() == "yearly" ? "2016-10-01" :  endDate.ToString("yyyy-MM-dd"))),
                                   new SqlParameter("@yearDate", yearDate.ToString("yyyy-MM-dd")),
                                   new SqlParameter("@Category",  categoryId.ToString()),
                                   new SqlParameter("@companyId", companyId ),
                                      new SqlParameter("@IsYearly",(option.ToString() == "yearly" ? "1" : "0")),

                                };

                     dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_StatementSummaryperSubCategoryMonthly]");
                 }
               

             }
             catch (Exception ex)
             {
                 throw new ApplicationException(Errors.ExemptionMessage(ex));
             }
             return dataTable;

         }


         public List<SummaryOfAccounts_VATSummaryEXCEL> getSummaryOfAccountsORWithSubCategoryNameWithVAT_SummaryEXCEL(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName, int aftercoverletter, int subCatId)
         {
             var list = new List<SummaryOfAccounts_VATSummaryEXCEL>();
             try
             {
                 dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),                             
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),    
                                    new SqlParameter("@HasCompanyLetter", aftercoverletter),
                                   new SqlParameter("@SubCategoryId", subCatId),
                                     
                              
                                };

                 list = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryNamewithVat_SummaryType]").DataTableToList<SummaryOfAccounts_VATSummaryEXCEL>();
             }
             catch (Exception ex)
             {
                 throw new ApplicationException(Errors.ExemptionMessage(ex));
             }

             return list;
         }




         public List<SummaryOfAccounts_DetailVATEXCEL> getSummaryOfAccountsORWithSubCategoryNameWithVATEXCEL(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId, int option, string SubCategoryName, int aftercoverletter)
         {
             var list = new List<SummaryOfAccounts_DetailVATEXCEL>();
             try
             {
                 dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@Option", option.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),   
                                     new SqlParameter("@HasCompanyLetter", aftercoverletter),
                                   new SqlParameter("@SubCategoryName", SubCategoryName.ToString())
                                };


                 list = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryNamewithVat]").DataTableToList<SummaryOfAccounts_DetailVATEXCEL>();
             }
             catch (Exception ex)
             {
                 throw new ApplicationException(Errors.ExemptionMessage(ex));
             }

             return list;
         }

        public DataTable PDNPDReportAfterCL(DateTime startDate, DateTime endDate, int categoryId, int gradeId, int companyId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", startDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@enDate", endDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@Category", categoryId.ToString()),
                                   new SqlParameter("@companyId", companyId.ToString()),
                                   new SqlParameter("@gradeId", gradeId.ToString()),
                                 
                                };


                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReports_PDNPDReportAfterCL]");

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable ReportDoctorListPatientRecord(DateTime fromDate, DateTime toDate,int DocId )
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", fromDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endDate", toDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@DOCID",  DocId)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReports_DocPatientRecord]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable ReportIPUninvoice(DateTime fromDate, DateTime toDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", fromDate.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@endDate", toDate.ToString("dd-MMM-yyyy"))
                            
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReport_IPUnInvoiceReport]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        public DataTable NetRevenue(DateTime fromDate, DateTime toDate,Enum patientType, Enum billtype, Enum billfinalize,int CategoryId, int IPID ,string RegNo, int cancelled)
        {
            var dataTable = new DataTable();
            try
            {

        

                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@DateFrom", fromDate.ToString("dd-MMM-yyyy"))
                                   ,new SqlParameter("@DateTo", toDate.ToString("dd-MMM-yyyy"))
                                   ,new SqlParameter("@PatientType", patientType.ToString())
                                    ,new SqlParameter("@BillType", billtype.ToString() == "ALL" ? 0 : (billtype.ToString() == "CASH" ? 1 : 2 ))
                                   ,new SqlParameter("@BillFinalized", (billfinalize.ToString() == "YES" ? 1 : 0 ))
                                   ,new SqlParameter("@CategoryId", CategoryId)
                                   ,new SqlParameter("@IPID", (IPID.ToString() == "" ? 0 : IPID))
                                  ,new SqlParameter("@RegistrationNo", (RegNo == null || RegNo.ToString()  == ""    ? "0" : RegNo))
                                   ,new SqlParameter("@CancelledOnly", cancelled )
                          
 
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[Net_Revenue_All]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }


        public DataTable Eod_DailyDoctorsTarget(DateTime fromDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@to", fromDate.ToString("dd-MMM-yyyy"))
                                   ,new SqlParameter("@IsNet", "G")
                                
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTableEODConnection("[EOD].[sp_DailyDoctorsTarget]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }

        
        public DataTable Eod_DailyRevenueDetail(DateTime fromDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@to", fromDate.ToString("dd-MMM-yyyy"))
                                   ,new SqlParameter("@IsNet", "G")

                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTableEODConnection("[EOD].[sp_DailyRevenueDetail]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }
        
     public DataTable Monthly_CIP_REVENUE(DateTime fromDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]
                                {
                                   new SqlParameter("@DateFrom", fromDate.ToString("dd-MMM-yyyy"))                                  
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("mcrs.Get_Monthly_CIP_Revenue");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }



    }


}


public class SummaryOfAccounts_DatatableLIST
{
    public string SubCategory { get; set; }
public string CCode { get; set; }
public string CompanyName { get; set; }
public string IPCount { get; set; }
public string OPCount { get; set; }
public string TCount { get; set; }
public string IPGross { get; set; }
public string IPDiscount { get; set; }
public string IPDeductable { get; set; }
public string IPNet { get; set; }
public string OPGross { get; set; }
public string OPDiscount { get; set; }
public string OPDeductable { get; set; }
public string OPNet { get; set; }
public string TGross { get; set; }
public string TDisc { get; set; }
public string TDeduc { get; set; }
public string TNet { get; set; }
public string Vat { get; set; }
public string NetVat { get; set; }
public string TNetVat { get; set; }
public string OPVat { get; set; }
public string IPVat { get; set; }
public string branch { get; set; }
public string address { get; set; }
public string slno { get; set; }
}
public class SummaryOfAccounts_VATSummaryEXCEL
{
    public string slno { get; set; }
    public string InsuranceName { get; set; }
    public string IPCount { get; set; }
    public string OPCount { get; set; }
    public string TCount { get; set; }
    public string IPGross { get; set; }
    public string IPNet { get; set; }
    public string IPVAT { get; set; }
    public string OPGross { get; set; }
    public string OPNet { get; set; }
    public string OPVat { get; set; }
    public string TGross { get; set; }
    public string TNet { get; set; }
    public string TNetVAT { get; set; }
    public string branch { get; set; }
    public string address { get; set; }



}

public class SummaryOfAccounts_DetailVATEXCEL
{
    public string SubCategory { get; set; }
    public string CCode { get; set; }
    public string CompanyName { get; set; }
    public string IPCount { get; set; }
    public string OPCount { get; set; }
    public string TCount { get; set; }
    public string IPGross { get; set; }
    public string IPDiscount { get; set; }
    public string IPDeductable { get; set; }
    public string IPNet { get; set; }
    public string OPGross { get; set; }
    public string OPDiscount { get; set; }
    public string OPDeductable { get; set; }
    public string OPNet { get; set; }
    public string TGross { get; set; }
    public string TDisc { get; set; }
    public string TDeduc { get; set; }
    public string TNet { get; set; }
    public string Vat { get; set; }
    public string NetVat { get; set; }
    public string TNetVat { get; set; }
    public string OPVat { get; set; }
    public string IPVat { get; set; }
    public string branch { get; set; }
    public string address { get; set; }
    public string slno { get; set; }


}

