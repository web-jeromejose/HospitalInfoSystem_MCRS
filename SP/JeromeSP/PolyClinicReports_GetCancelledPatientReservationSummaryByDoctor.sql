USE [HIS]
GO
/****** Object:  StoredProcedure [MCRS].[PolyClinicReports_GetCancelledPatientReservationSummaryByDoctor]    Script Date: 10/26/2016 10:26:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jason F. Morales
-- Create date: 18-May-2016
-- Description:	GetPatientReservationSummary By Doctor
-- =============================================
ALTER PROCEDURE [MCRS].[PolyClinicReports_GetCancelledPatientReservationSummaryByDoctor] 
(@startDate DATETIME, @endDate DATETIME , @doctorId INT, @patientType INT)


AS
BEGIN


	SET NOCOUNT ON;
	
	    --DECLARE  @startDate DATETIME, @endDate DATETIME
		--DECLARE @operatorId INT

		--SET @startDate	=  '5/21/2016 1:29:22 PM'
		--SET @endDate	='5/22/2016 1:29:22 PM'
		--SET @operatorId = 0
           DECLARE @branch VARCHAR(100)
           SET @branch =(SELECT TOP 1 Name +' - '+ City FROM OrganisationDetails)
           
           SELECT   b.EmpCode
					,b.Name
					,CASE WHEN a.patienttype = 3 THEN 'OP' ELSE 'IP' END PT_Type
					,COUNT(*) AS xcount
					,UPPER(@branch) Branch
					FROM oldDoctorSchedule a
				    LEFT JOIN doctor b 
					ON a.DoctorId=b.ID
					--LEFT JOIN oldDoctorSchedule d ON d.IPIDOPID = a.IPIDOPID --added for cancelled date

					WHERE a.CanDateTime >= @startDate AND a.CanDateTime < @endDate  --added for cancelled date
					-- a.DateOfBooking >= @startDate AND a.DateOfBooking < @endDate
					AND a.reservedconfirmed=1 
					--AND a.deleted=2
					AND (@doctorId = 0 OR a.DoctorId = @doctorId)	--op = 3, ip = 2
                    AND (@patientType = 0 OR a.PatientType = @patientType)
				    GROUP BY b.EmpCode,b.Name,a.patienttype
					ORDER BY Name
END
