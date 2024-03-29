USE [HIS]
GO
/****** Object:  StoredProcedure [MCRS].[RadiologyReports_GetAnesthesiaReport]    Script Date: 10/2/2016 3:26:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<SP [dbo].[SP_GetAnesthesiaScheduleReport]>
-- =============================================
ALTER PROCEDURE [MCRS].[RadiologyReports_GetAnesthesiaReport]
(@stdate datetime, @endate datetime)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
DECLARE @SA Varchar(255) SET @SA = (Select top 1 IssueAuthorityCode from OrganisationDetails )


select 
'Out-Patient' PatientType,
b.name RequestFrom,
@SA+'.' + REPLICATE('0', (10-LEN(a.ipidopid))) + CONVERT(VARCHAR,a.ipidopid) RegistrationNo,   
isnull(c.FamilyName,'') + ' ' + isnull(c.Firstname,'') + ' ' + isnull(c.MiddleName,'') + ' ' + isnull(c.lastname,'') as PatientName,
case when datediff(hh,c.dateofBirth,Getdate())<24       
then 1 
else       
case when datediff(hh,c.dateofBirth,Getdate())>=24 and datediff(hh,c.dateofBirth,Getdate())<720      
then datediff(hh,c.dateofBirth,Getdate())/24 else       
case when datediff(hh,c.dateofBirth,Getdate())>=720 and datediff(hh,c.dateofBirth,Getdate())<8760      
then datediff(hh,c.dateofBirth,Getdate())/720 else      
case When datediff(hh,c.dateofBirth,Getdate())>=8760      
then datediff(hh,c.dateofBirth,Getdate())/8760 end end      
end end Age,       
case when datediff(hh,c.dateofBirth,Getdate())<24       
then 'Day(s)' else       
case when datediff(hh,c.dateofBirth,Getdate())>=24 and datediff(hh,c.dateofBirth,Getdate())<720      
then 'Day(s)' else       
case when datediff(hh,c.dateofBirth,Getdate())>=720 and datediff(hh,c.dateofBirth,Getdate())<8760      
then 'Month(s)' else      
case When datediff(hh,c.dateofBirth,Getdate())>=8760      
then 'Year(s)' end end      
end end AgeType,
a.fromdatetime FromDate,a.todatetime ToDate,
d.code ServiceCode, d.name ServiceName,
Case when a.ReservedConfirmed =4 then 'Reserved' else 'Confirmed' end Status
from otschedule a
left join otno b on a.OTID = b.id 
left join patient c on a.IPIDOPID = c.registrationno
left join Anaesthesia d on a.AnaesthesiaID = d.id
where a.reservedconfirmed in(4,1) and a.deleted = 0 
and (a.fromdatetime >= @stdate and a.fromdatetime < @endate) 
order by a.fromdatetime
END
