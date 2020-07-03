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
-- Create date: <10-23-2016 update subcategory>
-- Description:	<from [MCRS].[ARReports_GetStatementSummary]>
-- =============================================
CREATE PROCEDURE [MCRS].[ARReports_GetStatementSummaryOfAccountwithSubCategoryName] 
	 (              
  @stDate    DATETIME,              
  @enDate    DATETIME,              
  @Category  INT,              
  @Option    TINYINT=0,
  @companyId INT,
  @gradeId   INT,
  @SubCategoryName Varchar(150)            
 ) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 
    DECLARE @branch VARCHAR(100), @address VARCHAR(100);
      SET @branch = (SELECT TOP 1 Name + ' - ' + City FROM OrganisationDetails)   
      SET @address = (SELECT TOP 1 Address1 + ' - ' + City FROM OrganisationDetails)
	
	IF OBJECT_ID('tempdb..#TSummary') IS NOT NULL
		DROP TABLE #TSummary
		
	IF OBJECT_ID('tempdb..#T2') IS NOT NULL
		DROP TABLE #T2
		
	IF OBJECT_ID('tempdb..#T3') IS NOT NULL
		DROP TABLE #T3

	CREATE TABLE #TSummary 
	(
		CCode			VARCHAR(20), 
		CompanyName		VARCHAR(100), 
		IPCount			INT, 
		OPCount			INT, 
		IPGross			MONEY,              
		OPGross			MONEY, 
		IPDiscount		MONEY, 
		OPDiscount		MONEY, 
		IPDeductable	MONEY, 
		OPDeductable	MONEY,
		SubCategory		VARCHAR(100)
	)

	CREATE TABLE #T2
	(
		  BillNo				INT
		, CompanyID				INT
		, RegistrationNo		INT
		, IssueAuthorityCode	VARCHAR(6)
		, Name					VARCHAR(200)
		, InvoiceNo				VARCHAR(200)
		, Consultation			MONEY
		, Investigation			MONEY
		, MedTreat				MONEY
		, Medicine				MONEY
		, Total					MONEY
		, Deposits				MONEY
		, Deductable			MONEY
		, DeductableType		TINYINT
		, PatientType			TINYINT
		, NoOfDays				INT
	)

	CREATE TABLE #T3
	(
		  BillNo				INT
		, CompanyId				INT
		, IssueAuthorityCode	VARCHAR(6)
		, Registrationno		INT
		, InvNo					VARCHAR(200)
		, IPID					INT
		, DiscountPha			MONEY
		, DiscountOth			MONEY
		, Discount				MONEY
	)	

	/*
	------------------------------------------------------------
		Inserting Data for OP  
	------------------------------------------------------------
	*/
	INSERT INTO #TSummary
		(
			CCode, CompanyName, IPCount, OPCount, IPGross, OPGross, IPDiscount, OPDiscount, IPDeductable,OPDeductable,SubCategory 
		)
	SELECT 
		  B.Code
		, B.Name
		, 0 AS IPCount
		, C.OPCount
		, 0 AS IPGross
		, SUM(A.BillAmount) AS OPGross
		, 0 AS IPDiscount
		, SUM(A.Discount) AS OPDisc
		, 0 AS IPDeduct
		, SUM(A.PaidAmount) AS OPDeduc 
		, d.name
	FROM ARCompanyBillDetail A (nolock)
	INNER JOIN Company B  (nolock)
		ON A.CompanyID = B.ID              
	LEFT JOIN SubCategory D
	    ON b.SubCategoryId =d.id	
	INNER JOIN 
			(SELECT B1.Code
				, COUNT(DISTINCT A1.RegistrationNo) AS OPCount 
			FROM ARCompanyBillDetail A1 (nolock)
			INNER JOIN Company B1 (nolock)
				ON A1.CompanyID = B1.ID              
			WHERE 
					A1.CategoryID = @Category 
				AND A1.Posted > 1 
				AND A1.BillDateTime >= @stDate AND A1.BillDateTime <= @enDate 
				
			GROUP BY B1.Code
			) AS C ON B.Code = C.Code              
	WHERE 
			A.CategoryID = @Category  
		AND A.Posted <> 0 
		AND A.BillDateTime >= @stDate AND A.BillDateTime < @enDate 
		AND(@companyId = 0 OR B.ID  = @companyId)
		AND (@gradeId = 0 OR A.GradeID = @gradeId)
	GROUP BY B.Code, B.Name, C.OPCount,d.name     



	/*
	------------------------------------------------------------
		Inserting Data for IP  
	------------------------------------------------------------
	*/
	INSERT INTO #T2
	SELECT 
		  IPBill.BillNo
		, IPBill.CompanyID
		, IPBill.RegistrationNo
		, IPBill.IssueAuthorityCode
		, IPBill.Name
		, IPBill.InvoiceBillNo AS InvoiceNo
		, SUM(RoomRent) AS Consultation
		, SUM(Investigation) AS Investigation
		, SUM(MedTreat) AS MedTreat
		, SUM(Medicine) AS Medicine 
		, SUM(Total) AS Total
		, SUM(Deposits) AS Deposits
		, SUM(Deductable) AS Deductable
		, DeductableType=0
		, patienttype=1
		, SUM(NOOfDays) AS NOOfDays 
	FROM 
	(              
		SELECT 
			  a.BillNo
			, b.CompanyID
			, (C.FirstName +' '+c.MiddleName+' '+c.LastName) AS Name 
			, c.IssueAuthorityCode 
			, c.Registrationno
			, d.name + ' ' + CONVERT(VARCHAR,b.Slno) AS [InvoiceBillNo]
			, c.ipid AS IPID
			, SUM(a.EditQuantity*a.DeductableAmount) AS Deductable
			, (CASE WHEN serviceid=13 THEN SUM(EditPrice*EditQuantity) ELSE 0 END)  AS Investigation
			, (CASE WHEN serviceid IN(5,23,37) THEN SUM(EditPrice*EditQuantity) ELSE 0 END) AS Medicine
			, (CASE WHEN serviceid =2 THEN SUM(EditPrice*EditQuantity) ELSE 0 END) AS RoomRent
			, (CASE WHEN serviceid =2 THEN SUM(EditQuantity) ELSE 0 END) AS NoOfDays
			, (CASE WHEN serviceid NOT IN(5,13,2,23,37) THEN SUM(EditPrice*EditQuantity) ELSE 0 END) AS MedTreat
			, SUM(EditPrice*EditQuantity) AS Total
			, Deposits=0 
		FROM 
			  Aripbillitemdetail a (nolock)
			, aripbill b (nolock)
			, OldInpatient c (nolock)
			, IPBillType d (nolock)    
		WHERE 
				b.billNo = A.billNo 
			AND b.ipid = C.ipid 
			AND d.ID = b.BILLTYPE 
			AND b.IsInvoiced = 1 
			AND b.InvoiceDatetime >= @stDate AND b.InvoiceDatetime < @enDate 
			AND b.CategoryID = @Category 
			AND a.ServiceID<>14 
			AND(@gradeId = 0 OR b.GradeID = @gradeId)
		GROUP BY 
			a.billNo,b.CompanyId,C.FirstName, 
			C.Registrationno ,b.Slno,a.Serviceid,d.Name,c.IssueAuthorityCode,c.MiddleName,c.LastName,c.ipid             
		HAVING SUM(EditPrice * EditQuantity) > 0.1
	) AS IPBill 
	GROUP BY 
		  IPBill.BillNo,IPBill.CompanyID,IPBill.RegistrationNo,IPBill.IssueAuthorityCode
		, IPBill.InvoiceBillNo,IPBill.Name,IPBill.IPID
		
		
	INSERT INTO #T3
	SELECT 
		  BillNo
		, CompanyId
		, IssueAuthorityCode 
		, Registrationno
		, [InvoiceBillNo] AS InvNo
		, IPID
		, SUM(ROUND(DiscountPha,2))AS DiscountPha
		, SUM(ROUND(DiscountOth,2))AS DiscountOth
		, SUM(ROUND(Discount,2)) AS Discount 
	FROM 
	(              
		SELECT 
			  a.BillNo
			, b.CompanyId
			, c.IssueAuthorityCode 
			, c.Registrationno
			, a.DepartmentID
			, d.name + ' ' + CONVERT(VARCHAR,b.Slno) AS [InvoiceBillNo]
			, c.ipid AS IPID
			, CASE WHEN Serviceid IN(5,37) THEN SUM(a.EditQuantity*a.Discount) ELSE 0 END AS DiscountPha
			, CASE WHEN Serviceid NOT IN(5,37) THEN SUM(a.EditQuantity*a.Discount) ELSE 0 END AS DiscountOth
			, SUM(A.EditQuantity * A.Discount) AS Discount              
		FROM 
			  Aripbillitemdetail a (nolock)
			, aripbill b (nolock)
			, OldInpatient c  (nolock)
			, IPBillType d (nolock) 
		WHERE 
				b.billNo = A.billNo 
			AND b.ipid = C.ipid 
			AND d.ID = b.BILLTYPE 
			AND b.IsInvoiced = 1 
			AND b.InvoiceDatetime >= @stDate AND b.InvoiceDatetime < @enDate 
			AND b.CategoryID = @Category  
			AND a.ServiceID<>14 
			AND(@gradeId = 0 OR b.GradeID = @gradeId)
			
		GROUP BY 
			  a.BillNo,b.CompanyId, C.Registrationno ,b.Slno
			, a.Serviceid,c.IssueAuthorityCode,a.DepartmentID, d.name,c.ipid               
		HAVING SUM(EditPrice*EditQuantity) > 0.1
	)TT 
	GROUP BY 
		BillNo,CompanyId, IssueAuthorityCode , Registrationno, InvoiceBillNo, IPID 
		
		
	;WITH T91
	AS
	(
		SELECT 
			  CompanyID
			, COUNT(DISTINCT IPID) AS IPCount 
		FROM 
		(
			SELECT 
				  B11.IPID
				, B11.CompanyID
				, COUNT( DISTINCT B11.IPID) AS IPCount 
			FROM ARIPBillItemDetail A11  (nolock)
			INNER JOIN ARIPBill B11  (nolock)
				ON A11.BillNo = B11.BillNo 
			WHERE 
					InvoiceDateTime >= @stDate AND InvoiceDateTime < @enDate 
				AND B11.CategoryID = @Category
				AND A11.ServiceID <> 14 
				AND B11.IsInvoiced=1 
				AND (@gradeId = 0 OR B11.GradeID = @gradeId)
			GROUP BY 
				B11.IPID, B11.CompanyID 
			HAVING SUM(A11.EditPrice*A11.EditQuantity)>0.1
		) AS Bill1 
		GROUP BY CompanyID
	),
	T9
	AS
	(
		SELECT 
			  T2.CompanyID
			, T2.RegistrationNo
			, T2.IssueAuthorityCode
			, T2.Name
			, T2.InvoiceNo
			, SUM(Consultation) AS Consultation
			, SUM(Investigation) AS Investigation
			, SUM(MedTreat) AS MedTreat
			, SUM(Medicine) AS Medicine 
			, SUM(Total) AS Total
			, SUM(T2.Deposits) AS Deposits
			, SUM(T3.DiscountPha) AS DiscountPha
			, SUM(T3.DiscountOth) AS DiscountOth
			, SUM(T3.Discount) AS Discount
			, SUM(Deductable) AS Deductable
			, SUM(CONVERT(MONEY,Total))-SUM(T3.Discount)-SUM(CONVERT(MONEY,Deductable))-SUM(CONVERT(MONEY,T2.Deposits)) AS NetAmount
			, DeductableType=0
			, patienttype=1
			, SUM(NOOfDays) AS NOOfDays
			, IPID       
		FROM #T2 T2
		INNER JOIN #T3 T3
			ON		T2.BillNo=T3.BillNo 
				AND T2.CompanyId = T3.CompanyId 
				AND T2.RegistrationNo = T3.RegistrationNo 
				AND T2.IssueAuthorityCode = T3.IssueAuthorityCode 
				AND T2.InvoiceNo = T3.invno 
		GROUP BY 
			T2.CompanyId,T2.RegistrationNo,T2.IssueAuthorityCode,T2.Name,T2.InvoiceNo,IPID
	)

	INSERT INTO #TSummary
		(
			CCode, CompanyName, IPCount, OPCount, IPGross, OPGross, IPDiscount, OPDiscount, IPDeductable,OPDeductable
		)              
	SELECT 
		  E.Code
		, E.Name
		, T91.IPCount
		, 0 AS OPCount
		, SUM(ROUND(Total,2)) AS IPGross
		, 0 AS OPGross
		, SUM(Discount) AS IPDiscount
		, 0 AS OPDisc
		, SUM(ROUND(Deductable,2)) AS IPDeduc
		, 0 AS OPDeduc 
	FROM T9
	INNER JOIN Company E  (nolock)
		ON T9.CompanyID = E.ID     
	INNER JOIN T91 
		ON T9.CompanyID = T91.CompanyID  
	WHERE (@companyId = 0 OR E.ID  = @companyId)
	GROUP BY E.Code, E.Name, T91.IPCount 

	IF @Category IN (23,70) AND @Option = 0            
		BEGIN            
		   --Update sub categories            
		        
			UPDATE #TSummary 
				SET SubCategory = s.Name            
			FROM #TSummary t1 
			INNER JOIN Company c 
				ON t1.CCode = c.Code             
			INNER JOIN SubCategory s 
				ON c.SubCategoryId = s.id            
		    
			
			if(@SubCategoryName <> '0')
		 BEGIN
				 SELECT 
						  SubCategory
						, SUM(IPCount) AS IPCount
						, SUM(OPCount) AS OPCount
						, SUM(IPCount+OPCount) AS TCount
						, SUM(IPGross) AS IPGross
						, SUM(IPDiscount) AS IPDiscount
						, SUM(IPDeductable) AS IPDeductable
						, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2)) AS IPNet
						, SUM(OPGross) AS OPGross
						, SUM(OPDiscount) AS OPDiscount
						, SUM(OPDeductable) AS OPDeductable
						, SUM(OPGross-OPDiscount-OPDeductable) AS OPNet
						, SUM(IPGross+OPGross) AS TGross
						, SUM(IPDiscount+OPDiscount) AS TDisc
						, SUM(IPDeductable+OPDeductable) AS TDeduc
						, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2))+SUM(OPGross-OPDiscount-OPDeductable) AS TNet
						,UPPER(@branch) branch
						,@address address
						,ROW_NUMBER() OVER(ORDER BY SubCategory) slno
					FROM #TSummary  where SubCategory = @SubCategoryName
					GROUP BY SubCategory 
					ORDER BY SubCategory   


		 END
		 
		 ELSE
		 
		 BEGIN

		 SELECT 
				  SubCategory
				, SUM(IPCount) AS IPCount
				, SUM(OPCount) AS OPCount
				, SUM(IPCount+OPCount) AS TCount
				, SUM(IPGross) AS IPGross
				, SUM(IPDiscount) AS IPDiscount
				, SUM(IPDeductable) AS IPDeductable
				, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2)) AS IPNet
				, SUM(OPGross) AS OPGross
				, SUM(OPDiscount) AS OPDiscount
				, SUM(OPDeductable) AS OPDeductable
				, SUM(OPGross-OPDiscount-OPDeductable) AS OPNet
				, SUM(IPGross+OPGross) AS TGross
				, SUM(IPDiscount+OPDiscount) AS TDisc
				, SUM(IPDeductable+OPDeductable) AS TDeduc
				, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2))+SUM(OPGross-OPDiscount-OPDeductable) AS TNet
				,UPPER(@branch) branch
				,@address address
				,ROW_NUMBER() OVER(ORDER BY SubCategory) slno
			FROM #TSummary 
			GROUP BY SubCategory 
			ORDER BY SubCategory   

		 END        
			         
		END             
	ELSE IF @Category = 24 AND @Option = 1            
		BEGIN  
		
		 if(@SubCategoryName <> '0')
		 BEGIN

		 	 SELECT 
				  CCode
				, CompanyName
				, ISNULL(t2.PolicyNo,'') AS PolicyNo
				, SUM(IPGross+OPGross) AS TGross
				, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2))+SUM(OPGross-OPDiscount-OPDeductable) AS TNet
				,UPPER(@branch) branch
				,@address address 
				,ROW_NUMBER() OVER(ORDER BY SubCategory) slno           
			FROM #TSummary t1
			LEFT OUTER JOIN Company t2  (nolock)
				ON t1.CCode = t2.Code  
				 where SubCategory = @SubCategoryName
			GROUP BY CCode, CompanyName, ISNULL(t2.PolicyNo,'') 

		 END
		 ELSE
		 BEGIN

		 SELECT 
				  CCode
				, CompanyName
				, ISNULL(t2.PolicyNo,'') AS PolicyNo
				, SUM(IPGross+OPGross) AS TGross
				, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2))+SUM(OPGross-OPDiscount-OPDeductable) AS TNet
				,UPPER(@branch) branch
				,@address address 
				,ROW_NUMBER() OVER(ORDER BY SubCategory) slno           
			FROM #TSummary t1 
			LEFT OUTER JOIN Company t2  (nolock)
				ON t1.CCode = t2.Code 
			GROUP BY CCode, CompanyName, ISNULL(t2.PolicyNo,'')  


		 END
		           
			          
		 END            
	ELSE            
		 BEGIN  
		 
		 if(@SubCategoryName <> '0')
		 BEGIN

					 SELECT 
							  SubCategory 				
							, CCode
							, CompanyName
							, SUM(IPCount) AS IPCount
							, SUM(OPCount) AS OPCount
							, SUM(IPCount+OPCount) AS TCount
							, SUM(IPGross) AS IPGross
							, SUM(IPDiscount) AS IPDiscount
							, SUM(IPDeductable) AS IPDeductable
							, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2)) AS IPNet
							, SUM(OPGross) AS OPGross
							, SUM(OPDiscount) AS OPDiscount
							, SUM(OPDeductable) AS OPDeductable
							, SUM(OPGross-OPDiscount-OPDeductable) AS OPNet
							, SUM(IPGross+OPGross) AS TGross
							, SUM(IPDiscount+OPDiscount) AS TDisc
							, SUM(IPDeductable+OPDeductable) AS TDeduc
							, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2))+SUM(OPGross-OPDiscount-OPDeductable) AS TNet
							,UPPER(@branch) branch
							,@address address
							,ROW_NUMBER() OVER(ORDER BY SubCategory) slno
						FROM #TSummary 
						where SubCategory = @SubCategoryName
						GROUP BY Subcategory,CCode, CompanyName 
						ORDER BY Subcategory,CCode, CompanyName  

		 END
		 ELSE
		 Begin

					 SELECT 
							  SubCategory 				
							, CCode
							, CompanyName
							, SUM(IPCount) AS IPCount
							, SUM(OPCount) AS OPCount
							, SUM(IPCount+OPCount) AS TCount
							, SUM(IPGross) AS IPGross
							, SUM(IPDiscount) AS IPDiscount
							, SUM(IPDeductable) AS IPDeductable
							, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2)) AS IPNet
							, SUM(OPGross) AS OPGross
							, SUM(OPDiscount) AS OPDiscount
							, SUM(OPDeductable) AS OPDeductable
							, SUM(OPGross-OPDiscount-OPDeductable) AS OPNet
							, SUM(IPGross+OPGross) AS TGross
							, SUM(IPDiscount+OPDiscount) AS TDisc
							, SUM(IPDeductable+OPDeductable) AS TDeduc
							, SUM(ROUND(IPGross-IPDiscount-IPDeductable,2))+SUM(OPGross-OPDiscount-OPDeductable) AS TNet
							,UPPER(@branch) branch
							,@address address
							,ROW_NUMBER() OVER(ORDER BY SubCategory) slno
						FROM #TSummary 
						GROUP BY Subcategory,CCode, CompanyName 
						ORDER BY Subcategory,CCode, CompanyName  


		 END
			          
		 END     


END
GO
