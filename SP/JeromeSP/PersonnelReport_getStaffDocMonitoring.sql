-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
alter PROCEDURE [MCRS].[PersonnelReport_getStaffDocMonitoring]
(@stdate datetime = '2016-09-01', @endate datetime = '2016-10-01',
@empId int = '0',@deptId int = '0'
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  Select b.doa,a.employeeid,a.name as fullname,c.deptcode,d.name,isnull(e.cv,0) cv, 
              isnull(e.ORIENT_DEPT,0) ORIENT_DEPT, 
              isnull(e.ORIENT_GEN,0) ORIENT_GEN, 
              isnull(e.JD,0) JD, 
              isnull(e.LICENSE,0) LICENSE, 
              isnull(e.EDUC_CERT,0) EDUC_CERT, 
              isnull(e.FS,0) FS, 
              isnull(e.IFC,0) IFC, 
              isnull(e.TQM,0) TQM, 
              isnull(e.BCLS,0) BCLS, 
              isnull(e.ACLS,0) ACLS, 
              isnull(e.EVAL_1,0) EVAL_1, 
              isnull(e.EVAL_2,0) EVAL_2, 
              isnull(e.EVAL_3,0) EVAL_3, 
              isnull(e.EVAL_4,0) EVAL_4, 
              isnull(e.CONFIDENTIALITY,0) CONFIDENTIALITY, 
              isnull(e.CREDENTIALING,0) CREDENTIALING, 
              isnull(e.PREVILEDGING,0) PREVILEDGING 
              From employee a 
              right join salarydetail b on a.id = b.id 
              left join department c on a.departmentid = c.id 
              left join designation d on a.designationid = d.id 
              left join mcrs_staffdocuments e on a.employeeid = e.employeeid 
              where a.deleted=0 
			  and (@empId = '0' OR a.employeeid = @empId)
			  and (@deptId = '0' OR a.departmentid = @deptId)
			  and b.doa >= @stdate and b.doa < @endate
			  order by b.DOA desc


END
GO
