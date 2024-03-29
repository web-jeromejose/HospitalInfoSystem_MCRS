USE [HIS]
GO
/****** Object:  StoredProcedure [MCRS].[PolyClinicReports_GetCancelledPatientReservation]    Script Date: 10/26/2016 12:52:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jason F. Morales
-- Create date: 14-June-2016
-- Description:	Get Cancelled Patient Reservation
-- =============================================
ALTER PROCEDURE [MCRS].[PolyClinicReports_GetCancelledPatientReservation] 
	(@startDate DATETIME, @endDate DATETIME , @operatorId INT, @patientType INT, @doctorId INT, @departmentId INT)

AS
BEGIN


	SET NOCOUNT ON;
	
	 --   DECLARE  @startDate DATETIME, @endDate DATETIME
		--DECLARE @operatorId INT, @patientType INT,@doctorId INT, @departmentId INT

		--SET @startDate	  = '5/21/2016 1:29:22 PM'
		--SET @endDate	  = '6/15/2016 1:29:22 PM'
		--SET @operatorId   = 0
		--SET @patientType  = 0
		--SET @doctorId     = 0
		--SET @departmentId = 0
		DECLARE @issueAuthorityCode AS VARCHAR(20), @branch AS VARCHAR(100)

SET @issueAuthorityCode = (SELECT TOP 1 IssueAuthorityCode FROM OrganisationDetails)
SET @branch = (SELECT TOP 1 Name + ' - '+ UPPER (City)FROM OrganisationDetails)

SELECT 
    ROW_NUMBER() OVER(ORDER BY a.FromDateTime,a.appointmentno)slno,
    CASE WHEN a.patienttype = 3 THEN 'OP' ELSE 'IP' END PatientType,
    a.appointmentno,
    CASE 
		WHEN a.ipidopid = 0 THEN '' 
		ELSE (@issueAuthorityCode + '.' + REPLICATE('0',10-(LEN(CONVERT(Varchar(10),a.ipidopid)))) + CONVERT(Varchar(10),a.ipidopid)) 
    END AS PIN,          
	a.patientname,
	a.fromdatetime,
	c.Name  DepartmentName,
	b.empcode + '-' + b.name Doctor,
    a.dateofbooking,
    a.OperatorId BookedBy,
    d.CanDateTime,
    f.Name CancelledBy,
    @branch branch
    
	FROM doctorschedule a
	LEFT JOIN doctor b 
	ON a.doctorid = b.id 
	JOIN Department c
	ON b.DepartmentID = c.ID
	JOIN oldDoctorSchedule d
	ON d.IPIDOPID = a.IPIDOPID
	AND D.AppointmentNo = A.AppointmentNo
    JOIN Employee f
	on  d.CanOperatorId = f.ID
	WHERE  (@operatorId = 0 OR d.CanOperatorId = @operatorId)
	--AND a.dateofbooking >= @startDate AND a.dateofbooking < @endDate
	AND d.CanDateTime >= @startDate AND d.CanDateTime < @endDate
	AND a.deleted = 2 
	AND (@patientType = 0 OR a.patienttype = @patientType)
	AND (@doctorId = 0 OR b.ID = @doctorId)
	AND (@departmentId = 0 OR c.ID = @departmentId)
	
END
