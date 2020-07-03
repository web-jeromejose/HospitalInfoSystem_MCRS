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
alter PROCEDURE [MCRS].[ARReports_GetYearlyTotalSummaryAccounts]
 (
	@startdate DATETIME = '2013-01-01',
	@endate DATETIME = '2013-03-01',
 	@yearDate DATETIME = '2013-01-01'
		,@CategoryIds INT = '76' --74
		,@companyId INT = '0'
		, @gradeId INT = '0'
		,@option varchar(100) = 'monthly'
 )
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

		IF OBJECT_ID('tempdb.dbo.#TSummaryNet' , 'U') IS NOT NULL
		DROP TABLE #TSummaryNet

		IF OBJECT_ID('tempdb.dbo.##tmptablemonthly' , 'U') IS NOT NULL
		DROP TABLE ##tmptablemonthly

		
		IF OBJECT_ID('tempdb.dbo.##tmptablemonthly12' , 'U') IS NOT NULL
		DROP TABLE ##tmptablemonthly12
		 
		IF OBJECT_ID('tempdb.dbo.#temptblVisitIds' , 'U') IS NOT NULL
		drop TABLE #temptblVisitIds

		IF OBJECT_ID('tempdb.dbo.#temptblVisitIds1' , 'U') IS NOT NULL
		drop TABLE #temptblVisitIds1


		IF OBJECT_ID('tempdb.dbo.#tempIds' , 'U') IS NOT NULL
		drop TABLE #tempIds



			SELECT id into #tempIds FROM Category WHERE DELETED = 0 ORDER BY name

			DECLARE @EmployeeList varchar(MAX)
			DECLARE @EmployeeListObj varchar(MAX)

			SELECT @EmployeeList = COALESCE(@EmployeeList + ', ', '') + 
			CAST(id AS varchar(5))
			FROM #tempIds

			set @EmployeeListObj = '['+@EmployeeList+']'

		
		

		
		DECLARE @tempMonthlywithId AS TABLE(TotalNet INT ,Date DateTime,Company Varchar(255),Id INT)
	
	if(@option = 'yearly')
	Begin
		
				--select @EmployeeListObj
				SELECT  element_id AS Id, StringValue AS VisitId  into #temptblVisitIds FROM fn_parseJSON(@EmployeeListObj) WHERE StringValue != ''
				DECLARE @rowCount AS INT
				DECLARE @counter AS INT
				DECLARE @visitCategoryId AS BIGINT
		 
				SELECT @rowCount = Count(*) FROM   #temptblVisitIds
				SET @counter = 1;

				
				CREATE TABLE ##tmptablemonthly 
				(
				IPCount			BIGINT, 
				OPCount			BIGINT, 
				Tcount			BIGINT, 
				TotalNet			BIGINT, 
				Date nvarchar(MAX),
				Company Varchar(MAX)
				)

					WHILE (@counter <= @rowCount)
						BEGIN
										SELECT  @visitCategoryId  =  VisitId FROM #temptblVisitIds WHERE Id =  @counter
		 	
	
										DECLARE @StartDT DATETIME
										SET @StartDT =  @yearDate

										WHILE @StartDT < DATEADD(yy, DATEDIFF(yy,0,@yearDate) + 1, -1) 
										BEGIN
											 declare @Startdate1 date set @Startdate1 = dateadd(month,datediff(month,0,CONVERT(VARCHAR,@StartDT)),0) 
											 DECLARE @enddate1 DATETIME set @enddate1 = dateadd(day,+1,dateadd(day,-1,dateadd(month,datediff(month,-1,CONVERT(VARCHAR,@StartDT)),0)))
		 									--SELECT   @Startdate1,@visitCategoryId,@companyId,@gradeId  
											insert into ##tmptablemonthly -- @tempMonthly 
											  EXEC [MCRS].[ARReports_GetMonthlyNetIncome] @Startdate1,@visitCategoryId,@companyId,@gradeId  
											SET @StartDT = DATEADD(MONTH,1,@StartDT)
										END
						SET @counter = @counter+1;
						END


--select * from	@tempMonthly
	DECLARE @tempYearly AS Table(Id int identity,Company Varchar(255)
	,Jan_tcount int,Jan_ipcount int,Jan_opcount int,Jan_totalnet int
	,Feb_tcount int,Feb_ipcount int,Feb_opcount int,Feb_totalnet int
	,Mar_tcount int,Mar_ipcount int,Mar_opcount int,Mar_totalnet int
	,Apr_tcount int,Apr_ipcount int,Apr_opcount int,Apr_totalnet int
	,May_tcount int,May_ipcount int,May_opcount int,May_totalnet int
	,Jun_tcount int,Jun_ipcount int,Jun_opcount int,Jun_totalnet int
	,Jul_tcount int,Jul_ipcount int,Jul_opcount int,Jul_totalnet int
	,Aug_tcount int,Aug_ipcount int,Aug_opcount int,Aug_totalnet int
	,Sep_tcount int,Sep_ipcount int,Sep_opcount int,Sep_totalnet int
	,Oct_tcount int,Oct_ipcount int,Oct_opcount int,Oct_totalnet int
	,Nov_tcount int,Nov_ipcount int,Nov_opcount int,Nov_totalnet int
	,Dec_tcount int,Dec_ipcount int,Dec_opcount int,Dec_totalnet int
	 )

	 --deleted null values
	 delete from ##tmptablemonthly  where Company IS NULL

