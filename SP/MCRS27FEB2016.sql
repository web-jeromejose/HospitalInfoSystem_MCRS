USE [HIS]
GO

/****** Object:  StoredProcedure [MCRS].[dashboard_daily_view]    Script Date: 02/27/2016 14:56:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MCRS].[dashboard_daily_view]') AND type in (N'P', N'PC'))
DROP PROCEDURE [MCRS].[dashboard_daily_view]
GO

/****** Object:  StoredProcedure [MCRS].[get_opbill_actual_amount]    Script Date: 02/27/2016 14:56:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MCRS].[get_opbill_actual_amount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [MCRS].[get_opbill_actual_amount]
GO

/****** Object:  StoredProcedure [MCRS].[get_opbill_actual_count]    Script Date: 02/27/2016 14:56:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MCRS].[get_opbill_actual_count]') AND type in (N'P', N'PC'))
DROP PROCEDURE [MCRS].[get_opbill_actual_count]
GO

USE [HIS]
GO

/****** Object:  StoredProcedure [MCRS].[dashboard_daily_view]    Script Date: 02/27/2016 14:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [MCRS].[dashboard_daily_view]
@date DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @IP_CURRENT_IN INT, @IP_CURRENT_IN_FTD INT,
			@IP_ADMISSION INT, @IP_ADMISSION_FTD INT,
			@IP_DISCHARGE INT, @IP_DISCHARGE_FTD INT
		
	SET @IP_CURRENT_IN = 0
	SET @IP_CURRENT_IN_FTD = 0
	SET @IP_ADMISSION = 0
	SET @IP_ADMISSION_FTD = 0
	SET @IP_DISCHARGE = 0
	SET @IP_DISCHARGE_FTD = 0
	
	-- IP
	
	-- currently in
	SELECT @IP_CURRENT_IN = COUNT(A.IPID)
            FROM HIS..AllInpatients AS a
            INNER JOIN HIS..BedTransfers c 
            ON a.IPID = c.ipid LEFT JOIN HIS..Category AS b ON a.CategoryID = b.ID 
            WHERE (dischargedatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')) OR dischargedatetime IS NULL) 
            AND admitdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))  
            AND admitdatetime >= '01-Jan-2007' 
            AND C.ID IN(SELECT TOP 1 d.id FROM HIS..BedTransfers d 
				LEFT JOIN his..bed e ON d.bedid = e.id WHERE d.IPID = a.IPID 
				AND e.name NOT LIKE 'ER%' ORDER BY d.id DESC) 
	-- currently ftd			
	SELECT @IP_CURRENT_IN_FTD = COUNT(A.IPID) 
            FROM HIS..AllInpatients AS a 
            INNER JOIN HIS..BedTransfers c ON a.IPID = c.ipid LEFT JOIN HIS..Category AS b ON a.CategoryID = b.ID 
            WHERE (dischargedatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'), 106), ' ', '-')) OR dischargedatetime IS NULL)
            AND admitdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'), 106), ' ', '-')) 
            AND admitdatetime >= CAST('01-Jan-2007' AS DATETIME)  
            AND C.ID IN(SELECT TOP 1 d.id FROM HIS..BedTransfers d 
            LEFT JOIN his..bed e ON d.bedid = e.id WHERE d.IPID = a.IPID 
            AND e.name NOT LIKE 'ER%' ORDER BY d.id DESC) 		
						
    -- admission 
    SELECT @IP_ADMISSION = COUNT(A.IPID)
            FROM HIS..AllInpatients AS a
            INNER JOIN HIS..BedTransfers c ON a.IPID = c.ipid LEFT JOIN HIS..Category AS b ON a.CategoryID = b.ID 
            WHERE admitdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')
            AND admitdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
            AND C.ID IN(SELECT TOP 1 d.id FROM HIS..BedTransfers d 
            LEFT JOIN his..bed e ON d.bedid = e.id WHERE d.IPID = a.IPID 
            ORDER BY d.id DESC)  
            
    -- admission ftd  
    SELECT @IP_ADMISSION_FTD = COUNT(A.IPID)
                FROM HIS..AllInpatients AS a
                INNER JOIN HIS..BedTransfers c ON a.IPID = c.ipid LEFT JOIN HIS..Category AS b ON a.CategoryID = b.ID
                WHERE admitdatetime > REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-')
                AND admitdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'))
            AND C.ID IN(SELECT TOP 1 d.id FROM HIS..BedTransfers d 
            LEFT JOIN his..bed e ON d.bedid = e.id WHERE d.IPID = a.IPID 
            AND e.name NOT LIKE 'ER%' ORDER BY d.id DESC)          

    -- discharge
    SELECT @IP_DISCHARGE = COUNT(A.IPID)
            FROM HIS..AllInpatients AS a 
            INNER JOIN HIS..BedTransfers c ON a.IPID = c.ipid LEFT JOIN HIS..Category AS b ON a.CategoryID = b.ID
            WHERE dischargedatetime > REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')
            AND dischargedatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
            AND C.ID IN(SELECT TOP 1 d.id FROM HIS..BedTransfers d  
            LEFT JOIN his..bed e ON d.bedid = e.id WHERE d.IPID = a.IPID 
            ORDER BY d.id DESC)
    -- discharge today
    SELECT @IP_DISCHARGE_FTD = COUNT(A.IPID)
            FROM HIS..AllInpatients AS a
            INNER JOIN HIS..BedTransfers c ON a.IPID = c.ipid LEFT JOIN HIS..Category AS b ON a.CategoryID = b.ID
            WHERE dischargedatetime > REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-')
            AND dischargedatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'))
            AND C.ID IN(SELECT TOP 1 d.id FROM HIS..BedTransfers d 
            LEFT JOIN his..bed e ON d.bedid = e.id WHERE d.IPID = a.IPID 
            AND e.name NOT LIKE 'ER%' ORDER BY d.id DESC)          
    

   -- DELIVERY    
    
	DECLARE 
		@DE_FOR_THE_MONTH INT,
		@DE_FOR_THE_DAY INT,
		@DE_TODAY INT
		
		SET @DE_FOR_THE_MONTH = 0
		SET @DE_FOR_THE_DAY = 0
		SET @DE_TODAY = 0
	
	SELECT @DE_FOR_THE_MONTH = COUNT(*)
    FROM middleware..deliverystat 
		WHERE MONTH(delivery_date) = MONTH(REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')) AND YEAR(delivery_date) = YEAR(REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')) AND deleted=0
		
	SELECT @DE_FOR_THE_DAY = COUNT(*)
    FROM middleware..deliverystat 
		WHERE delivery_date = REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')
	
	SELECT @DE_TODAY = COUNT(*)
    FROM middleware..deliverystat WHERE delivery_date = REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-') AND deleted=0
    
    
    -- ER
    DECLARE 
		@ER_CONSULTATION INT,
		@ER_CONSULTATION_FTD INT,
		@ER_ADMISSION INT,
		@ER_ADMISSION_FTD INT

		SET @ER_CONSULTATION = 0
		SET @ER_CONSULTATION_FTD = 0
		SET @ER_ADMISSION = 0
		SET @ER_ADMISSION_FTD = 0
		
    SELECT @ER_CONSULTATION = COUNT(*)  
            FROM his..opcompanybilldetail WHERE billdatetime 
            >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') 
            AND billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
            AND serviceid=2 AND itemcode LIKE 'ER%'
    
    	
    SELECT @ER_CONSULTATION_FTD = COUNT(*)  
            FROM his..opcompanybilldetail WHERE billdatetime 
            >= REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-') 
            AND billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'))
            AND serviceid=2 AND itemcode LIKE 'ER%'
    
    SELECT @ER_ADMISSION = COUNT(*)
    FROM his..allinpatients 
    WHERE 
    admitdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')
    AND admitdatetime < DATEADD(d,1, REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-')) AND admitedatid=1 
        
    SELECT @ER_ADMISSION_FTD = COUNT(*)
    FROM his..allinpatients 
    WHERE 
    admitdatetime >= REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-')
    AND admitdatetime < DATEADD(d,1,REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-')) AND admitedatid=1 

	-- OP TODAY
	 DECLARE 
		@OP_CURTOTAL_FTD INT, @OP_CURPAID_FTD INT, @OP_CURFREE_FTD INT
		
		SET @OP_CURTOTAL_FTD = 0
		SET @OP_CURPAID_FTD = 0
		SET @OP_CURFREE_FTD = 0
		
	SELECT @OP_CURTOTAL_FTD = COUNT(*) 
	FROM dbo.opcompanybilldetail 
	WHERE billdatetime >= REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-') 
	AND billdatetime < DATEADD(D,1, REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-')) AND serviceid=2
	
	SELECT  @OP_CURPAID_FTD = COUNT(*)
	FROM dbo.opcompanybilldetail a 
	LEFT JOIN opdoctororder b ON a.opbillid=b.opbillid
	LEFT JOIN opdoctororderdetail c ON b.id=c.opdoctororderid 
	WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-') 
	AND a.billdatetime < DATEADD(D,1, REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'))
	AND a.serviceid=2 AND c.type=1 
             
	SELECT  @OP_CURFREE_FTD = COUNT(*)
	FROM dbo.opcompanybilldetail a 
	LEFT JOIN opdoctororder b ON a.opbillid=b.opbillid
	LEFT JOIN opdoctororderdetail c ON b.id=c.opdoctororderid 
	WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-') 
	AND a.billdatetime < DATEADD(D,1, REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'))
	AND a.serviceid=2 AND c.type=2  
	
	
	-- OP CASH 
	
	  DECLARE 
		@OP_CASH_TOTAL INT,
		@OP_CASH_PAIDCOUNT INT,
		@OP_CASH_FREECOUNT INT
		
		SET @OP_CASH_TOTAL = 0
		SET @OP_CASH_PAIDCOUNT = 0
		SET @OP_CASH_FREECOUNT = 0
	
	SELECT @OP_CASH_TOTAL = SUM(x.NO_PATIENT) FROM (SELECT COUNT(*) NO_PATIENT
	FROM opcompanybilldetail a 
	LEFT JOIN opdoctororder b ON a.opbillid=b.opbillid 
	LEFT JOIN opdoctororderdetail c ON b.id=c.opdoctororderid
	WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') AND a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	AND a.serviceid=2 AND companyid=1
	UNION ALL 
	SELECT COUNT(*) NO_PATIENT 
	FROM canopcompanybilldetail a 
	LEFT JOIN canopdoctororder b ON a.opbillid=b.opbillid 
	LEFT JOIN canopdoctororderdetail c ON b.id=c.opdoctororderid 
	WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') AND a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	AND a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	AND a.serviceid=2 AND companyid=1) X
	
	
	select @OP_CASH_PAIDCOUNT = sum(x.NO_PATIENT) from (Select count(*) NO_PATIENT 
	from opcompanybilldetail a
	left join opdoctororder b on a.opbillid=b.opbillid 
	left join opdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.serviceid=2 and a.companyid=1 and c.type=1 
	UNION ALL 
	Select count(*) NO_PATIENT 
	from canopcompanybilldetail a 
	left join canopdoctororder b on a.opbillid=b.opbillid 
	left join canopdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.serviceid=2 and companyid=1 and c.type=1 ) X
	
	
	select @OP_CASH_FREECOUNT = sum(x.NO_PATIENT) from (Select count(*) NO_PATIENT 
	from opcompanybilldetail a
	left join opdoctororder b on a.opbillid=b.opbillid 
	left join opdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.serviceid=2 and a.companyid=1 and c.type=2 
	UNION ALL 
	Select count(*) NO_PATIENT 
	from canopcompanybilldetail a 
	left join canopdoctororder b on a.opbillid=b.opbillid 
	left join canopdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.serviceid=2 and companyid=1 and c.type=2 ) X

	-- OP CHARGE
	
	  DECLARE 
		@OP_CHARGE_TOTAL INT,
		@OP_CHARGE_PAIDCOUNT INT,
		@OP_CHARGE_FREECOUNT INT
		
		SET @OP_CHARGE_TOTAL = 0
		SET @OP_CHARGE_PAIDCOUNT = 0
		SET @OP_CHARGE_FREECOUNT = 0
	
	SELECT @OP_CHARGE_TOTAL = SUM(x.NO_PATIENT) FROM (SELECT COUNT(*) NO_PATIENT
	FROM opcompanybilldetail a 
	LEFT JOIN opdoctororder b ON a.opbillid=b.opbillid 
	LEFT JOIN opdoctororderdetail c ON b.id=c.opdoctororderid
	WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') AND a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	AND a.serviceid=2 AND companyid not in (1)--, 4611)
	UNION ALL 
	SELECT COUNT(*) NO_PATIENT 
	FROM canopcompanybilldetail a 
	LEFT JOIN canopdoctororder b ON a.opbillid=b.opbillid 
	LEFT JOIN canopdoctororderdetail c ON b.id=c.opdoctororderid 
	WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') AND a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	AND a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	-- CAIRO
	AND a.serviceid=2 AND companyid not in (1)--, 4611)
	-- JEDDAH include aramco
	--AND a.serviceid=2 AND companyid not in (1, 4611)
	) X
	
	select @OP_CHARGE_PAIDCOUNT = sum(x.NO_PATIENT) from (Select count(*) NO_PATIENT 
	from opcompanybilldetail a
	left join opdoctororder b on a.opbillid=b.opbillid 
	left join opdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.serviceid=2 and a.companyid not in(1)--, 4611)
	 and c.type=1 
	UNION ALL 
	Select count(*) NO_PATIENT 
	from canopcompanybilldetail a 
	left join canopdoctororder b on a.opbillid=b.opbillid 
	left join canopdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--and a.serviceid=2 and companyid=1 and c.type=1 
	and a.serviceid=2 and companyid not in(1)--, 4611) 
	and c.type=1 
	--and a.serviceid=2 and companyid not in(1, 4611) and c.type=1 
	) X
	
	select @OP_CHARGE_FREECOUNT = sum(x.NO_PATIENT) from (Select count(*) NO_PATIENT 
	from opcompanybilldetail a
	left join opdoctororder b on a.opbillid=b.opbillid 
	left join opdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.serviceid=2 and a.companyid not in(1)--, 4611) 
	and c.type=2 
	UNION ALL 
	Select count(*) NO_PATIENT 
	from canopcompanybilldetail a 
	left join canopdoctororder b on a.opbillid=b.opbillid 
	left join canopdoctororderdetail c on b.id=c.opdoctororderid 
	where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	and a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--and a.serviceid=2 and companyid=1 and c.type=1 
	and a.serviceid=2 and companyid not in(1)--, 4611) 
	and c.type=2 
	--and a.serviceid=2 and companyid not in(1, 4611) and c.type=1 
	) X 
	
	DECLARE 
		@OP_ARAMCO_TOTAL INT,
		@OP_ARAMCO_PAIDCOUNT INT,
		@OP_ARAMCO_FREECOUNT INT
		
		SET @OP_ARAMCO_TOTAL = 0
		SET @OP_ARAMCO_PAIDCOUNT = 0
		SET @OP_ARAMCO_FREECOUNT = 0
	
	--SELECT @OP_ARAMCO_TOTAL = SUM(x.NO_PATIENT) FROM (SELECT COUNT(*) NO_PATIENT
	--FROM opcompanybilldetail a 
	--LEFT JOIN opdoctororder b ON a.opbillid=b.opbillid 
	--LEFT JOIN opdoctororderdetail c ON b.id=c.opdoctororderid
	--WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') AND a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--AND a.serviceid=2 AND companyid = 4611 
	--UNION ALL 
	--SELECT COUNT(*) NO_PATIENT 
	--FROM canopcompanybilldetail a 
	--LEFT JOIN canopdoctororder b ON a.opbillid=b.opbillid 
	--LEFT JOIN canopdoctororderdetail c ON b.id=c.opdoctororderid 
	--WHERE a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') AND a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--AND a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	---- CAIRO
	--AND a.serviceid=2 AND companyid = 4611 
	---- JEDDAH include aramco
	----AND a.serviceid=2 AND companyid not in (1, 4611)
	--) X
	
	--select @OP_ARAMCO_PAIDCOUNT = sum(x.NO_PATIENT) from (Select count(*) NO_PATIENT 
	--from opcompanybilldetail a
	--left join opdoctororder b on a.opbillid=b.opbillid 
	--left join opdoctororderdetail c on b.id=c.opdoctororderid 
	--where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--and a.serviceid=2 and a.companyid= 4611 and c.type=1 
	--UNION ALL 
	--Select count(*) NO_PATIENT 
	--from canopcompanybilldetail a 
	--left join canopdoctororder b on a.opbillid=b.opbillid 
	--left join canopdoctororderdetail c on b.id=c.opdoctororderid 
	--where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--and a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	----and a.serviceid=2 and companyid=1 and c.type=1 
	--and a.serviceid=2 and companyid = 4611 and c.type=1 
	----and a.serviceid=2 and companyid not in(1, 4611) and c.type=1 
	--) X
	
	--select @OP_ARAMCO_FREECOUNT = sum(x.NO_PATIENT) from (Select count(*) NO_PATIENT 
	--from opcompanybilldetail a
	--left join opdoctororder b on a.opbillid=b.opbillid 
	--left join opdoctororderdetail c on b.id=c.opdoctororderid 
	--where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--and a.serviceid=2 and a.companyid=4611 and c.type=2 
	--UNION ALL 
	--Select count(*) NO_PATIENT 
	--from canopcompanybilldetail a 
	--left join canopdoctororder b on a.opbillid=b.opbillid 
	--left join canopdoctororderdetail c on b.id=c.opdoctororderid 
	--where a.billdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and a.billdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	--and a.canceldatetime >= DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	----and a.serviceid=2 and companyid=1 and c.type=1 
	--and a.serviceid=2 and companyid = 4611  and c.type=2 
	----and a.serviceid=2 and companyid not in(1, 4611) and c.type=1 
	--) X
	
	
	
	-- OR
	
	DECLARE 
		@OR_FORTHEDAY INT,
		@OR_FORTHEDAY_FTD INT,
		@OR_SCHEDULE INT,
		@OR_SCHEDULE_FTD INT,
		@OR_ORIGSCHED INT
		
		SET @OR_FORTHEDAY = 0
		SET @OR_FORTHEDAY_FTD = 0
		SET @OR_SCHEDULE = 0
		SET @OR_SCHEDULE_FTD = 0
		SET @OR_ORIGSCHED = 0
		
	Select @OR_FORTHEDAY = count(*)
	from dbo.OTOrder where otstartdatetime >= REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-') and otstartdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), @date, 106), ' ', '-'))
	
	Select @OR_FORTHEDAY_FTD = count(*)
	from dbo.OTOrder where otstartdatetime >= REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-') and otstartdatetime < DATEADD(D,1,REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-'))
	



	--select * from OTORder
	 --select * from OTSchedule
	 --select * from OTOrderDetail
	

	  
   SELECT 
   @IP_CURRENT_IN IPCurrentlyIn, 
   @IP_CURRENT_IN_FTD IPCurrentlyInFTD,
   
   @IP_ADMISSION IPAdmit, 
   @IP_ADMISSION_FTD IPAdmitFTD,
   
   @IP_DISCHARGE IPDis, 
   @IP_DISCHARGE_FTD IPDisFTD,
   
   @DE_FOR_THE_MONTH DEForTheMonth, 
   @DE_FOR_THE_DAY DEForTheDay,
   @DE_TODAY DEToday,
   @ER_CONSULTATION ERCons,
   @ER_CONSULTATION_FTD ERConsForToday,
   @ER_ADMISSION ERAdmit,
   @ER_ADMISSION_FTD ERAdmitForToday,
   
   @OP_CURTOTAL_FTD OPCurTotal,
   @OP_CURPAID_FTD OPCurPaid ,
   @OP_CURFREE_FTD OPCurFree,
  
   @OP_CASH_TOTAL  OPCashTotal,
   @OP_CASH_PAIDCOUNT OPCashPaidCount,
   @OP_CASH_FREECOUNT OPCashFreeCount,
   
   @OP_CHARGE_TOTAL  OPChargeTotal,
   @OP_CHARGE_PAIDCOUNT OPChargePaidCount,
   @OP_CHARGE_FREECOUNT OPChargeFreeCount,
   
   @OP_ARAMCO_TOTAL  OPAramcoTotal,
   @OP_ARAMCO_PAIDCOUNT OPAramcoPaidCount,
   @OP_ARAMCO_FREECOUNT OPAramcoFreeCount,
   
   @OR_FORTHEDAY ORForTheDay,
   @OR_FORTHEDAY_FTD ORForTheDayFTD
   
   
   
   
END

-- MCRS.dashboard_daily_view '23-FEB-2016'

GO

/****** Object:  StoredProcedure [MCRS].[get_opbill_actual_amount]    Script Date: 02/27/2016 14:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [MCRS].[get_opbill_actual_amount]
	@fdate DATETIME,
	@tdate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT SUM(amount) AS opactualamount FROM dbo.opbill
	WHERE DATETIME >= @fdate AND DATETIME < DATEADD(D,1, @tdate)
END


GO

/****** Object:  StoredProcedure [MCRS].[get_opbill_actual_count]    Script Date: 02/27/2016 14:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [MCRS].[get_opbill_actual_count]
	@fdate DATETIME,
	@tdate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT count(*) AS opactualcount FROM dbo.opbill
	WHERE DATETIME >= @fdate AND DATETIME < DATEADD(D,1, @tdate)
END


GO

