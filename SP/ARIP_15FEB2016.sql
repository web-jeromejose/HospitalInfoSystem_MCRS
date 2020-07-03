USE [HIS]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_breakup_invoice_npd]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[generate_breakup_invoice_npd]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[generate_breakup_invoice_npd]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_main_invoice]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[generate_main_invoice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[generate_main_invoice]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_main_invoice_npd]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[generate_main_invoice_npd]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[generate_main_invoice_npd]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_main_invoice_pd]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[generate_main_invoice_pd]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[generate_main_invoice_pd]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_account_list]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_account_list]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_account_list]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_bill_list_npd]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_bill_list_npd]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_bill_list_npd]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_billlist_npd_batch]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_billlist_npd_batch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_billlist_npd_batch]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_discharge_patients]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_discharge_patients]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_discharge_patients]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_general_invoice_category]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_general_invoice_category]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_general_invoice_category]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_general_invoice_company]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_general_invoice_company]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_general_invoice_company]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_admitdates]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_GIP_admitdates]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_GIP_admitdates]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_patients]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_GIP_patients]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_GIP_patients]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_breakup]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_GIP_report_breakup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_GIP_report_breakup]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_breakup_CST]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_GIP_report_breakup_CST]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_GIP_report_breakup_CST]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_breakup_RBMED]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_GIP_report_breakup_RBMED]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_GIP_report_breakup_RBMED]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_main]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_GIP_report_main]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_GIP_report_main]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_admitdate]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_ipbill_admitdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_ipbill_admitdate]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_info]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_ipbill_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_ipbill_info]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_ipbservice]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_ipbill_ipbservice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_ipbill_ipbservice]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_items]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_ipbill_items]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_ipbill_items]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_posneg_adj]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_ipbill_posneg_adj]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_ipbill_posneg_adj]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_service_items]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_ipbill_service_items]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_ipbill_service_items]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_services]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_ipbill_services]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_ipbill_services]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_nonpack_admitdate]    Script Date: 02/15/2016 18:00:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ARIPBILLING].[get_nonpack_admitdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ARIPBILLING].[get_nonpack_admitdate]
GO

USE [HIS]
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_breakup_invoice_npd]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[generate_breakup_invoice_npd] 
	@XMLDATA XML
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @COUNTER INT, @RCOUNT INT, @TMP_BILLNO VARCHAR(50), @TMP_CURRENCY VARCHAR(50)
	DECLARE @IPBILLNO TABLE(
		id INT IDENTITY(1,1),
		billno VARCHAR(50)
	)
	
	CREATE TABLE #tmp1( 
		BILLNO VARCHAR(50), SERVICENAME VARCHAR(500), ITEMCODE VARCHAR(500), ITEMNAME VARCHAR(500), 
		PRICE NUMERIC(18,4), QTY INT, DISC NUMERIC(18,4), DEDUC NUMERIC(18,4),
		GROSS NUMERIC(18,4), NET NUMERIC(18,4), MEDICALIDNUMBER VARCHAR(50),
		INVOICENO VARCHAR(50), PIN VARCHAR(50), PTNAME VARCHAR(50), POLICYNO VARCHAR(25),
		ADMITDATE VARCHAR(50), ADMITTIME VARCHAR(20), DISDATE VARCHAR(50), DISTIME VARCHAR(20),
		COMPANY VARCHAR(250), GRADE VARCHAR(20), CADDR VARCHAR(250)
	)
	
	
	SELECT @TMP_CURRENCY = ORA_CURRENCYCODE FROM dbo.OrganisationDetails

	
	INSERT INTO @IPBILLNO (billno)
		SELECT doc.col.value('ipbillno[1]', 'VARCHAR(50)')
		FROM   @XMLDATA.nodes('/DocumentElement/XMLDATA') doc(col)
	
	SELECT @RCOUNT = COUNT(*) FROM @IPBILLNO
	
	SET @COUNTER = 1
	
	WHILE (@COUNTER <= @RCOUNT)
	
		BEGIN
				
				SELECT @TMP_BILLNO = billno FROM @IPBILLNO WHERE ID = @COUNTER
				
				
				INSERT INTO #tmp1(
					BILLNO, SERVICENAME, ITEMCODE, ITEMNAME, 
					PRICE, QTY, DISC, DEDUC ,
					GROSS, NET , MEDICALIDNUMBER,
					INVOICENO, PIN , PTNAME, POLICYNO,
					ADMITDATE , ADMITTIME, DISDATE, DISTIME,
					COMPANY , GRADE, CADDR
				)
				
				
				SELECT PP.BillNo, PP.ServiceName, PP.Code, PP.Descript,
					   PP.Price, pp.Qty, (pp.Disc * pp.Qty), (pp.Deduc * pp.Qty),
					   ( PP.Price * PP.Qty ),
					   ( ( PP.Price * PP.Qty ) - (pp.Disc * pp.Qty) - (pp.Deduc * pp.Qty) ) ,
					   P.MedIDNumber, 'IPCR - ' + CONVERT(VARCHAR(30), P.invoiceno) INVOICENO, 
					 (P.IACode + '.' + RIGHT('0000000000' + CAST(p.RegistrationNo AS VARCHAR(10)), 10)) AS Pin,
					 P.NAME, P.PolicyNo,
					 REPLACE(CONVERT(VARCHAR(50),P.AdmitDateTime , 106),' ','-') AS AdmitDate,
					 LTRIM(RIGHT(CONVERT(CHAR(20), P.AdmitDateTime, 22), 11)) AS AdmTime,
					 REPLACE(CONVERT(VARCHAR(50),P.DischargeDateTime , 106),' ','-') AS DisDate,
					 LTRIM(RIGHT(CONVERT(CHAR(20), P.DischargeDateTime, 22), 11)) AS DisTime,
					 UPPER(P.CompanyCode) + ' - ' + UPPER(P.CompanyName) AS Company,
					 UPPER(P.GradeName) AS Grade,
					 P.CompanyAddress AS CompAddress
				FROM   (SELECT A.billno                                       AS BillNo,
							   A.Serviceid,
							   CASE A.serviceid
								 WHEN 2 THEN 'ROOM AND BOARD'
								 ELSE B.ServiceName
							   END                                            AS ServiceName,
							   CONVERT(VARCHAR(10), A.editorderdatetime, 101) AS OrderDate,
							   A.itemcode                                     AS Code,
							   A.itemname                                     AS Descript,
							   A.editprice                                    AS Price,
							   Count(A.editquantity)                          AS Qty,
							   A.discount                                     AS Disc,
							   A.deductableamount                             AS Deduc
						FROM   dbo.aripbillitemdetail AS A
							   LEFT JOIN dbo.ipbservice AS B
									   ON A.ServiceID = B.id
						WHERE  ( A.serviceid NOT IN ( 1, 5, 21, 22,
													  23, 37, 14 ) )
							   AND ( A.editprice <> 0 )
							   AND ( A.billno = @TMP_BILLNO )
						GROUP  BY A.edititemid,
								  A.serviceid,
								  A.BillNo,
								  B.ServiceName,
								  A.itemcode,
								  CONVERT(VARCHAR(10), A.editorderdatetime, 101),
								  A.itemname,
								  A.editprice,
								  A.discount,
								  A.deductableamount
						UNION ALL
						SELECT BillNo,
							   A.Serviceid,
							   CASE A.ServiceId
								 WHEN 37 THEN 'TAKE HOME MEDICINE'
								 ELSE A.ServiceName
							   END AS ServiceName,
							   OrderDate,
							   Code,
							   Descript,
							   Price,
							   Qty,
							   Disc,
							   Deduc
						FROM   (SELECT A.billno            AS BillNo,
									   A.ServiceId,
									   A.editorderdatetime AS OrderDate,
									   A.itemcode          AS Code,
									   A.itemname          AS Descript,
									   B.ServiceName       AS ServiceName,
									   A.editprice         AS Price,
									   A.editquantity      AS Qty,
									   A.discount          AS Disc,
									   A.deductableamount  AS Deduc
								FROM   dbo.aripbillitemdetail AS A
									   LEFT JOIN dbo.IPBService AS B
											   ON A.ServiceID = B.id
								WHERE  ( A.serviceid IN ( 1, 5, 21, 23, 37 ) )
									   AND ( A.editprice <> 0 )
									   AND ( A.billno = @TMP_BILLNO )
								GROUP  BY A.serviceid,
										  A.serialno,
										  A.editorderdatetime,
										  A.itemcode,
										  A.billno,
										  A.itemname,
										  A.serviceid,
										  B.ServiceName,
										  A.editprice,
										  A.editquantity,
										  A.discount,
										  A.deductableamount) AS A) PP
								LEFT JOIN
			        
					( SELECT ISNULL(PT.MedIDNumber, '') AS MedIDNumber,
						   d.SlNo                     AS invoiceno,
						   PT.IssueAuthorityCode AS IACode,
						   ISNULL(a.FamilyName, '') + ' '
						   + ISNULL(a.FirstName, '') + ' '
						   + ISNULL(a.MiddleName, '') + ' '
						   + ISNULL(a.LastName, '')   AS NAME,
						   h.PolicyNo,
						   a.AdmitDateTime,
						   a.RegistrationNo,
						   h.Code                     AS CompanyCode,
						   s.NAME                     AS GradeName,
						   h.NAME                     AS CompanyName,
						   a.DischargeDateTime,
						   h.Address                  AS CompanyAddress,
						   d.BillNo
					FROM   dbo.ARIPBill AS d
						   INNER JOIN dbo.Patient AS PT
									  INNER JOIN dbo.OldInPatient AS a
											  ON PT.Registrationno = a.RegistrationNo
								   ON d.IPID = a.IPID
						   INNER JOIN dbo.Grade AS s
								   ON d.GradeID = s.ID
						   INNER JOIN dbo.Company AS h
								   ON d.CompanyID = h.ID
			              
						   INNER JOIN dbo.OldInPatient AS k
								   ON a.IPID = k.IPID
					WHERE  ( d.BillNo = @TMP_BILLNO ) ) P ON PP.BillNo = P.billno		  
		  
							ORDER  BY PP.Servicename 
    
			SET @COUNTER = @COUNTER + 1
			
		END
		
		
		SELECT A.*, UPPER(B.BillToWords) BillToWords, 
		REPLACE(CONVERT(VARCHAR(50),GETDATE(), 106),' ','-') AS DateNow,
		LTRIM(RIGHT(CONVERT(CHAR(20), GETDATE(), 22), 11)) AS TimeNow,
		UPPER(@TMP_CURRENCY) BranchCur
		FROM #tmp1 A
		LEFT JOIN ( SELECT billno, dbo.Currency_towords(SUM(NET)) AS BillToWords
			
		FROM #tmp1 GROUP BY billno) B ON A.BILLNO = B.BILLNO
		
		
		
		DROP TABLE #tmp1
	
END
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_main_invoice]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[generate_main_invoice]
	@ispack INT,
	@billno VARCHAR(50)
	--@XMLDATA XML