DECLARE @tempCompanywithId AS TABLE( CompanyId int identity,Company Varchar(200))
 insert into @tempCompanywithId
 select distinct(company) from ##tmptablemonthly

 DECLARE @rowCount1 AS INT
		DECLARE @counter1 AS INT
		DECLARE @visitCategoryId1 AS BIGINT
		 
			SELECT @rowCount1 =  count(*) from @tempCompanywithId
				SET @counter1 = 1;

WHILE (@counter1 <= @rowCount1)
	BEGIN
	--select * from @tempCompanywithId where CompanyId =  @counter1

	
insert into @tempYearly (Company,Jan_tcount,Jan_ipcount,Jan_opcount,Jan_totalnet)
select a.Company,a.Tcount,a.IPCount, a.OPCount,a.TotalNet from ##tmptablemonthly a 
where a.Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'January'

update @tempYearly 
SET Feb_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'February' )
,Feb_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'February' )
,Feb_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'February' )
,Feb_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'February' )
 where Id = @counter1

 update @tempYearly 
SET Mar_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'March' )
,Mar_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'March' )
,Mar_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'March' )
,Mar_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'March' )
 where Id = @counter1

  update @tempYearly 
SET Apr_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'April' )
,Apr_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'April' )
,Apr_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'April' )
,Apr_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'April' )
 where Id = @counter1


  update @tempYearly 
SET May_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'May' )
,May_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'May' )
,May_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'May' )
,May_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'May' )
 where Id = @counter1

  update @tempYearly 
SET Jun_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'June' )
,Jun_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'June' )
,Jun_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'June' )
,Jun_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'June' )
 where Id = @counter1

  update @tempYearly 
SET Jul_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'July' )
,Jul_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'July' )
,Jul_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'July' )
,Jul_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'July' )
 where Id = @counter1

  update @tempYearly 
SET Aug_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'August' )
,Aug_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'August' )
,Aug_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'August' )
,Aug_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'August' )
 where Id = @counter1

  update @tempYearly 
SET Sep_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'September' )
,Sep_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'September' )
,Sep_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'September' )
,Sep_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'September' )
 where Id = @counter1

  update @tempYearly 
SET Oct_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'October' )
,Oct_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'October' )
,Oct_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'October' )
,Oct_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'October' )
 where Id = @counter1

  update @tempYearly 
SET Nov_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'November' )
,Nov_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'November' )
,Nov_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'November' )
,Nov_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'November' )
 where Id = @counter1

  update @tempYearly 
