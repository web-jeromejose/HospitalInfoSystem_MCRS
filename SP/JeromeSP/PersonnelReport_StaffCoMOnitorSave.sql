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
-- Author:		<Jerome Jose>
-- Create date: <Oct 2 2016>
-- Description:	<Description,,>
-- =============================================
alter PROCEDURE [MCRS].[PersonnelReport_StaffDocmonitoringSave]
(@employeeid varchar(255), @fullname varchar(255), @deptcode varchar(255), @name varchar(255), @cv varchar(255), @orient_dept varchar(255), @orient_gen varchar(255), @jd varchar(255), @license varchar(255),
 @educ_cert varchar(255), @fs varchar(255), @ifc varchar(255), @tqm varchar(255), @bcls varchar(255), @acls varchar(255), @eval_1 varchar(255),
  @eval_2 varchar(255), @eval_3 varchar(255), @eval_4 varchar(255),  @confidentiality varchar(255), @credentialing varchar(255), @previledging varchar(255)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	IF(@cv = '1' OR @orient_dept = '1' OR @orient_gen = '1' OR @jd = '1' OR @license = '1' OR @educ_cert = '1' OR @fs = '1' OR @ifc = '1' OR @tqm = '1' OR @bcls = '1' OR @acls = '1' OR @eval_1 = '1' OR @eval_2 = '1' OR @eval_3 = '1' OR @eval_4 = '1' OR @confidentiality = '1' OR @credentialing = '1' OR @previledging = '1')
	BEGIN
			delete from mcrs_staffdocuments where employeeid = @employeeid

			Insert into mcrs_staffdocuments(EMPLOYEEID,CV,ORIENT_DEPT,ORIENT_GEN,JD,LICENSE,EDUC_CERT,FS,IFC,TQM,BCLS,ACLS,EVAL_1,EVAL_2,EVAL_3,EVAL_4,CONFIDENTIALITY,CREDENTIALING,PREVILEDGING)
			                         values(@employeeid,@cv,@orient_dept,@orient_gen,@jd,@license,@educ_cert,@fs,@ifc,@tqm,@bcls,@acls,@eval_1,@eval_2,@eval_3,@eval_4,@confidentiality,@credentialing,@previledging)

	END
	ELSE
	BEGIN
			delete from mcrs_staffdocuments where employeeid = @employeeid
	END

END
GO
