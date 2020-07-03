Declare @from DateTime set @from ='20-oct-2016'
Declare @to DateTime set  @to ='28-oct-2016'
Declare @patientType int set @patientType = 0
Declare @companyId int set @companyId = 0
Declare @departmentId int set @departmentId  =0
Declare @doctorId int set  @doctorId = 0
Declare @regNo int set @regNo  = 0
Declare @isRevenue int set @isRevenue  = 0
Declare @serviceId int set @serviceId  =0 
 Declare @empId int set @empId  =0 
	
	SELECT *,
		 (CASE WHEN x.PackageAmount > 0 
				THEN  x.PackageAmount - x.HISRevenue 
				ELSE 0
		   END ) *-1 HISGainLoss
		 , CASE WHEN x.PackageAmount > 0
				THEN x.HISRevenue -(x.PackageAmount - x.HISRevenue)+ x.Deductibles 
				    --+ (x.Exclusion *-1)
                ELSE x.HISRevenue - x.DiscountAmount  + x.Deductibles
		   END  HISRecievable
		 ,'Discharged' BillType
		 , CASE WHEN x.PackageAmount > 0 
				THEN 'Yes'
				ELSE 'No'
		   END PackageDealPatient
		 ,CASE 
			   WHEN ( x.HISRevenue - x.PackageAmount  ) >= 0 THEN  'Loss'
			   ELSE 'Gain' 
		  END GainLossPatient
   FROM(



			SELECT
			 Emp.FirstName +' '+ Emp.MiddleName+' ' +Emp.LastName as EmpName,Emp.Id as EmpId,a.IPID
			,CASE WHEN a.Cash = 1 THEN 'IPCS' ELSE 'IPCR' END + RIGHT('000000000' + CAST(a.SlNo AS VARCHAR(9)), 9) AS InvoiceNo 
			,REPLACE(CONVERT(VARCHAR(50), ISNULL(c.AdmitDateTime, GETDATE()), 106), ' ', '-') AdmissionDate
			,REPLACE(CONVERT(VARCHAR(50), ISNULL(c.DischargeDateTime, GETDATE()), 106), ' ', '-') DischargeDate
			,[dbo].[formatDateSGHMMMyy](c.DischargeDateTime) AS DischargeMonth
			,c.IssueAuthorityCode + '.' + RIGHT('0000000000' + CAST(c.Registrationno AS VARCHAR(10)), 10) AS PIN
			,CASE WHEN ISNULL(h.PackageAmount,0)> 0 THEN (SELECT Name from bed where ID = x.BedID)
			      ELSE j.Name 
			  END AS RoomNo
			,d.EmpCode DoctorCode	
			,g.Name  MedicalDept
			,RTRIM(UPPER(e.Code)) AS CompanyCode
			,e.Name CompanyName
			,f.ServiceName ServiceCategory
			,CASE WHEN ISNULL(h.PackageAmount,0)> 0 THEN x.ITEMCODE
			      ELSE b.ITEMCODE
			 END  AS ServiceCode
			,CASE WHEN ISNULL(h.PackageAmount,0)> 0 THEN x.ITEMNAME
			      ELSE b.ITEMNAME 
			 END AS ServiceDesc
			,REPLACE(CONVERT(VARCHAR(50), ISNULL(b.DateTime, GETDATE()), 106), ' ', '-') ServiceDate
			,[dbo].[formatDateSGHMMMyy](b.DateTime) AS ServiceMonth
			,
			CASE WHEN ISNULL(h.PackageAmount,0)> 0 THEN ISNULL(x.Quantity,0)
			      ELSE ISNULL(b.Quantity,0)
			 END Quantity
			,CASE WHEN ISNULL(h.PackageAmount,0)> 0  THEN CAST(ISNULL(x.Price, 0)AS NUMERIC(18,4))
			      ELSE CAST(ISNULL(b.Price, 0)AS NUMERIC(18,4))
			 END Rate
			,CASE WHEN ISNULL(h.PackageAmount,0)> 0 
			     THEN 
			         CASE WHEN a.BedTypeID = -1 THEN 0
					       ELSE x.Price* x.Quantity
                      END
			     ELSE 
			          CASE WHEN a.BedTypeID = -1 THEN 0
					       ELSE b.Price* b.Quantity
                      END
			 END AS  HISRevenue

			,CASE WHEN ISNULL(h.PackageAmount,0) > 0 THEN '0%'
			      ELSE CONVERT (VARCHAR(10),ROUND(CAST((ISNULL(b.discount,0) / CASE WHEN ISNULL(b.Price,0) = 0 THEN 1 ELSE b.Price END)  * 100 AS FLOAT),2))+ '%' 
			 END AS DiscountPercentage

			,CASE WHEN ISNULL(h.PackageAmount,0) > 0 THEN 0 
			      ELSE CAST((ISNULL(b.Discount,0) * ISNULL(b.Quantity,0)) AS NUMERIC(18,2)) 
			 END AS DiscountAmount

			,CASE WHEN ISNULL(h.PackageAmount,0) > 0 
				  THEN ((x.Quantity * x.Price)/h.BillAmount)* h.PackageAmount ELSE 0 
			 END PackageAmount

			,CASE WHEN ISNULL(h.PackageAmount,0) > 0
                 THEN
                    (x.ApolloPrice  * x.Quantity)*-1
                  ELSE
    		         (b.ApolloPrice  * b.Quantity)*-1
			  END AS Exclusion

			 ,CASE WHEN ISNULL(h.PackageAmount,0) > 0
			  THEN
			         x.DeductableAmount
			  ELSE 
					 b.DeductableAmount
			  END AS Deductibles

			  ,Case WHEN a.BillType = 1 then 
			      Case when ISNULL(h.PackageAmount,0) > 0 then x.Quantity * x.Price else b.price * b.Quantity end
			   else 0 end CashRevenue

			  ,Case WHEN a.BillType = 2 then 
			        Case when ISNULL(h.PackageAmount,0) > 0 then x.Quantity * x.Price else b.price * b.Quantity end
			   else 0 end ChargeRevenue

			   ,CASE When a.Cash = 1 
					then 
					CASE WHEN ISNULL(h.PackageAmount,0)> 0 
						 THEN 
							 CASE WHEN a.BedTypeID = -1 THEN 0
								   ELSE ((x.Price* x.Quantity) -  ((x.ApolloPrice  * x.Quantity)*-1))
							  END
						 ELSE 
							  CASE WHEN a.BedTypeID = -1 THEN 0
								   ELSE ((b.Price* b.Quantity) -((b.ApolloPrice  * b.Quantity)*-1))
							  END
					 END 
			   
			    else 0 end HISCashRevenue

				,CASE WHEN a.Cash <> 1 
				then
						CASE WHEN ISNULL(h.PackageAmount,0)> 0 
						 THEN 
							 CASE WHEN a.BedTypeID = -1 THEN 0
								   ELSE ((x.Price* x.Quantity) -  ((x.ApolloPrice  * x.Quantity)*-1))
							  END
						 ELSE 
							  CASE WHEN a.BedTypeID = -1 THEN 0
								   ELSE ((b.Price* b.Quantity) -((b.ApolloPrice  * b.Quantity)*-1))
							  END
					 END 
				else
				0
				end
				HISChargeRevenue

				,a.Cash as ccode
				,h.PackageAmount as ccpackamount
				,a.BedTypeID as ccbedtype
				, (x.Price* x.Quantity) revenueA
				,  (x.ApolloPrice  * x.Quantity)*-1 exclusionA
				, (b.Price* b.Quantity) revenueB
				, (b.ApolloPrice  * b.Quantity)*-1 exclusionB
				


		FROM IPBILL a
		inner JOIN ipbillitemdetail b
		ON   b.BillNo = a.BillNo
		inner JOIN OldInPatient c
		ON a.IPID = c.IPID 
	
		LEFT JOIN dbo.doctor d ON c.DoctorID = D.ID
		LEFT JOIN dbo.company e  ON a.CompanyID = e.ID
		LEFT JOIN dbo.IPBService f ON b.ServiceID = f.ID
		LEFT JOIN dbo.Department g ON g.ID = b.DepartmentID
		LEFT JOIN dbo.PackageBill h on a.BillNo = h.BillNo and b.ItemID = h.PackageItemID
		LEFT JOIN dbo.PackagebillItemDetail x on h.PackageID = x.PackageID and h.BillNo = x.BillNo
		LEFT JOIN bed j		        On b.BedID = j.ID
		LEFT join Employee Emp on Emp.ID = a.OperatorID
		WHERE (@patientType = 0 OR  a.Cash = @patientType)
		AND (@companyId = 0 OR e.ID = @companyId)
		AND (@doctorId = 0 OR D.EmployeeID = @doctorId)
		AND (@regNo = 0 OR c.RegistrationNo = @regNo)
		AND (@departmentId = 0 OR g.ID = @departmentId)
		AND (@serviceId = 0 OR f.ID= @serviceId)
		AND (@empId = 0 OR Emp.ID= @empId)
		AND  c.DischargeDateTime >= @from
		AND  c.DischargeDateTime < @to
		AND (@isRevenue = 0 OR (b.DateTime >= @from and b.DateTime <=@to))
 
    
	
	) X ORDER BY  Convert(varchar(30),x.DischargeDate,102), x.IPID, x.PIN, x.InvoiceNo


	
	----------------OP-------------------------------------------------------------------------------------------------------
	SELECT *
		 , (CASE WHEN x.PackageAmount > 0 
				THEN  x.HISRevenue - x.PackageAmount
				ELSE 0
		   END)*- 1 HISGainLoss
		 , CASE WHEN x.PackageAmount > 0
				THEN x.PackageAmount
				ELSE x.HISRevenue - x.DiscountAmount
		   END  HISRecievable
		 ,'Undischarge' BillType
		 , CASE WHEN x.PackageAmount > 0 
				THEN 'Yes'
				ELSE 'No'
		   END PackageDealPatient
		 ,CASE 
			   WHEN x.PackageAmount = 0 THEN '-'
			   WHEN x.PackageAmount > 0 AND (CASE WHEN x.PackageAmount > 0 THEN  x.HISRevenue - x.PackageAmount ELSE 0 END ) > 0 THEN  'Gain'
			   ELSE 'Loss' 
		  END GainLossPatient
   FROM(
			SELECT a.IPID
		    ,'' InvoiceNo 
			,REPLACE(CONVERT(VARCHAR(50), ISNULL(c.AdmitDateTime, GETDATE()), 106), ' ', '-') AdmissionDate
			,'' DischargeDate
			,'' DischargeMonth
			,c.IssueAuthorityCode + '.' + RIGHT('0000000000' + CAST(c.Registrationno AS VARCHAR(10)), 10) AS PIN
			,j.Name RoomNo
			,d.EmpCode DoctorCode
			,g.Name  MedicalDept
			,RTRIM(UPPER(e.Code)) AS CompanyCode
			,e.Name CompanyName
			,f.ServiceName ServiceCategory
			,b.ITEMCODE AS ServiceCode
			,b.ITEMNAME AS ServiceDesc
			,REPLACE(CONVERT(VARCHAR(50), ISNULL(b.DateTime, GETDATE()), 106), ' ', '-') ServiceDate
			,[dbo].[formatDateSGHMMMyy](b.DateTime) AS ServiceMonth
			,ISNULL(b.Quantity,0) Quantity
			,CAST(ISNULL(b.Price, 0)AS NUMERIC(18,4)) Rate
			,CAST((ISNULL(b.Price, 0) * ISNULL(b.Quantity,0))AS NUMERIC(18,2)) AS  HISRevenue
			,CONVERT (VARCHAR(10),ROUND(CAST((ISNULL(b.discount,0) / CASE WHEN ISNULL(b.Price,0) = 0 THEN 1 ELSE b.Price END)  * 100 AS FLOAT),2))+ '%' AS DiscountPercentage
			,CAST((ISNULL(b.Discount,0) * ISNULL(b.Quantity,0)) AS NUMERIC(18,2)) AS DiscountAmount
			,0  PackageAmount
			,Case WHEN ((b.ApolloPrice * b.Quantity) - (b.Quantity * b.ApolloPrice)) > 0 AND b.Price = 0 
				  THEN  ((b.ApolloPrice * b.Quantity) - (b.Quantity * b.ApolloPrice))
				  ELSE 0
		     END AS Exclusion
			,Case WHEN ((b.Price * b.Quantity) - (b.Quantity * b.ApolloPrice)) > 0 AND b.Price > 0 
				  THEN  ((b.Price * b.Quantity) - (b.Quantity * b.Discount)) -((b.ApolloPrice * b.Quantity) - (b.Quantity * b.ApolloPrice))
				  ELSE 0
			 END AS Deductibles
			 , '' as CashRevenue
			 , '' as ChargeRevenue
		FROM CurrentIPBill a

		inner JOIN CurrentIPBillItemDetail b ON a.Id = b.IPBillID 
		inner JOIN InPatient c					ON a.IPID = c.IPID
		LEFT JOIN dbo.doctor d				ON c.DoctorID = D.ID
		LEFT JOIN dbo.company e				ON a.CompanyID = e.ID
		LEFT JOIN dbo.IPBService f			ON b.ServiceID = f.ID
		LEFT JOIN dbo.Department g			ON g.ID = d.DepartmentID
		LEFT JOIN dbo.IPPackage h			ON a.IPID = h.IPID 
        LEFT JOIN dbo.Bed j				ON b.BedID = j.ID
		WHERE 
		c.AdmitDateTime >= @from
		AND  c.AdmitDateTime < @to  
		-- GET DATA EXCEPT TO DAY BECAUSE ITS NOT LOADED YET
		--AND  b.DateTime < REPLACE(CONVERT(VARCHAR(50), GETDATE(), 106), ' ', '-')
		
		AND (@patientType = 0 OR  a.Cash = @patientType)
		AND (@companyId = 0 OR e.ID = @companyId)
		AND (@doctorId = 0 OR D.EmployeeID = @doctorId)
		AND (@regNo = 0 OR c.RegistrationNo = @regNo)
		AND (@departmentId = 0 OR g.ID = @departmentId)
		AND ((@serviceId = 0 and ISNULL(B.ServiceID,0) >  0 )OR f.ID= @serviceId)
		AND (@isRevenue = 0 OR (b.DateTime >= @from and b.DateTime <=@to))
		
    ) X ORDER BY  Convert(varchar(30),x.AdmissionDate,102), x.IPID, x.PIN