AS
BEGIN
	SET NOCOUNT ON;
	
	
	CREATE TABLE #tmp1(
		BILLNO VARCHAR(50),
		DEPTNAME VARCHAR(250),
		AMOUNT NUMERIC(18,2),
		DISCOUNT NUMERIC(18,2),
		DEDUCTABLE NUMERIC(18,2),
		MEDICALIDNUMBER VARCHAR(50),
		INVOICENO BIGINT,
		PIN VARCHAR(50),
		PTNAME VARCHAR(50),
		POLICYNO VARCHAR(25),
		ADMITDATE VARCHAR(50),
		ADMITTIME VARCHAR(20),
		DISDATE VARCHAR(50),
		DISTIME VARCHAR(20),
		COMPANY VARCHAR(250),
		GRADE VARCHAR(20),
		CADDR VARCHAR(250)
	)
	
	
	IF @ispack = 1 
	BEGIN
		

     INSERT INTO #tmp1(BILLNO, DEPTNAME,
		AMOUNT, DISCOUNT, DEDUCTABLE, MEDICALIDNUMBER,
		INVOICENO, PIN, PTNAME,
		POLICYNO, ADMITDATE, ADMITTIME,
		DISDATE, DISTIME, COMPANY, GRADE, CADDR)
     
     SELECT A.BillNo, A.DEPTNAME, A.AMOUNT, A.DISCOUNT, A.DEDUCTABLE, 
		 P.MedIDNumber, P.invoiceno, 
		 (P.IACode + '.' + RIGHT('0000000000' + CAST(p.RegistrationNo AS VARCHAR(10)), 10)) AS Pin,
		 P.NAME, P.PolicyNo,
		 REPLACE(CONVERT(VARCHAR(50),P.AdmitDateTime , 106),' ','-') AS AdmitDate,
		 LTRIM(RIGHT(CONVERT(CHAR(20), P.AdmitDateTime, 22), 11)) AS AdmTime,
		 REPLACE(CONVERT(VARCHAR(50),P.DischargeDateTime , 106),' ','-') AS DisDate,
		 LTRIM(RIGHT(CONVERT(CHAR(20), P.DischargeDateTime, 22), 11)) AS DisTime,
		 UPPER(P.CompanyCode) + ' - ' + UPPER(P.CompanyName) as Company,
		 UPPER(P.GradeName) as Grade,
		 P.CompanyAddress As CompAddress
     FROM (
        
         SELECT BillNo,
				DEPTNAME,
                Sum(AMOUNT)                  AS AMOUNT,
                Sum(DISCOUNT)                AS DISCOUNT,
                Sum(DEDUCTABLE)              AS DEDUCTABLE
         FROM   (SELECT CASE SERVICEID
                          WHEN 2 THEN 'ROOM AND BOARD'
                          ELSE B.NAME
                        END                                      AS DEPTNAME,
                        Sum(A.EditPrice * A.EditQuantity)        AS AMOUNT,
                        Sum(A.EditQuantity * A.Discount)         AS DISCOUNT,
                        Sum(A.EditQuantity * A.DeductableAmount) AS DEDUCTABLE,
                        A.billno
                        
                 FROM   dbo.ARIPBillItemDetail AS A
                        INNER JOIN dbo.Department AS b
                                ON A.DepartmentID = b.ID
                 WHERE  ( A.ServiceID NOT IN ( 1, 5, 21, 23,
                                               37, 14 ) )
                        AND ( A.EditPrice <> 0 )
                        AND ( A.BillNo = @billno )
                 GROUP  BY A.ServiceID, A.BillNo,
                           b.NAME) AS C
         GROUP  BY DEPTNAME, BillNo
         
         
        UNION ALL
        
		SELECT 
				A.billno,
				CASE SERVICEID
                 WHEN 37 THEN 'TAKE HOME MEDICINE'
                 ELSE B.NAME
               END                                      AS DEPTNAME,
			   Sum(A.EditPrice * A.EditQuantity)        AS AMOUNT,
               Sum(A.EditQuantity * A.Discount)         AS DISCOUNT,
               Sum(A.EditQuantity * A.DeductableAmount) AS DEDUCTABLE
               
        FROM   dbo.ARIPBillItemDetail AS A
               INNER JOIN dbo.Department AS B
                       ON A.DepartmentID = B.ID
        WHERE  ( A.ServiceID IN ( 1, 5, 21, 23, 37 ) )
               AND ( A.BillNo = @billno )
        GROUP  BY A.ServiceID, A.BillNo,
                  B.NAME 
        ) A
        
        LEFT JOIN
        
        ( SELECT Isnull(PT.MedIDNumber, '') AS MedIDNumber,
               d.SlNo                     AS invoiceno,
               PT.IssueAuthorityCode as IACode,
               Isnull(a.FamilyName, '') + ' '
               + Isnull(a.FirstName, '') + ' '
               + Isnull(a.MiddleName, '') + ' '
               + Isnull(a.LastName, '')   AS NAME,
			   h.PolicyNo,
               a.AdmitDateTime,
               a.RegistrationNo,
               h.Code                     AS CompanyCode,
               s.NAME                     AS GradeName,
               h.NAME                     AS CompanyName,
               a.DischargeDateTime,
               h.Address                  AS CompanyAddress,
               d.BillNo
        FROM   dbo.ARIPBill AS d
               INNER JOIN dbo.Patient AS PT
                          INNER JOIN dbo.OldInPatient AS a
                                  ON PT.Registrationno = a.RegistrationNo
                       ON d.IPID = a.IPID
               INNER JOIN dbo.Grade AS s
                       ON d.GradeID = s.ID
               INNER JOIN dbo.Company AS h
                       ON d.CompanyID = h.ID
              
               INNER JOIN dbo.OldInPatient AS k
                       ON a.IPID = k.IPID
        WHERE  ( d.BillNo = @billno ) ) P ON A.BillNo = P.billno

         
    
     
         
         
        
	END
	
	IF @ispack = 2
	BEGIN
	 RETURN
	END
	
	
	SELECT A.*, UPPER(B.BillToWords) BillToWords FROM #tmp1 A
	LEFT JOIN (
	
	SELECT billno, dbo.Currency_towords(
		SUM(AMOUNT - (DISCOUNT + DEDUCTABLE)) 
     ) as BillToWords
		
     FROM #tmp1 GROUP BY billno) B ON A.BILLNO = B.BILLNO
     
     
    
	
	DROP TABLE #tmp1 
	
END

-- aripbilling.generate_main_invoice 0, '245954'
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_main_invoice_npd]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[generate_main_invoice_npd]
	@XMLDATA XML
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @COUNTER INT, @RCOUNT INT, @TMP_BILLNO VARCHAR(50), @TMP_CURRENCY VARCHAR(20)
	
	select @TMP_CURRENCY = ORA_CURRENCYCODE from dbo.OrganisationDetails

	CREATE TABLE #tmp1(
		RECTYPE INT, RECEIPTNO VARCHAR(50), DEPDATE VARCHAR(50),
		BILLNO VARCHAR(50), DEPTNAME VARCHAR(250), AMOUNT NUMERIC(18,4), DEPAMOUNT NUMERIC(18,4),  DISCOUNT NUMERIC(18,3),
		DEDUCTABLE NUMERIC(18,3), MEDICALIDNUMBER VARCHAR(50),
		INVOICENO VARCHAR(50), PIN VARCHAR(50), PTNAME VARCHAR(50), POLICYNO VARCHAR(25),
		ADMITDATE VARCHAR(50), ADMITTIME VARCHAR(20), DISDATE VARCHAR(50), DISTIME VARCHAR(20),
		COMPANY VARCHAR(250), GRADE VARCHAR(20), CADDR VARCHAR(250)
	)
	
	DECLARE @IPBILLNO TABLE(
		id INT IDENTITY(1,1),
		billno VARCHAR(50)
	)
	
	INSERT INTO @IPBILLNO (billno)
		SELECT doc.col.value('ipbillno[1]', 'VARCHAR(50)')
		FROM   @XMLDATA.nodes('/DocumentElement/XMLDATA') doc(col)
	
	
	SELECT @RCOUNT = COUNT(*) FROM @IPBILLNO
	
	SET @COUNTER = 1
	
	WHILE (@COUNTER <= @RCOUNT)
	
	BEGIN
	
		SELECT @TMP_BILLNO = billno FROM @IPBILLNO where ID = @COUNTER
	
		
		INSERT INTO #tmp1(RECTYPE, RECEIPTNO, DEPDATE, BILLNO, DEPTNAME,
			AMOUNT, DEPAMOUNT, DISCOUNT, DEDUCTABLE, MEDICALIDNUMBER,
			INVOICENO, PIN, PTNAME,
			POLICYNO, ADMITDATE, ADMITTIME,
			DISDATE, DISTIME, COMPANY, GRADE, CADDR
		 )
			 SELECT A.RECTYPE, A.ReceiptNo, A.DepDate, A.BillNo, A.DEPTNAME, A.AMOUNT, A.DEPAMOUNT, A.DISCOUNT, A.DEDUCTABLE, 
				 P.MedIDNumber, 'IPCR - ' + CONVERT(VARCHAR(30), P.invoiceno), 
				 (P.IACode + '.' + RIGHT('0000000000' + CAST(p.RegistrationNo AS VARCHAR(10)), 10)) AS Pin,
				 P.NAME, P.PolicyNo,
				 REPLACE(CONVERT(VARCHAR(50),P.AdmitDateTime , 106),' ','-') AS AdmitDate,
				 LTRIM(RIGHT(CONVERT(CHAR(20), P.AdmitDateTime, 22), 11)) AS AdmTime,
				 REPLACE(CONVERT(VARCHAR(50),P.DischargeDateTime , 106),' ','-') AS DisDate,
				 LTRIM(RIGHT(CONVERT(CHAR(20), P.DischargeDateTime, 22), 11)) AS DisTime,
				 UPPER(P.CompanyCode) + ' - ' + UPPER(P.CompanyName) as Company,
				 UPPER(P.GradeName) as Grade,
				 P.CompanyAddress As CompAddress
			 FROM (
		        
				 SELECT 1 as RECTYPE,
						'' as ReceiptNo,
						'' as DepDate,
						BillNo,
						DEPTNAME,
						Sum(AMOUNT)                  AS AMOUNT,
						0 as DEPAMOUNT,
						Sum(DISCOUNT)                AS DISCOUNT,
						Sum(DEDUCTABLE)              AS DEDUCTABLE
				 FROM   (SELECT CASE SERVICEID
								  WHEN 2 THEN 'ROOM AND BOARD'
								  ELSE B.ServiceName
								END                                      AS DEPTNAME,
								Sum(A.EditPrice * A.EditQuantity)        AS AMOUNT,
								Sum(A.EditQuantity * A.Discount)         AS DISCOUNT,
								Sum(A.EditQuantity * A.DeductableAmount) AS DEDUCTABLE,
								A.billno
		                        
						 FROM   dbo.ARIPBillItemDetail AS A
								INNER JOIN dbo.IPBService AS b
										ON A.ServiceID = b.ID
						 WHERE  ( A.ServiceID NOT IN ( 1, 5, 21, 23, 37, 14 ) )
								AND ( A.EditPrice <> 0 )
								AND ( A.BillNo = @TMP_BILLNO )
						 GROUP  BY A.ServiceID, A.BillNo, b.ServiceName) AS C
				 GROUP  BY DEPTNAME, BillNo
				 
			UNION ALL
		        
				SELECT 1 as RECTYPE,
						'' as ReceiptNo,
						'' as DepDate,
						A.Billno,
						CASE SERVICEID
						 WHEN 37 THEN 'TAKE HOME MEDICINE'
						 ELSE B.ServiceName
					   END                                      AS DEPTNAME,
					   Sum(A.EditPrice * A.EditQuantity)        AS AMOUNT,
					   0 as DEPAMOUNT,
					   Sum(A.EditQuantity * A.Discount)         AS DISCOUNT,
					   Sum(A.EditQuantity * A.DeductableAmount) AS DEDUCTABLE
		               
				FROM   dbo.ARIPBillItemDetail AS A
					   INNER JOIN dbo.IPBService AS B
							   ON A.ServiceID = B.ID
				WHERE  ( A.ServiceID IN ( 1, 5, 21, 23, 37 ) )
					   AND ( A.BillNo = @TMP_BILLNO )
				GROUP  BY A.ServiceID, A.BillNo,
						  B.ServiceName 
			-- UNION THE DEPOSIT HERE
			
			UNION ALL			  
					select 
						2 as RECTYPE,
						'T' + CONVERT(VARCHAR(50), ReceiptNo) as ReceiptNo,
						REPLACE(CONVERT(VARCHAR(50),DateTime , 106),' ','-') as DepDate,
						@TMP_BILLNO as billNo,
						'' as DEPTNAME,
						0 as AMOUNT,
						(CASE WHEN TYPE <> 3 THEN Amount 
						ELSE 
							(Amount * -1) 
						
						END)as DEPAMOUNT,
						0 as DISCOUNT,
						0 as DEDUCTABLE
					from dbo.IPtransactions where ipid IN(Select ipid from aripbill where billno = @TMP_BILLNO)	  
					
				) A
		        
				LEFT JOIN
		        
				( SELECT Isnull(PT.MedIDNumber, '') AS MedIDNumber,
					   d.SlNo                     AS invoiceno,
					   PT.IssueAuthorityCode as IACode,
					   Isnull(a.FamilyName, '') + ' '
					   + Isnull(a.FirstName, '') + ' '
					   + Isnull(a.MiddleName, '') + ' '
					   + Isnull(a.LastName, '')   AS NAME,
					   h.PolicyNo,
					   a.AdmitDateTime,
					   a.RegistrationNo,
					   h.Code                     AS CompanyCode,
					   s.NAME                     AS GradeName,
					   h.NAME                     AS CompanyName,
					   a.DischargeDateTime,
					   h.Address                  AS CompanyAddress,
					   d.BillNo
				FROM   dbo.ARIPBill AS d
					   INNER JOIN dbo.Patient AS PT
								  INNER JOIN dbo.OldInPatient AS a
										  ON PT.Registrationno = a.RegistrationNo
							   ON d.IPID = a.IPID
					   INNER JOIN dbo.Grade AS s
							   ON d.GradeID = s.ID
					   INNER JOIN dbo.Company AS h
							   ON d.CompanyID = h.ID
		              
					   INNER JOIN dbo.OldInPatient AS k
							   ON a.IPID = k.IPID
				WHERE  ( d.BillNo = @TMP_BILLNO ) ) P ON A.BillNo = P.billno
        
        SET @COUNTER = @COUNTER + 1

	END

	
	SELECT A.*, UPPER(B.BillToWords) BillToWords, 
		REPLACE(CONVERT(VARCHAR(50),GETDATE(), 106),' ','-') AS DateNow,
		LTRIM(RIGHT(CONVERT(CHAR(20), GETDATE(), 22), 11)) AS TimeNow,
		UPPER(@TMP_CURRENCY) BranchCur
	FROM #tmp1 A
	LEFT JOIN (
	
	SELECT billno, dbo.Currency_towords(
		 SUM((AMOUNT) - (DISCOUNT + DEDUCTABLE))
     ) as BillToWords
		
     FROM #tmp1 WHERE RECTYPE = 1 GROUP BY billno) B ON A.BILLNO = B.BILLNO
    
     ORDER BY A.DEPTNAME


	DROP TABLE #tmp1 
	
END

-- aripbilling.generate_main_invoice 0, '245954'
GO

/****** Object:  StoredProcedure [ARIPBILLING].[generate_main_invoice_pd]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[generate_main_invoice_pd]
	@XMLDATA XML
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @COUNTER INT, @RCOUNT INT, @TMP_BILLNO VARCHAR(50), @TMP_CURRENCY VARCHAR(50)
	DECLARE @IPBILLNO TABLE(
		id INT IDENTITY(1,1),
		billno VARCHAR(50)
	)
	
	SELECT @TMP_CURRENCY = ORA_CURRENCYCODE FROM dbo.OrganisationDetails
	
	CREATE TABLE #tmp1(
		GTYPE VARCHAR(50), NAME VARCHAR(500), GROSS NUMERIC(18,2), NET NUMERIC(18,2),
		BILLNO VARCHAR(50), 
		MEDICALIDNUMBER VARCHAR(50),
		INVOICENO VARCHAR(50), PIN VARCHAR(50), PTNAME VARCHAR(50), POLICYNO VARCHAR(25),
		ADMITDATE VARCHAR(50), ADMITTIME VARCHAR(20), DISDATE VARCHAR(50), DISTIME VARCHAR(20),
		COMPANY VARCHAR(250), GRADE VARCHAR(20), CADDR VARCHAR(250)
	)

	INSERT INTO @IPBILLNO (billno)
		SELECT doc.col.value('ipbillno[1]', 'VARCHAR(50)')
		FROM   @XMLDATA.nodes('/DocumentElement/XMLDATA') doc(col)
	
	SELECT @RCOUNT = COUNT(*) FROM @IPBILLNO
	
	SET @COUNTER = 1
	
	
	WHILE (@COUNTER <= @RCOUNT)
	BEGIN
	
		SELECT @TMP_BILLNO = billno FROM @IPBILLNO WHERE ID = @COUNTER
		
		INSERT INTO #tmp1(
			GTYPE, NAME, GROSS, NET, BILLNO, 
			MEDICALIDNUMBER, INVOICENO, PIN, PTNAME, POLICYNO,
			ADMITDATE, ADMITTIME, DISDATE, DISTIME, COMPANY, GRADE, CADDR
		)
		SELECT PP.GTYPE, 
			PP.NAME, PP.GROSS, PP.NET, PP.BILLNO,
			P.MedIDNumber, 'IPCR - ' + CONVERT(VARCHAR(30), P.invoiceno) INVOICENO, 
			 (P.IACode + '.' + RIGHT('0000000000' + CAST(p.RegistrationNo AS VARCHAR(10)), 10)) AS Pin,
			 P.NAME, P.PolicyNo,
			 REPLACE(CONVERT(VARCHAR(50),P.AdmitDateTime , 106),' ','-') AS AdmitDate,
			 LTRIM(RIGHT(CONVERT(CHAR(20), P.AdmitDateTime, 22), 11)) AS AdmTime,
			 REPLACE(CONVERT(VARCHAR(50),P.DischargeDateTime , 106),' ','-') AS DisDate,
			 LTRIM(RIGHT(CONVERT(CHAR(20), P.DischargeDateTime, 22), 11)) AS DisTime,
			 UPPER(P.CompanyCode) + ' - ' + UPPER(P.CompanyName) AS Company,
			 UPPER(P.GradeName) AS Grade,
			 P.CompanyAddress AS CompAddress
		FROM (
			SELECT * FROM (	
					SELECT 'PD'	   AS GTYPE,
						   B.PACKAGENAME       AS NAME,
						   B.EditPackageAmount AS GROSS,
						   0 AS NET,
						   B.BillNo AS BILLNO
					FROM   dbo.ARPackageBill AS B
						   INNER JOIN dbo.ARIPBillItemDetail AS A
								   ON B.PackageItemID = A.EditItemID
									  AND B.PackageOrderID = A.OrderID
					WHERE  ( B.PackageAmount = 0 )
						   AND ( B.BillNo = @TMP_BILLNO )
						   AND ( A.BillNo = @TMP_BILLNO )
					UNION ALL
					SELECT 'PD'	   AS GTYPE,
						   B.PACKAGENAME       AS NAME,
						   B.EditPackageAmount AS GROSS,
						   0 AS NET,
						   B.BillNo AS BILLNO
					FROM   dbo.ARPackageBill AS B
						   INNER JOIN dbo.ARIPBillItemDetail AS A
								   ON B.PackageItemID = A.EditItemID
									  AND B.PackageOrderID = A.OrderID
					WHERE  ( B.PackageAmount <> 0 )
						   AND ( B.BillNo = @TMP_BILLNO )
						   AND ( A.BillNo = @TMP_BILLNO )
				) P

				UNION ALL           
				         
				SELECT 'OTHERS' GTYPE, 
					   A.ITEMCODE + ' - ' + A.ITEMNAME   AS NAME,
					   SUM(EDITQUANTITY * EDITPRICE)                                                                             AS GROSS,
					   SUM(( EDITQUANTITY * EDITPRICE ) - ( ( EDITQUANTITY * DEDUCTABLEAMOUNT ) + ( EDITQUANTITY * Discount ) )) AS NET,
					   A.BillNo AS BILLNO
				FROM   dbo.ARIPBILLITEMDETAIL A
				WHERE  A.SERVICEID IN( 22 )
					   AND SERIALNO NOT IN( SELECT B.SerialNo
							FROM   dbo.ARPackageBill AS A
								   INNER JOIN dbo.ARIPBillItemDetail AS B
										   ON A.PackageItemID = B.EditItemID
											  AND A.PackageServiceID = B.ServiceID
											  AND A.PackageOrderID = B.OrderID
											  AND A.BillNo = B.BillNo
							WHERE  ( A.BillNo = @TMP_BILLNO ))
					   AND SERIALNO NOT IN (0) AND A.BILLNO = @TMP_BILLNO
				GROUP  BY A.ITEMNAME,
						  A.ITEMCODE, A.BillNo
				UNION ALL
				SELECT 'OTHERS' GTYPE, 
					   ISNULL(CODE + ' - ', ' ') +  DESCRIPTION AS NAME,
					   AMOUNT  GROSS,
					   AMOUNT  NET,
					   BillNo AS BILLNO
				FROM   dbo.NPDAMOUNT
				WHERE  BILLNO = @TMP_BILLNO
				) PP
				LEFT JOIN
			        
					( SELECT ISNULL(PT.MedIDNumber, '') AS MedIDNumber,
						   d.SlNo                     AS invoiceno,
						   PT.IssueAuthorityCode AS IACode,
						   ISNULL(a.FamilyName, '') + ' '
						   + ISNULL(a.FirstName, '') + ' '
						   + ISNULL(a.MiddleName, '') + ' '
						   + ISNULL(a.LastName, '')   AS NAME,
						   h.PolicyNo,
						   a.AdmitDateTime,
						   a.RegistrationNo,
						   h.Code                     AS CompanyCode,
						   s.NAME                     AS GradeName,
						   h.NAME                     AS CompanyName,
						   a.DischargeDateTime,
						   h.Address                  AS CompanyAddress,
						   d.BillNo
					FROM   dbo.ARIPBill AS d
						   INNER JOIN dbo.Patient AS PT
									  INNER JOIN dbo.OldInPatient AS a
											  ON PT.Registrationno = a.RegistrationNo
								   ON d.IPID = a.IPID
						   INNER JOIN dbo.Grade AS s
								   ON d.GradeID = s.ID
						   INNER JOIN dbo.Company AS h
								   ON d.CompanyID = h.ID
			              
						   INNER JOIN dbo.OldInPatient AS k
								   ON a.IPID = k.IPID
					WHERE  ( d.BillNo = @TMP_BILLNO ) ) P ON PP.BillNo = P.billno
		
		SET @COUNTER = @COUNTER + 1	
    END

    
    SELECT A.*, UPPER(B.BillToWords) BillToWords, 
		REPLACE(CONVERT(VARCHAR(50),GETDATE(), 106),' ','-') AS DateNow,
		LTRIM(RIGHT(CONVERT(CHAR(20), GETDATE(), 22), 11)) AS TimeNow,
		UPPER(@TMP_CURRENCY) BranchCur
	FROM #tmp1 A
	LEFT JOIN ( SELECT billno, dbo.Currency_towords(SUM(NET)) AS BillToWords
		
     FROM #tmp1 GROUP BY billno) B ON A.BILLNO = B.BILLNO

    
    
    
    DROP TABLE #tmp1
    
END
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_account_list]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_account_list]
	@rtype INT,
	@catid INT,
	@fdate DATETIME,
	@tdate DATETIME
	
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @rtype = 1
	BEGIN
		SELECT DISTINCT a.Id,
					 UPPER(a.Code) + ' - ' + a.NAME AS Name
		FROM   dbo.Category AS a
			   INNER JOIN dbo.ARIPBill AS b
					   ON a.ID = b.CategoryID
		WHERE  ( b.InvoiceDateTime >= @fdate )
			   AND ( b.InvoiceDateTime < DATEADD(d,1,@tdate) )
		ORDER  BY Name 
    END
	ELSE IF @rtype = 2
	BEGIN
		SELECT DISTINCT a.Id,
                        UPPER(a.Code) + ' - ' + a.NAME AS Name
        FROM   dbo.Company AS a
               INNER JOIN dbo.ARIPBill AS b
                       ON a.ID = b.CompanyID
        WHERE  ( a.CategoryID = @catid )
               AND ( b.InvoiceDateTime >= @fdate )
			   AND ( b.InvoiceDateTime < DATEADD(d,1,@tdate) )
        ORDER  BY Name 
        
	END
END

-- aripbilling.get_account_list 2, 
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_bill_list_npd]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_bill_list_npd] 
	@ispack INT,
	@billno VARCHAR(50)
AS
  BEGIN
      SET NOCOUNT ON;

	IF @ispack = 1
	BEGIn

      SELECT A.BillNo,
             LTRIM(RTRIM(b.issueauthoritycode + '.' +
                         + REPLICATE('0', 10 - LEN(CAST(b.RegistrationNo AS CHAR(10))))
                         + CAST(b.RegistrationNo AS CHAR(10)))) AS PIN,
             CONVERT(VARCHAR, b.AdmitDateTime, 106)             AS AdmitDate,
             UPPER(CO.Code) + ' - ' + UPPER(CO.Name) AS Company
      FROM   dbo.OldInPatientDetails AS C
             INNER JOIN dbo.OldInPatient AS b
                     ON C.IPID = b.IPID
             INNER JOIN dbo.ARIPBill AS A
                     ON b.IPID = A.IPID
             INNER JOIN dbo.oldinpatient OI
                     ON A.IPID = OI.IPID
             LEFT JOIN dbo.company CO ON A.CompanyID = CO.ID
      WHERE  ( A.BillNo NOT IN (SELECT BillNo
                                FROM   dbo.ARPackageBill) )
             AND ( A.BillNo = @billno )
      ORDER  BY b.RegistrationNo
      
    END
    
    IF @ispack = 2
    BEGIN
		SELECT A.BillNo,
             LTRIM(RTRIM(b.issueauthoritycode + '.' +
                         + REPLICATE('0', 10 - LEN(CAST(b.RegistrationNo AS CHAR(10))))
                         + CAST(b.RegistrationNo AS CHAR(10)))) AS PIN,
             CONVERT(VARCHAR, b.AdmitDateTime, 106)             AS AdmitDate,
             UPPER(CO.Code) + ' - ' + UPPER(CO.Name) AS Company
        FROM   dbo.OldInPatientDetails AS C
               INNER JOIN dbo.OldInPatient AS b
                       ON C.IPID = b.IPID
               INNER JOIN dbo.ARIPBill AS a
                       ON b.IPID = a.IPID
               LEFT JOIN dbo.company CO ON A.CompanyID = CO.ID
        WHERE  ( a.BillNo IN (SELECT BillNo
                              FROM   ARPackageBill) )
               AND ( a.BillNo = @billno )
        ORDER  BY b.RegistrationNo 
        
    END
  END 

-- aripbilling.get_bill_list_npd 0, 245954
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_billlist_npd_batch]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_billlist_npd_batch]
	@ispack INT,
	@catid INT,
	@comid BIGINT,
	@fdate DATETIME,
	@tdate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @ispack = 1
	BEGIN
		SELECT A.BillNo,
             LTRIM(RTRIM(b.issueauthoritycode + '.' +
                         + REPLICATE('0', 10 - LEN(CAST(b.RegistrationNo AS CHAR(10))))
                         + CAST(b.RegistrationNo AS CHAR(10)))) AS PIN,
             CONVERT(VARCHAR, b.AdmitDateTime, 106)             AS AdmitDate,
             UPPER(CO.Code) + ' - ' + UPPER(CO.Name) AS Company
        FROM   Company AS co
               INNER JOIN ARIPBill AS A
                       ON co.ID = A.CompanyID
               INNER JOIN OldInPatientDetails AS C
                          INNER JOIN OldInPatient AS b
                                  ON C.IPID = b.IPID
                       ON A.IPID = b.IPID
        WHERE  ( A.BillNo NOT IN (SELECT BillNo
                                  FROM   ARPackageBill) )
               AND ( A.CategoryID = @catid )
               AND (@comid = 0 OR A.CompanyID = @comid)
               AND ( A.InvoiceDateTime >= @fdate )
               AND ( A.InvoiceDateTime < DATEADD(d, 1,@tdate)  )
        ORDER  BY co.Code,
                  b.AdmitDateTime,
                  b.RegistrationNo 
	END
	ELSE IF @ispack = 2
	BEGIN
		SELECT A.BillNo,
             LTRIM(RTRIM(b.issueauthoritycode + '.' +
                         + REPLICATE('0', 10 - LEN(CAST(b.RegistrationNo AS CHAR(10))))
                         + CAST(b.RegistrationNo AS CHAR(10)))) AS PIN,
             CONVERT(VARCHAR, b.AdmitDateTime, 106)             AS AdmitDate,
             UPPER(CO.Code) + ' - ' + UPPER(CO.Name) AS Company
        FROM   ARIPBill AS a
               INNER JOIN OldInPatient AS b
                       ON a.IPID = b.IPID
               INNER JOIN OldInPatientDetails AS C
                       ON C.IPID = b.IPID
               INNER JOIN Company AS co
                       ON a.CompanyID = co.ID
               INNER JOIN ARPackageBill AS A1
                       ON a.BillNo = A1.BillNo
        WHERE  ( a.CategoryID = @catid ) AND (@comid = 0 or A.CompanyID = @comid) 
               AND ( a.InvoiceDateTime >= @fdate AND a.InvoiceDateTime < DATEADD(d,1,@tdate ))
               AND ( a.BillNo NOT IN (SELECT BillNo
                                      FROM   ARIPBillItemDetail
                                      WHERE  ( EditOrderDateTime >= @fdate AND EditOrderDateTime < DATEADD(d,1,@tdate) )
                                      GROUP  BY BillNo
                                      HAVING ( Sum(EditPrice * Quantity) <= 0.1 )
                                             AND ( Sum(EditPrice * Quantity) > 0 )) )
        ORDER  BY co.Code,
                  b.AdmitDateTime,
                  b.RegistrationNo 
        
	END
END
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_discharge_patients]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_discharge_patients]
	@catid INT,
	@comid INT,
	@withcash INT,
	@fdate DATETIME,
	@tdate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	-- DISLIST
	CREATE TABLE #DISLIST ( IPID INT , PIN VARCHAR(50) , PTName VARCHAR(100) 
		, Bed CHAR(10) , AdmitDate VARCHAR(50)
		, DisDate VARCHAR(50) , Doctor VARCHAR(10) 
		, CompanyCode VARCHAR(10) , CompanyName VARCHAR(100) , PDStat CHAR(2)
	)
	CREATE TABLE #GETTOTAL ( IPID INT , PIN VARCHAR(50) 
		, PTName VARCHAR(100) , Bed CHAR(10) , AdmitDate VARCHAR(50)
		, DisDate VARCHAR(50) , Doctor VARCHAR(10) , CompanyCode VARCHAR(10) 
		, CompanyName VARCHAR(100) , PDStat CHAR(2)
		, NOD INT , IPBillBillNo VARCHAR(50) , PackBillNo VARCHAR(50)
		, TotalCharges Decimal(18,2) , EligiblePackageAmount Decimal(18,2)
	)
	-- 
	INSERT INTO #DISLIST
		SELECT 
		   a.IPID											   AS IPID, 
		   (a.IssueAuthorityCode + '.' + RIGHT('0000000000' + 
		   CAST(a.RegistrationNo AS VARCHAR(10)), 10))         AS PIN,
		   CAST(a.familyname + ',' + a.firstname + ' ' 
		   + a.middlename + ' ' + a.lastname AS CHAR(30))	   AS PTName,
		   CAST(b.NAME AS CHAR(7))                             AS Bed, 
		   REPLACE(CONVERT (VARCHAR(50), 
		   a.admitdatetime, 106),' ','-')					   AS AdmitDate,
		   REPLACE(CONVERT (VARCHAR(50), 
		   a.dischargedatetime, 106),' ','-')				   AS DisDate,
		   e.empcode                                           AS Doctor, 
		   co.Code                                             AS CompanyCode, 
		   co.Name                                             AS CompanyName,
		   CASE ISNULL(c.ipid, 0) 
			 WHEN 0 THEN '' 
			 ELSE 'PD' 
		   END                                                 AS PDStat
	   FROM   dbo.OldInPatient AS a 
			  INNER JOIN dbo.Company AS co ON a.CompanyID = co.ID 
			  INNER JOIN dbo.BedTransfers AS bt ON a.IPID = bt.ipid AND a.DischargeDateTime = bt.todatetime 
			  INNER JOIN dbo.Bed AS b ON bt.bedid = b.ID 
			  INNER JOIN dbo.Station AS s ON b.StationID = s.ID 
			  INNER JOIN dbo.Employee AS e ON a.DoctorID = e.ID 
			  LEFT OUTER JOIN dbo.IPPackage AS c ON a.IPID = c.Ipid
		WHERE  (@withcash = 1 OR a.CategoryID NOT IN(1))
			   AND (a.dischargedatetime >= @fdate AND a.dischargedatetime < (@tdate + '23:59:59'))
			   AND ( a.admitdatetime >= '01-Sep-06' ) -- fix september 01, 2006
			   AND (@catid = 0 OR a.CategoryID = @catid)
			   AND (@comid = 0 OR a.CompanyID = @comid)
		
		INSERT INTO #GETTOTAL
			SELECT A.IPID, A.PIN, 
			A.PTName, A.Bed, A.AdmitDate, A.DisDate, A.Doctor, A.CompanyCode, A.CompanyName
			,A.PDStat
			,DATEDIFF(D,a.AdmitDate,a.DisDate) as NOD
			,C.BillNo as IPBillBillNo
			,ISNULL(B.BillNo,0) as PackBillNo
			,Sum(D.Price * D.Quantity) as TotalCharges
			,ISNULL(B.EligiblePackageAmount,0) as PkgAmount
			FROM #DISLIST A
				LEFT OUTER JOIN dbo.PackageBill B ON A.IPID = B.IPID AND B.Cash = 2
				LEFT OUTER JOIN dbo.IPBill C ON A.IPID = C.IPID AND C.Cash = 2
				LEFT OUTER JOIN dbo.IPBillItemDetail D ON C.BillNo = D.BillNo
			GROUP BY
				A.PIN, A.PTName, A.PDStat, A.IPID, A.Doctor, A.DisDate,A.CompanyName, A.CompanyCode, A.Bed, A.AdmitDate
				, B.BillNo
				, C.BillNo
				, B.EligiblePackageAmount 
		--select * from #DISLIST
		--select * from #GETTOTAL
		SELECT distinct ISNULL(E.BillNO, '') as BillNO
			, A.IPID
			, A.PIN
			, A.CompanyCode
			, A.CompanyName
			, REPLACE(CONVERT(varchar(50), ISNULL(A.AdmitDate, GETDATE()), 106),' ','-') as AdmitDate
			, A.Bed
			, A.Doctor
			, REPLACE(CONVERT(varchar(50), ISNULL(A.DisDate, GETDATE()), 106),' ','-') as DisDate
			, A.NOD
			, ISNULL(A.IPBillBillNo, '') as IPBillBillNo
			, A.PDStat
			, A.PTName
			, A.PackBillNo
			,CASE 
				WHEN A.PackBillNo = 0
					THEN ISNULL(A.TotalCharges, 0)
				ELSE ISNULL(Sum(E.Price * E.Quantity), 0)
			END TotalCharges
			, A.EligiblePackageAmount as PkgAmount
			, F.Name AS BranchName
			, F.City AS Branch
			, F.District + ', ' + '' + F.City + ' ' + F.Country AS BranchAddress
		FROM #GETTOTAL A
			LEFT JOIN dbo.PackageBillItemDetail E ON A.PackBillNo = E.BillNO
			LEFT JOIN dbo.OrganisationDetails F ON F.Id > 0
		GROUP BY
			A.AdmitDate, A.Bed, A.CompanyCode, A.CompanyName, A.DisDate
			, A.Doctor, A.IPBillBillNo, A.IPID, A.NOD, A.PDStat, A.PIN, A.PTName
			, A.PackBillNo, A.TotalCharges, A.EligiblePackageAmount
			, E.BillNO
			, F.Name
			, F.City
			, F.District + ', ' + '' + F.City + ' ' + F.Country 
		ORDER BY A.CompanyName
		
		DROP TABLE #DISLIST
		DROP TABLE #GETTOTAL
		
END

--[WITH CASH]		 [ARIPBILLING].[get_discharge_patients] 1,0,1,'01-jun-2015','01-jun-2015'
--[WITHOUT CASH]     [ARIPBILLING].[get_discharge_patients] 11,0,1,'01-jun-2015','30-jun-2015'

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_general_invoice_category]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_general_invoice_category] --'01-Jun-2015', '30-Jun-2015'
	@fdate DATETIME,
	@tdate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT a.Id, a.Code + ' - ' + a.Name AS Name 
	FROM Category a,ARIPBill b 
	WHERE a.id=b.CategoryId AND b.InvoiceDateTime >= (@fdate + ' ' + LTRIM(RIGHT(CONVERT(CHAR(20), GETDATE(), 22), 11)))
		AND b.InvoiceDateTime < dateadd(d,1,@tdate)
	ORDER BY Name 

END


GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_general_invoice_company]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_general_invoice_company]-- 11, '01-Jun-2015', '30-Jun-2015'
	@catid INT,
	@fdate DATETIME,
	@tdate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	Select Distinct a.Id, a.Code + ' - ' + a.Name as Name
	from Company a ,ARIPBill b 
	where a.Id=b.CompanyId and a.CategoryId=@catid  
		and b.InvoiceDateTime >= (@fdate + ' ' + LTRIM(RIGHT(CONVERT(CHAR(20), GETDATE(), 22), 11))) 
		and b.InvoiceDateTime < DATEADD(d,1,@tdate) 
	Order by Name 

END


GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_admitdates]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_GIP_admitdates] --'1432216'
	@pin BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
	d.id as Catid
	,d.Code + ' - ' + d.Name as Category
	,c.ID as Comid
	,c.Code
	,c.Code + ' - ' + c.Name as Company
	,(a.issueauthoritycode + '.' + RIGHT('0000000000' + CAST(a.RegistrationNo AS VARCHAR(10)), 10)) AS Pin,
	REPLACE(CONVERT(VARCHAR(50), a.AdmitDateTime, 106),' ','-') + ' ' 
		+ LTRIM(RIGHT(CONVERT(CHAR(20), a.AdmitDateTime, 22), 11)) AS AdmitDateTime,
	a.IPID, b.BillNo 
	FROM dbo.OldInPatient a
		INNER JOIN dbo.ARIPBill B ON a.IPID = b.IPID
		LEFT JOIN dbo.Company c ON c.ID = b.companyid
		LEFT JOIN dbo.category d ON d.ID = b.CategoryID
	WHERE  a.Billtype = 2 AND a.RegistrationNo = @pin 
	ORDER BY a.AdmitDateTime DESC, a.IPID DESC
END
--aripbilling.get_GIP_admitdates 1432216
--select top 100 * from oldinpatient where billtype = 1
--

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_patients]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_GIP_patients] --11,0,'01-Jun-2015', '30-Jun-2015'
	@catid INT,
	@comid INT,
	@fdate datetime,
	@tdate datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	  --Isnull(b.medidnumber, '')                      AS MedIDNumber, 
       --C.letterno, 
       --''                                                 AS PPNNO, 
       --''                                                 AS CCNO, 
       --''                                                 AS RELATION, 
       A.BillNo, 
       A.IPID, 
       (b.IssueAuthorityCode + '.' + RIGHT('0000000000' + CAST(b.RegistrationNo AS VARCHAR(10)), 10)) AS Pin,
       'ADMISSION DATE: ' + REPLACE(CONVERT(VARCHAR(50), b.AdmitDateTime, 106),' ','-') + ' ' 
		+ LTRIM(RIGHT(CONVERT(CHAR(20), b.AdmitDateTime, 22), 11)) AS AdmitDate
	FROM dbo.ARIPBill A
		INNER JOIN dbo.Oldinpatient B ON A.ipid = B.ipid
		LEFT JOIN dbo.OldInPatientDetails C ON C.IPID = B.IPID
		LEFT JOIN dbo.Company co ON co.id = A.companyid
		
	
	--dbo.company AS co 
	--	   INNER JOIN dbo.aripbill AS A ON co.id = A.companyid 
	--	   INNER JOIN dbo.oldinpatientdetails AS C 
	--	   INNER JOIN dbo.oldinpatient AS b ON C.ipid = b.ipid ON A.ipid = b.ipid 
	WHERE  ( A.billno NOT IN (SELECT billno FROM dbo.arpackagebill) ) 
		   AND A.categoryid = @catid AND (@comid = 0 OR A.CompanyID = @comid) 
		   AND ( A.invoicedatetime >= @fdate + ' ' + Ltrim(RIGHT(CONVERT(CHAR(20), Getdate(), 22), 11))) 
		   AND ( A.invoicedatetime < Dateadd(d, 1, @tdate) ) 
	ORDER  BY b.registrationno 
END

--[ARIPBILLING].[get_GIP_patients] 11,0,'01-Jun-2015', '30-Jun-2015'

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_breakup]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_GIP_report_breakup]
	@billno BIGINT,
	@ispack INT
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @ispack = 0
      BEGIN ;
          WITH cte_PTINFO
               AS (SELECT d.BillNo,
                          ( 'IPCR-' + CONVERT(VARCHAR(100), d.SlNo) )                           AS InvoiceNo,
                          ( a.issueauthoritycode + '.'
                            + RIGHT('0000000000' + Cast(a.RegistrationNo AS VARCHAR(10)), 10) ) AS Pin,
                          Replace(CONVERT(VARCHAR(50), a.AdmitDateTime, 106), ' ', '-')         AS AdmitDate,
                          Isnull(a.FamilyName, '') + ' '
                          + Isnull(a.FirstName, '') + ' '
                          + Isnull(a.MiddleName, '') + ' '
                          + Isnull(a.LastName, '')                                              AS PTName,
                          Ltrim(RIGHT(CONVERT(CHAR(20), a.AdmitDateTime, 22), 11))              AS AdmTime,
                          h.Address                                                             AS CompAddress,
                          Replace(CONVERT(VARCHAR(50), a.DischargeDateTime, 106), ' ', '-')     AS DisDate,
                          h.NAME                                                                AS CompName,
                          h.Code                                                                AS CompCode,
                          Ltrim(RIGHT(CONVERT(CHAR(20), a.DischargeDateTime, 22), 11))          AS DisTime,
                          s.NAME                                                                AS GradeName,
                          Isnull(PT.MedIDNumber, '')                                            AS MedIDNumber
                   FROM   dbo.ARIPBill AS d
                          INNER JOIN dbo.Category AS b
                                  ON d.CategoryID = b.ID
                          INNER JOIN dbo.Patient AS PT
                                     INNER JOIN dbo.OldInPatient AS a
                                             ON PT.Registrationno = a.RegistrationNo
                                  ON d.IPID = a.IPID
                          INNER JOIN dbo.Grade AS s
                                  ON d.GradeID = s.ID
                          INNER JOIN dbo.Company AS h
                                  ON d.CompanyID = h.ID
                   WHERE  ( d.BillNo = @billno )),
               cte_PTDISCOUNT
               AS (SELECT C.DepartmentName                                AS DisDeptName,
                          Round(Sum(C.DISCOUNT) / Sum(C.AMOUNT), 2) * 100 AS DiscPer,
                          Sum(C.discount)                                 AS DiscAmt
                   FROM   (SELECT CASE A.serviceid
                                    WHEN 2 THEN 'ROOM AND BOARD'
                                    WHEN 37 THEN 'TAKE HOME MEDICINE'
                                    ELSE B.NAME
                                  END                               AS DepartmentName,
                                  --Sum(A.editquantity * A.discount / A.editprice * A.editquantity) * 
                                  --100 AS  DISCOUNTPERCENTAGE, 
                                  Sum(A.editprice * A.editquantity) AS AMOUNT,
                                  Sum(A.editquantity * A.discount)  AS DISCOUNT
                           FROM   DBO.aripbillitemdetail AS A
                                  INNER JOIN DBO.department AS b
                                          ON A.departmentid = b.id
                           WHERE  ( A.editprice <> 0 )
                                  AND ( A.edititemid <> 0 )
                                  AND ( A.discount > 0 )
                                  AND ( A.billno = @billno )
                           GROUP  BY A.serviceid,
                                     B.NAME) AS C
                   GROUP  BY C.departmentname
                  --ORDER  BY departmentname 
                  ),
               cte_ITEMLIST
               AS (SELECT A.serviceid,
                          CASE A.serviceid
                            WHEN 2 THEN 'ROOM AND BOARD'
                            ELSE B.NAME
                          END                                            AS DeptName,
                          CONVERT(VARCHAR(10), A.editorderdatetime, 101) AS OrderDate,
                          A.itemcode                                     AS Code,
                          A.itemname                                     AS Descript,
                          A.editprice                                    AS Price,
                          Count(A.editquantity)                          AS Qty,
                          A.discount                                     AS Disc,
                          A.deductableamount                             AS Deduc
                   FROM   dbo.aripbillitemdetail AS A
                          INNER JOIN dbo.department AS B
                                  ON A.departmentid = B.id
                   WHERE  ( A.serviceid NOT IN ( 1, 5, 21, 22,
                                                 23, 37, 14 ) )
                          AND ( A.editprice <> 0 )
                          AND ( A.billno = @billno )
                   GROUP  BY A.edititemid,
                             A.serviceid,
                             B.NAME,
                             CONVERT(VARCHAR(10), A.editorderdatetime, 101),
                             A.itemcode,
                             A.itemname,
                             A.editprice,
                             A.discount,
                             A.deductableamount
                   --ORDER  BY orderdate, departmentname, code
                   UNION ALL
                   SELECT A.Serviceid,
                          CASE A.ServiceId
                            WHEN 37 THEN 'TAKE HOME MEDICINE'
                            ELSE A.DeptName
                          END AS DeptName,
                          OrderDate,
                          Code,
                          Descript,
                          Price,
                          Qty,
                          Disc,
                          Deduc
                   FROM   (SELECT A.ServiceId,
                                  A.editorderdatetime AS OrderDate,
                                  A.itemcode          AS Code,
                                  A.itemname          AS Descript,
                                  B.NAME              AS DeptName,
                                  A.editprice         AS Price,
                                  A.editquantity      AS Qty,
                                  A.discount          AS Disc,
                                  A.deductableamount  AS Deduc
                           FROM   dbo.aripbillitemdetail AS A
                                  INNER JOIN dbo.department AS B
                                          ON A.departmentid = B.id
                           WHERE  ( A.serviceid IN ( 1, 5, 21, 23, 37 ) )
                                  AND ( A.editprice <> 0 )
                                  AND ( A.billno = @billno )
                           GROUP  BY A.serviceid,
                                     A.serialno,
                                     A.editorderdatetime,
                                     A.itemcode,
                                     A.itemname,
                                     A.serviceid,
                                     B.NAME,
                                     A.editprice,
                                     A.editquantity,
                                     A.discount,
                                     A.deductableamount) AS A),
               cte_DISCSUM
               AS (SELECT Sum(A.DiscAmt) AS DiscTotal
                   FROM   cte_PTDISCOUNT A),
               cte_GROSS
               AS (SELECT Sum(A.Price * A.Qty) AS Gross
                   FROM   cte_ITEMLIST A),
               cte_DEDUC
               AS (SELECT Sum(A.Deduc) AS Deduc
                   FROM   cte_ITEMLIST A),
               cte_NETAMOUNT
               AS (SELECT A.Gross - Isnull(B.DiscTotal, 0) - Isnull(C.Deduc, 0) AS NetAmount
                   FROM   cte_GROSS A
                          LEFT JOIN cte_DISCSUM B
                                 ON B.DiscTotal > 0
                          LEFT JOIN cte_DEDUC C
                                 ON C.Deduc > 0)
          --select * from cte_GROSS
          SELECT A.ServiceID,
                 CASE
                   WHEN A.ServiceID = 1
                         OR A.ServiceID = 15 THEN '*' + A.Code
                   ELSE A.Code
                 END                                          Code,
                 A.Deduc,
                 A.DeptName,
                 A.Descript,
                 A.Disc,
                 A.OrderDate,
                 A.Price,
                 A.Qty,
                 B.*,
                 Cast(Isnull(C.DiscPer, 0) AS FLOAT)          AS DiscPer,
                 Cast(Isnull(C.DiscAmt, 0) AS FLOAT)          AS DiscAmt,
                 Cast(Isnull(D.DiscTotal, 0) AS FLOAT)        AS DiscTotal,
                 dbo.Currency_towords(Isnull(E.NetAmount, 0)) AS NetAmountInWords
          FROM   cte_ITEMLIST A
                 LEFT JOIN cte_PTINFO B
                        ON B.BillNo = @billno
                 LEFT OUTER JOIN cte_PTDISCOUNT C
                              ON Lower(C.DisDeptName) = Lower(A.DeptName)
                 LEFT JOIN cte_DISCSUM D
                        ON D.DiscTotal > 0
                 LEFT JOIN cte_NETAMOUNT E
                        ON E.NetAmount > 0
          ORDER  BY A.DeptName,
                    A.Code,
                    A.OrderDate,
                    C.DiscAmt,
                    C.DiscPer,
                    C.DisDeptName
      END
END
--aripbilling.get_GIP_report_breakup 230082, 0

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_breakup_CST]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_GIP_report_breakup_CST] --230082
	@billno BIGINT
	--@ispack INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 0 AS ServiceId,
		   CASE
             WHEN SERVICEID = 37 THEN 'TAKE HOME MEDICINE'
             ELSE B.NAME
           END                 AS DeptName,
           A.EditOrderDateTime AS OrderDate,
           C.ItemCode          AS Code,
           C.NAME			   AS Descript,
           A.EditPrice		   AS Price,
           A.EditQuantity	   AS Qty,
           A.Discount		   AS Disc,
           A.DeductableAmount  AS Deduc
    FROM   dbo.ARIPBillItemDetail AS A
           INNER JOIN dbo.Item AS C
                   ON A.EditItemID = C.ID
           INNER JOIN dbo.Department AS B
                   ON A.DepartmentID = B.ID
    WHERE  ( A.ServiceID IN ( 1, 5, 21, 23, 37 ) )
           AND ( A.EditPrice <> 0 )
           AND ( A.BillNo = @billno )
    ORDER  BY OrderDate,
              B.NAME 
    
END
--[ARIPBILLING].[get_GIP_report_breakup_CST] 230082

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_breakup_RBMED]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_GIP_report_breakup_RBMED]
	@billno BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT A.ServiceID,
       ''                 AS DeptName,
       CASE SERVICEID
         WHEN 2 THEN Isnull(dbo.Fn_rbname(A.BEDTYPEID, d .CategoryId, d .CompanyId), '')
         ELSE 'MEDICAL CARE'
       END                AS Descript,
       A.EditPrice        AS Price,
       Count(A.EditPrice) AS Qty,
       A.Discount         AS Disc,
       A.DeductableAmount AS Deduc
FROM   Bed AS C
       INNER JOIN BedType AS B
               ON C.BedTypeID = B.id
       INNER JOIN ARIPBillItemDetail AS A
                  INNER JOIN ARIPBill AS D
                          ON A.BillNo = D.BillNo
               ON C.ID = A.EditItemID
WHERE  ( A.ServiceID IN ( 2 ) )
       AND ( A.BillNo = 230082 )
GROUP  BY A.ServiceID,
          A.BedTypeID,
          A.EditPrice,
          A.DeductableAmount,
          A.Discount,
          A.ServiceID,
          D.CategoryID,
          D.CompanyID 
END
--aripbilling.get_GIP_report_breakup_RBMED 230082, 0

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_GIP_report_main]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ARIPBILLING].[get_GIP_report_main] --230904, 0
	@billno BIGINT,
	@ispack INT
AS
BEGIN
	SET NOCOUNT ON;
	--select * from oldinpatient where ipid = 170121
	
--[INFORMATION]
	IF @ispack = 0
	BEGIN
		;WITH
		cte_PTINFO AS (
			SELECT 
				d.BillNo
				,('IPCR-' + CONVERT(VARCHAR(100),d.SlNo)) AS InvoiceNo
				,(a.issueauthoritycode + '.' + RIGHT('0000000000' + CAST(a.RegistrationNo AS VARCHAR(10)), 10)) AS Pin
				,REPLACE(CONVERT(VARCHAR(50), a.AdmitDateTime, 106),' ','-') AS AdmitDate
				,ISNULL(a.FamilyName, '') + ' ' + ISNULL(a.FirstName, '') + ' ' + ISNULL(a.MiddleName, '') + ' ' + ISNULL(a.LastName, '') AS PTName
				,LTRIM(RIGHT(CONVERT(CHAR(20), a.AdmitDateTime, 22), 11)) AS AdmTime
				,h.Address AS CompAddress
				,REPLACE(CONVERT(VARCHAR(50),a.DischargeDateTime , 106),' ','-') AS DisDate
				,h.Name AS CompName
				,h.Code AS CompCode
				,LTRIM(RIGHT(CONVERT(CHAR(20), a.DischargeDateTime, 22), 11)) AS DisTime
				,s.Name AS GradeName
				,ISNULL(PT.MedIDNumber, '') AS MedIDNumber
			FROM 
				dbo.ARIPBill AS d INNER JOIN
				dbo.Category AS b ON d.CategoryID = b.ID INNER JOIN
				dbo.Patient AS PT INNER JOIN
				dbo.OldInPatient AS a ON PT.Registrationno = a.RegistrationNo ON d.IPID = a.IPID INNER JOIN
				dbo.Grade AS s ON d.GradeID = s.ID INNER JOIN
				dbo.Company AS h ON d.CompanyID = h.ID
			WHERE
				(d.BillNo = @billno) 
		),
		cte_MAINDETAILS AS (
			SELECT BillNo, DepartmentName, 
				SUM(AMOUNT) AS Amount, 
				SUM(DISCOUNT) AS Discount, 
				SUM(DEDUCTABLE) AS Deductables
			FROM (SELECT
						A.BillNo,
						CASE SERVICEID WHEN 2 
						THEN 'ROOM AND BOARD' 
						ELSE B.NAME END AS DepartmentName, 
						SUM(A.EditPrice * A.EditQuantity) AS AMOUNT, 
						SUM(A.EditQuantity * A.Discount) AS DISCOUNT, 
						SUM(A.EditQuantity * A.DeductableAmount) AS DEDUCTABLE
					FROM dbo.ARIPBillItemDetail A 
						INNER JOIN Department  b ON A.DepartmentID = b.ID
					WHERE (A.ServiceID NOT IN (1, 5, 21, 23, 37, 14)) 
						AND (A.EditPrice <> 0) 
						AND (A.BillNo = @billno)
					GROUP BY A.BillNo, A.ServiceID, b.Name) 
				AS C
			GROUP BY BillNo, DepartmentName
			UNION ALL
			SELECT A.BillNo,    
				CASE SERVICEID WHEN 37 
					THEN 'TAKE HOME MEDICINE' 
					ELSE B.NAME END AS DepartmentName, 
					SUM(A.EditPrice * A.EditQuantity) AS Amount, 
					SUM(A.EditQuantity * A.Discount) AS Discount, 
					SUM(A.EditQuantity * A.DeductableAmount) AS Deductables
			FROM dbo.ARIPBillItemDetail A 
				INNER JOIN dbo.Department B ON A.DepartmentID = B.ID
			WHERE (A.ServiceID IN (1, 5, 21, 23, 37)) 
				AND (A.BillNo = @billno)
			GROUP BY A.BillNo,A.ServiceID, B.Name
		),
		cte_PTADVPAY AS
		(
			SELECT 
				@billno AS BillNo
				,ReceiptNo
				,Amount
				,REPLACE(CONVERT(VARCHAR(50), DATETIME, 106),' ','-') + ' ' 
				+ LTRIM(RIGHT(CONVERT(CHAR(20), DATETIME, 22), 11)) AS PDateTime
				FROM dbo.IPTRANSACTIONS WHERE IPID=(SELECT IPID FROM ARIPBill WHERE billno = @billno)
		--select * from iptransactions
		),
		cte_TOT1 AS (
			SELECT A.BillNo, A.DepartmentName, A.Deductables, A.Discount 
				,A.Amount
				,ISNULL(convert(varchar(50),C.ReceiptNo), '')AS ReceiptNo
				,ISNULL(C.Amount, 0) AS AdvPayAmount
				,ISNULL(C.PDateTime, '') as AdvDateTime
				,B.InvoiceNo
				,B.Pin
				,B.PTName
				--in words
				--,SUM(A.Amount) as SUMWORDS
				--,dbo.Currency_ToWords(SUM(A.Amount))
				,B.AdmitDate
				,B.AdmTime
				,B.CompAddress
				,B.CompCode
				,B.CompName
				,B.GradeName
				,B.MedIDNumber
				,B.DisDate
				,B.DisTime
				FROM cte_MAINDETAILS A
			RIGHT OUTER JOIN cte_PTINFO B ON B.BillNo = A.BillNo
			LEFT OUTER JOIN cte_PTADVPAY C ON C.BillNo = B.BillNo
		),
		cte_SUMDED AS (
			select SUM(Deductables) as SUMDED from cte_TOT1
		),
		cte_SUMDISC AS (
			select SUM(Discount) as SUMDISC from cte_TOT1
		),
		cte_BILLTOTAL AS (
			select SUM(Amount) as SUMTOT from cte_TOT1
		),
		cte_SUMADVPAY AS (
			select SUM(Amount) as SUMADVPAY from cte_PTADVPAY
		)
		
		select A.*, 
		dbo.Currency_ToWords(
		ISNULL(B.SUMTOT,0) 
		- ISNULL(C.SUMDED,0) 
		- ISNULL(D.SUMDISC,0) 
		- ISNULL(E.SUMADVPAY, 0)) as NumInWords 
		from cte_TOT1 A 
		LEFT JOIN cte_BILLTOTAL B ON B.SUMTOT <> 0
		LEFT JOIN cte_SUMDED C ON C.SUMDED <> 0
		LEFT JOIN cte_SUMDISC D ON D.SUMDISC <> 0
		LEFT JOIN cte_SUMADVPAY E ON E.SUMADVPAY <> 0
			
		--aripbilling.get_GIP_report_main 230904, 0
		--#########################
		--SELECT TYPE,RECEIPTNO,AMOUNT,DATETIME FROM IPTRANSACTIONS WHERE IPID=0
	END
END

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_admitdate]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_ipbill_admitdate]
	@pin BIGINT,
	@retcode INT OUT,
	@retmsgs VARCHAR(500) OUT
AS
BEGIN
	SET NOCOUNT ON;
	
	SET @retcode = 0
	SET @retmsgs = ''
	
 DECLARE @BlockMessage VARCHAR(250),
              @BlockMID     INT,
              @BlockAmount  DECIMAL(18, 2),
              @BlockRD      DATETIME,
              @MappedTo     VARCHAR(20)

      -- check if PIN is blocked on status 0
      IF EXISTS(SELECT TOP 1 RegistrationNo
                FROM   dbo.pinblock
                WHERE  RegistrationNo = @pin
                       AND Blocked = 1
                       AND Status = 0)
        BEGIN
            SELECT TOP 1 @BlockMessage = B.NAME,
                         @BlockMID = A.MessageTypeId,
                         @BlockAmount = ISNULL(A.BLOCKAMOUNT, 0),
                         @BlockRD = A.ReleaseDateTime
            FROM   dbo.pinblock A
                   LEFT JOIN dbo.PINMessages B
                          ON B.Id = A.MessageTypeId
            WHERE  A.RegistrationNo = @pin
                   AND A.Blocked = 1
                   AND Status = 0
                   
                   
            SET @retcode = 1

            SET @retmsgs = 'This PIN Number is Blocked!<br><b>REASON</b> : '
                                + @BlockMessage

            IF @BlockMID = 3
              BEGIN
                  SET @retmsgs = 'Patient Expired!'
              END

            IF @BlockRD IS NULL
              BEGIN
                  SET @retmsgs = ISNULL(@BlockMessage, 'AR')

                  IF @BlockAmount > 0
                    BEGIN
                        SET @retmsgs = @retmsgs
                                            + '<br><b>BLOCKED AMOUNT</b> : '
                                            + CONVERT(VARCHAR(100), @BlockAmount)
                    END
              END
            ELSE
              BEGIN
                  IF DATEDIFF(D, @BlockRD, GETDATE()) <> 0
                    BEGIN
                        SET @retmsgs = ISNULL(@BlockMessage, 'AR')

                        IF @BlockAmount > 0
                          BEGIN
                              SET @retmsgs = @retmsgs
                                                  + '<br><b>BLOCKED AMOUNT</b> : '
                                                  + @BlockAmount
                          END
                    END
              END

            --PRINT @ErrorMessage -- <-- comment out | for testing

            RETURN
        END
	ELSE
	BEGIN
	
	SELECT
		   b.BillNo AS BillNo,
		   REPLACE(CONVERT(VARCHAR(50),a.AdmitDateTime , 106),' ','-') 
		   + ' ' 
		   +  LTRIM(RIGHT(CONVERT(CHAR(20), a.AdmitDateTime, 22), 11)) AdmissionDate
		FROM   dbo.ARIPBill AS b
			   INNER JOIN dbo.Company AS C
					   ON b.CompanyID = C.ID
			   INNER JOIN dbo.OldInPatient AS a
					   ON b.IPID = a.IPID
		WHERE  ( ISNULL(C.Aramco, 0) <> 1 )
			   AND ( a.RegistrationNo = @pin )
		ORDER  BY a.AdmitDateTime DESC
	END
END
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_info]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_ipbill_info]
	@billno BIGINT,
	@retcode INT OUT,
	@retmsgs VARCHAR(500) OUT
AS
BEGIN
	SET NOCOUNT ON;
	
	SET @retcode = 0
	SET @retmsgs = ''
	
	IF EXISTS(select hascompanyletter from dbo.aripbill where billno = @billno and HasCompanyLetter = 1)
	BEGIN
		SET @retcode = 1
		SET @retmsgs = '<b>Bill is LOCKED!</b><br>Covering Letter has already been Generated.'
		RETURN
	END
	ELSE
	BEGIN
		SELECT 
		a.IPID,
		b.BillNo,
		   Upper(Isnull(a.familyname, '')) + ' '
             + Upper(Isnull(a.Firstname, '')) + ' '
             + Upper(Isnull(a.MiddleName, '')) + ' '
             + Upper(Isnull(a.LastName, ''))   AS PTName,
		   a.Age,
		   s.NAME                    AS Sex,
		   b.CategoryId,
		   c.Code + ' - ' + c.NAME                    AS Category,
		   b.CompanyId,
		   cc.Code + ' - ' + cc.NAME AS Company,
		   b.TariffId,
		   cc.FollowRules,
		   b.GradeId,
		   RTRIM(g.NAME)                    AS GradeName,
		   b.BillAmount,
		   b.EditBillAmount,
		   b.SlNo,
		   
		   t.NAME                    AS BillType,
		   REPLACE(CONVERT(VARCHAR(50),b.BillDate , 106),' ','-') 
		   + ' ' 
		   +  LTRIM(RIGHT(CONVERT(CHAR(20), b.BillDate, 22), 11)) BillDate,
		   b.DeductableType,
		   b.BedTypeId,
		   b.IsInvoiced
	FROM   dbo.Sex AS s
		   INNER JOIN dbo.OldInPatient AS a
				   ON s.ID = a.Sex
		   INNER JOIN dbo.Category AS c
					  INNER JOIN dbo.ARIPBill AS b
							  ON c.ID = b.CategoryID
					  INNER JOIN dbo.Company AS cc
							  ON b.CompanyID = cc.ID
				   ON a.IPID = b.IPID
		   INNER JOIN dbo.Grade AS g
				   ON b.GradeID = g.ID
		   INNER JOIN dbo.IPBillType AS t
				   ON b.BillType = t.Id
	WHERE  ( b.BillNo = @billno)
	END
	
END

-- aripbilling.get_ipbill_info 246003
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_ipbservice]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_ipbill_ipbservice]
	@catid INT,
	@comid BIGINT,
	@graid BIGINT
AS
BEGIN

                           
        SELECT
			Id, SERVICENAME AS Name 
        
        FROM dbo.IPBService a 
        WHERE 
			a.Deleted = 0
        AND
        a.ID NOT IN(SELECT serviceid FROM dbo.IPCompanyServices  WHERE
                          			CompanyID = @comid
                                         AND CategoryID = @catid
                                         AND GradeID = @graid
                                         AND ( IncludeType = 0
                                                OR (IncludeType = 1
                                                     AND LEVEL = 2 ) )
                                  )
        ORDER BY a.ServiceName
                            
                          
END

-- aripbilling.get_ipbill_ipbservice 24, 16357, 30551

GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_items]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_ipbill_items]
	@serid INT,
	@billno BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @SQL VARCHAR(MAX)
	
	SELECT Isnull(A.ItemCode, '')    AS Code,
		   Isnull(A.ItemName, '')    AS Name,
		   UPPER(Isnull(A.ItemCode, '')) + ' - ' + UPPER(Isnull(A.ItemName, '')) as ItemName,
		   A.BillNo,
		   A.SerialNo,
		   A.OrderId,
		   A.EditItemId,
		   A.Quantity,
		   A.EditQuantity,
		   A.EditOrderDateTime,
		   A.EditPrice,
		   A.Price,
		   B.EditBillAmount,
		   A.EditFromDateTime,
		   A.EditToDateTime,
		    REPLACE(CONVERT(VARCHAR(50),A.Datetime , 106),' ','-') 
		   + ' ' 
		   +  LTRIM(RIGHT(CONVERT(CHAR(20), A.Datetime, 22), 11)) DateTime,
		   A.FromDateTime,
		   A.ToDateTime,
		   A.DeductableAmount,
		   A.Discount,
		   B.DeductableType,
		   A.DiscountLevel,
		   A.DeductableLevel,
		   Isnull(A.MarkupAmount, 0) AS MarkUpAmount,
		   Isnull(A.BedTypeID, 1)    AS BedTypeId
	FROM   dbo.ARIPBillItemDetail AS A
		   INNER JOIN dbo.ARIPBill AS B
				   ON A.BillNo = B.BillNo
	WHERE  ( B.BillNo = @billno )
		   AND ( A.ServiceID = @serid )
		   AND ( B.BillType <> 6 )
		   AND ( A.EditPrice <> 0 )
	ORDER  BY A.ServiceID,
			  A.EditOrderDateTime,
			  A.OrderID,
			  A.EditItemID 
	
END
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_posneg_adj]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_ipbill_posneg_adj]
	@billno BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Sum(NegativeAdj) AS NegativeAdj,
           Sum(PositiveAdj) AS PositiveAdj
    FROM   (SELECT Sum(CASE
                         WHEN ( ( Price * Quantity ) - ( EditPrice * EditQuantity ) ) > 0 THEN ( ( EditPrice * EditQuantity ) - ( Price * Quantity ) )
                         ELSE 0
                       END) AS NegativeAdj,
                   Sum(CASE
                         WHEN ( ( Price * Quantity ) - ( EditPrice * EditQuantity ) ) < 0 THEN ( ( EditPrice * EditQuantity ) - ( Price * Quantity ) )
                         ELSE 0
                       END) AS PositiveAdj
            FROM   dbo.ARIPBillItemDetail
            WHERE  ( BillNo = @billno )
            UNION
            SELECT -Sum(Amount) AS NegativeAdj,
                   0            AS PositiveAdj
            FROM   (SELECT DISTINCT billno,
                                    serviceid,
                                    orderid,
                                    itemid,
                                    datetime,
                                    quantity * price AS Amount
                    FROM   dbo.CancelARIPBillItemDetail AS a
                    WHERE  ( billno = @billno )
                           AND ( itemid NOT IN (SELECT ItemID
                                                FROM   dbo.ARIPBillItemDetail
                                                WHERE  ( BillNo = a.billno )
                                                       AND ( ServiceID = a.serviceid )
                                                       AND ( Isnull(OrderID, 0) = Isnull(a.orderid, 0) )
                                                       AND ( ItemID = a.itemid )
                                                       AND ( Datetime = a.datetime )) )
                    GROUP  BY billno,
                              serviceid,
                              orderid,
                              itemid,
                              datetime,
                              price,
                              quantity) AS c) AS i 
    
	
END
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_service_items]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [ARIPBILLING].[get_ipbill_service_items]
	@serid int,
	@comid bigint,
	@graid bigint
as
begin
	set nocount on;
	
	DECLARE @SQL VARCHAR(MAX), @MTBL VARCHAR(200)
	
	SELECT @MTBL = RTRIM(mastertable) from dbo.IPBService where ID = @serid
	
	IF @serid IN (20, 13, 29, 26, 16, 12, 7, 24, 6, 8, 10)
	BEGIN
		SET @SQL ='SELECT Id, Code + '' - '' + NAME AS Name 
					FROM dbo.' + @MTBL + ' WHERE  ( Deleted = 0 )
					AND ID NOT IN (SELECT ItemID
								FROM   dbo.IPCompanyItemServices
								WHERE  ( CODE IS NOT NULL
										AND NAME iS NOT NULL
								       AND CompanyID = ' + CONVERT(VARCHAR(50),@comid) + ' )
									   AND ( GradeID = ' + CONVERT(VARCHAR(50),@graid) + ' )
									   AND ( ServiceID = ' + CONVERT(VARCHAR(50),@serid) + ' )) '
    
		EXEC (@SQL)
    END
    
    IF @serid IN (3, 2)
	BEGIN
		SET @SQL ='SELECT Id, NAME AS Name 
					FROM dbo.bed WHERE  ( Deleted = 0 )
					AND ID NOT IN (SELECT ItemID
								FROM   dbo.IPCompanyItemServices
								WHERE  ( NAME iS NOT NULL
								       AND CompanyID = ' + CONVERT(VARCHAR(50),@comid) + ' )
									   AND ( GradeID = ' + CONVERT(VARCHAR(50),@graid) + ' )
									   AND ( ServiceID = ' + CONVERT(VARCHAR(50),@serid) + ' )) '
    
		EXEC (@SQL)
    END
    
    IF @serid IN (1)
	BEGIN
		SET @SQL ='SELECT Id, ItemCode + '' - '' + NAME AS Name 
					FROM dbo.' + @MTBL + ' WHERE  ( Deleted = 0 )
					AND ID NOT IN (SELECT ItemID
								FROM   dbo.IPCompanyItemServices
								WHERE  ( ItemCODE IS NOT NULL
										AND NAME iS NOT NULL
								       AND CompanyID = ' + CONVERT(VARCHAR(50),@comid) + ' )
									   AND ( GradeID = ' + CONVERT(VARCHAR(50),@graid) + ' )
									   AND ( ServiceID = ' + CONVERT(VARCHAR(50),@serid) + ' )) '
    
		EXEC (@SQL)
    
    END
    
    IF @serid IN (15)
	BEGIN
		SET @SQL ='SELECT Id, EmpCode + '' - '' + NAME AS Name 
					FROM dbo.' + @MTBL + ' WHERE  ( Deleted = 0 )
					AND ID NOT IN (SELECT ItemID
								FROM   dbo.IPCompanyItemServices
								WHERE  ( EmpCode IS NOT NULL
										AND NAME iS NOT NULL
								       AND CompanyID = ' + CONVERT(VARCHAR(50),@comid) + ' )
									   AND ( GradeID = ' + CONVERT(VARCHAR(50),@graid) + ' )
									   AND ( ServiceID = ' + CONVERT(VARCHAR(50),@serid) + ' )) '
    
		EXEC (@SQL)
    
    END
    
    IF @serid IN (25)
    BEGIN
		SELECT Id,
               CODE + ' - ' + NAME AS Name
        FROM   (SELECT ID=0,
                       NAME='CROSS MATCH',
                       Code='',
                       DepartmentID = 54, -- change for cairo from 54 = 
                       Deleted = 0 )i
        WHERE  Deleted = 0
               AND ID NOT IN (SELECT ItemID
                              FROM   dbo.IPCompanyItemServices
                              WHERE  CompanyID = @comid
                                     AND GradeID = @graid
                                     AND ServiceID = @serid) 
        
    END

	IF @serid IN (5, 37)
	BEGIN
		SET @SQL ='SELECT Id, ItemCode + '' - '' + NAME AS Name 
					FROM dbo.PharmacyItem WHERE  ( Deleted = 0 )
					AND ID NOT IN (SELECT ItemID
								FROM   dbo.IPCompanyItemServices
								WHERE  ( ItemCODE IS NOT NULL
										AND NAME iS NOT NULL
								       AND CompanyID = ' + CONVERT(VARCHAR(50),@comid) + ' )
									   AND ( GradeID = ' + CONVERT(VARCHAR(50),@graid) + ' )
									   AND ( ServiceID = ' + CONVERT(VARCHAR(50),@serid) + ' )) '
    
		EXEC (@SQL)
    
    END

end

-- aripbilling.get_ipbill_service_items 37, 16357, 30551


GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_ipbill_services]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ARIPBILLING].[get_ipbill_services]
	@billno bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT ServiceName Name,
		   Id
	FROM   IPBService
	WHERE  ( ID IN (SELECT ServiceID
					FROM   ARIPBillDetail
					WHERE  ( BillNo = @billno )
						   AND ( Amount <> 0 )
							OR ( BillNo = @billno )
							   AND ( ApolloAmount <> 0 )
							OR ( BillNo = @billno )
							   AND ( EditAmount <> 0 )
					GROUP  BY ServiceID) ) 


	
END
GO

/****** Object:  StoredProcedure [ARIPBILLING].[get_nonpack_admitdate]    Script Date: 02/15/2016 18:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [ARIPBILLING].[get_nonpack_admitdate] -- 245995
	@pin bigint
as
begin
	set nocount on;
	SELECT 
	REPLACE(CONVERT (VARCHAR(50), a.AdmitDateTime, 106), ' ', '-')+ ' ' +
		SUBSTRING(CONVERT(VARCHAR,a.AdmitDateTime,22),10,8) + ' ' + SUBSTRING(CONVERT(VARCHAR,a.AdmitDateTime,22), 19,2) AdmitDateTime,
           a.IPID,
           B.BillNo
    FROM   OldInPatient AS a
           INNER JOIN ARIPBill AS B
                   ON a.IPID = B.IPID
           INNER JOIN Company AS c
                   ON B.CompanyID = c.ID
    WHERE  ( a.Billtype = 2 )
           AND ( a.RegistrationNo = @pin )
    ORDER  BY a.AdmitDateTime DESC,
              a.IPID DESC 
    
	
end

-- aripbilling.get_nonpack_admitdate 1461169
GO