SET Dec_ipcount = (select IPCount from ##tmptablemonthly a  where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'December' )
,Dec_opcount  = (select OPCount  from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'December' )
,Dec_tcount = (select Tcount   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'December' )
,Dec_totalnet = (select TotalNet   from ##tmptablemonthly a where Company =  (select Company from @tempCompanywithId where CompanyId =  @counter1) and a.Date = 'December' )
 where Id = @counter1

	
	SET @counter1 = @counter1+1;
	END

	select * from @tempYearly
   
	END --DONE YEARLY option



if(@option = 'monthly')
Begin

			DECLARE @tempMonthly AS TABLE( Id int identity,IPCount int, OPCount int, Tcount int, TotalNet INT ,Date nvarchar(200),Company Varchar(255))

				SELECT  element_id AS Id, StringValue AS VisitId  into #temptblVisitIds1 FROM fn_parseJSON('[76,74]') WHERE StringValue != ''
				DECLARE @rowCount12 AS INT
				DECLARE @counter12 AS INT
				DECLARE @visitCategoryId12 AS BIGINT
		 
				SELECT @rowCount12 = Count(*) FROM   #temptblVisitIds1
				SET @counter12 = 1;

				
				CREATE TABLE ##tmptablemonthly12 
				(
				IPCount			BIGINT, 
				OPCount			BIGINT, 
				Tcount			BIGINT, 
				TotalNet			BIGINT, 
				Date nvarchar(MAX),
				Company Varchar(MAX)
				)  
				 
					WHILE (@counter12 <= @rowCount12)
						BEGIN
								declare @Startdate2 date set @Startdate2 = dateadd(month,datediff(month,0,CONVERT(VARCHAR,@startdate)),0) 
								DECLARE @enddate2 DATETIME set @enddate2 = dateadd(day,+1,dateadd(day,-1,dateadd(month,datediff(month,-1,CONVERT(VARCHAR,@endate)),0)))
			
										SELECT  @visitCategoryId12  =  VisitId FROM #temptblVisitIds1 WHERE Id =  @counter12
										WHILE @Startdate2 < @enddate2 
										BEGIN
										 
											insert into @tempMonthly 
											EXEC [MCRS].[ARReports_GetMonthlyNetIncome] @Startdate2,@visitCategoryId12,@companyId,@gradeId  
											SET @Startdate2 = DATEADD(MONTH,1,@Startdate2)
										END
						SET @counter12 = @counter12 +1;
						END
									--   [MCRS].[ARReports_GetYearlyTotalSummaryAccounts]
						

						DECLARE @tempAllMonthly AS Table(Id int identity,Company Varchar(255)
	,Jan_tcount int,Jan_ipcount int,Jan_opcount int,Jan_totalnet int
	,Feb_tcount int,Feb_ipcount int,Feb_opcount int,Feb_totalnet int
	,Mar_tcount int,Mar_ipcount int,Mar_opcount int,Mar_totalnet int
	,Apr_tcount int,Apr_ipcount int,Apr_opcount int,Apr_totalnet int
	,May_tcount int,May_ipcount int,May_opcount int,May_totalnet int
	,Jun_tcount int,Jun_ipcount int,Jun_opcount int,Jun_totalnet int
	,Jul_tcount int,Jul_ipcount int,Jul_opcount int,Jul_totalnet int
	,Aug_tcount int,Aug_ipcount int,Aug_opcount int,Aug_totalnet int
	,Sep_tcount int,Sep_ipcount int,Sep_opcount int,Sep_totalnet int
	,Oct_tcount int,Oct_ipcount int,Oct_opcount int,Oct_totalnet int
	,Nov_tcount int,Nov_ipcount int,Nov_opcount int,Nov_totalnet int
	,Dec_tcount int,Dec_ipcount int,Dec_opcount int,Dec_totalnet int
	 )

	 --deleted null values
	 delete from @tempMonthly  where Company IS NULL

DECLARE @tempCompanywithIdpermonth AS TABLE( CompanyId int identity,Company Varchar(200))
 insert into @tempCompanywithIdpermonth
 select distinct(company) from @tempMonthly

 DECLARE @rowCounter1 AS INT
		DECLARE @countermonthly AS INT
		DECLARE @visitCategoryIdmonth AS BIGINT
		 
			SELECT @rowCounter1 =  count(*) from @tempCompanywithIdpermonth
				SET @countermonthly = 1;

WHILE (@countermonthly <= @rowCounter1)
	BEGIN
	--select * from @tempCompanywithIdpermonth where CompanyId =  @countermonthly

	
insert into @tempAllMonthly (Company,Jan_tcount,Jan_ipcount,Jan_opcount,Jan_totalnet)
select a.Company,a.Tcount,a.IPCount, a.OPCount,a.TotalNet from @tempMonthly a 
where a.Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'January'

update @tempAllMonthly 
SET Feb_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'February' )
,Feb_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'February' )
,Feb_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'February' )
,Feb_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'February' )
 where Id = @countermonthly

 update @tempAllMonthly 
SET Mar_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'March' )
,Mar_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'March' )
,Mar_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'March' )
,Mar_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'March' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Apr_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'April' )
,Apr_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'April' )
,Apr_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'April' )
,Apr_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'April' )
 where Id = @countermonthly


  update @tempAllMonthly 
SET May_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'May' )
,May_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'May' )
,May_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'May' )
,May_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'May' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Jun_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'June' )
,Jun_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'June' )
,Jun_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'June' )
,Jun_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'June' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Jul_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'July' )
,Jul_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'July' )
,Jul_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'July' )
,Jul_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'July' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Aug_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'August' )
,Aug_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'August' )
,Aug_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'August' )
,Aug_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'August' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Sep_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'September' )
,Sep_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'September' )
,Sep_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'September' )
,Sep_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'September' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Oct_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'October' )
,Oct_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'October' )
,Oct_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'October' )
,Oct_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'October' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Nov_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'November' )
,Nov_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'November' )
,Nov_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'November' )
,Nov_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'November' )
 where Id = @countermonthly

  update @tempAllMonthly 
SET Dec_ipcount = (select IPCount from @tempMonthly a  where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'December' )
,Dec_opcount  = (select OPCount  from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'December' )
,Dec_tcount = (select Tcount   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'December' )
,Dec_totalnet = (select TotalNet   from @tempMonthly a where Company =  (select Company from @tempCompanywithIdpermonth where CompanyId =  @countermonthly) and a.Date = 'December' )
 where Id = @countermonthly

	
	SET @countermonthly = @countermonthly+1;
	END

	select * from @tempAllMonthly



END









--declare @ctr int set @ctr = 1
--declare @ctrAll int 
--		set @ctrAll = (select count(*) from @tempMonthly)

--		 insert into @tempYearly (Company)  (Select Company from @tempMonthly where Id = @ctr )  
--		while @ctrAll >=  @ctr
--		begin 
		 
--		select * from @tempMonthly where id = @ctr 

--	set @ctr = @ctr + 1
--	end

END
GO
