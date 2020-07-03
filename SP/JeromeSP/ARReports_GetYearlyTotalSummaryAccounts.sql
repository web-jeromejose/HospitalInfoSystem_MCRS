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

 	@startAndEndDate DATETIME = '2013-01-01'
		,@Category INT = '76' --74
		,@companyId INT = '0'
		, @gradeId INT = '0'
		,@option varchar(100) = 'yearly'
 )
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

		IF OBJECT_ID('tempdb.dbo.#TSummaryNet' , 'U') IS NOT NULL
		DROP TABLE #TSummaryNet

		IF OBJECT_ID('tempdb.dbo.#tmpTableMonthly' , 'U') IS NOT NULL
		DROP TABLE #tmpTableMonthly

		DECLARE @tempMonthly AS TABLE( Id int identity,IPCount int, OPCount int, Tcount int, TotalNet INT ,Date nvarchar(200),Company Varchar(255))
		DECLARE @tempMonthlywithId AS TABLE(TotalNet INT ,Date DateTime,Company Varchar(255),Id INT)
	
	if(@option = 'yearly')
	Begin
		DECLARE @StartDT DATETIME
		SET @StartDT =  @startAndEndDate
		 

		WHILE @StartDT < DATEADD(yy, DATEDIFF(yy,0,@startAndEndDate) + 1, -1) 
		BEGIN
			 declare @Startdate1 date set @Startdate1 = dateadd(month,datediff(month,0,CONVERT(VARCHAR,@StartDT)),0) 
			 DECLARE @enddate1 DATETIME set @enddate1 = dateadd(day,+1,dateadd(day,-1,dateadd(month,datediff(month,-1,CONVERT(VARCHAR,@StartDT)),0)))	
			 insert into @tempMonthly
			   EXEC [MCRS].[ARReports_GetMonthlyNetIncome] @Startdate1,@Category,@companyId,@gradeId  
			SET @StartDT = DATEADD(MONTH,1,@StartDT)
		END


	END



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
	

select * from @tempMonthly

-- [MCRS].[ARReports_GetYearlyTotalSummaryAccounts]

--START WORKING

--insert into @tempYearly (Company,Jan_tcount,Jan_ipcount,Jan_opcount,Jan_totalnet)
--select Company,Tcount,IPCount, OPCount,TotalNet from @tempMonthly where Id = 1

--update @tempYearly 
--SET Feb_ipcount = (select IPCount from @tempMonthly where Id = 2)
--,Feb_opcount  = (select OPCount from @tempMonthly where Id = 2)
--,Feb_tcount = (select Tcount from @tempMonthly where Id = 2)
--,Feb_totalnet = (select TotalNet from @tempMonthly where Id = 2)
-- where Id = 1

--update @tempYearly 
--SET Mar_ipcount = (select IPCount from @tempMonthly where Id = 3)
--,Mar_opcount  = (select OPCount from @tempMonthly where Id = 3)
--,Mar_tcount = (select Tcount from @tempMonthly where Id = 3)
--,Mar_totalnet = (select TotalNet from @tempMonthly where Id = 3)
-- where Id = 1

-- update @tempYearly 
--SET Apr_ipcount = (select IPCount from @tempMonthly where Id = 4)
--,Apr_opcount  = (select OPCount from @tempMonthly where Id = 4)
--,Apr_tcount = (select Tcount from @tempMonthly where Id = 4)
--,Apr_totalnet = (select TotalNet from @tempMonthly where Id = 4)
-- where Id = 1


-- update @tempYearly 
--SET May_ipcount = (select IPCount from @tempMonthly where Id = 5)
--,May_opcount  = (select OPCount from @tempMonthly where Id = 5)
--,May_tcount = (select Tcount from @tempMonthly where Id = 5)
--,May_totalnet = (select TotalNet from @tempMonthly where Id =5)
-- where Id = 1


-- update @tempYearly 
--SET Jun_ipcount = (select IPCount from @tempMonthly where Id = 6)
--,Jun_opcount  = (select OPCount from @tempMonthly where Id = 6)
--,Jun_tcount = (select Tcount from @tempMonthly where Id = 6)
--,Jun_totalnet = (select TotalNet from @tempMonthly where Id = 6)
-- where Id = 1


-- update @tempYearly 
--SET Jul_ipcount = (select IPCount from @tempMonthly where Id = 7)
--,Jul_opcount  = (select OPCount from @tempMonthly where Id = 7)
--,Jul_tcount = (select Tcount from @tempMonthly where Id = 7)
--,Jul_totalnet = (select TotalNet from @tempMonthly where Id = 7)
-- where Id = 1


-- update @tempYearly 
--SET Aug_ipcount = (select IPCount from @tempMonthly where Id = 8)
--,Aug_opcount  = (select OPCount from @tempMonthly where Id = 8)
--,Aug_tcount = (select Tcount from @tempMonthly where Id = 8)
--,Aug_totalnet = (select TotalNet from @tempMonthly where Id = 8)
-- where Id = 1


-- update @tempYearly 
--SET Sep_ipcount = (select IPCount from @tempMonthly where Id = 9)
--,Sep_opcount  = (select OPCount from @tempMonthly where Id = 9)
--,Sep_tcount = (select Tcount from @tempMonthly where Id = 9)
--,Sep_totalnet = (select TotalNet from @tempMonthly where Id = 9)
-- where Id = 1


-- update @tempYearly 
--SET Oct_ipcount = (select IPCount from @tempMonthly where Id = 10)
--,Oct_opcount  = (select OPCount from @tempMonthly where Id = 10)
--,Oct_tcount = (select Tcount from @tempMonthly where Id = 10)
--,Oct_totalnet = (select TotalNet from @tempMonthly where Id = 10)
-- where Id = 1


-- update @tempYearly 
--SET Nov_ipcount = (select IPCount from @tempMonthly where Id = 11)
--,Nov_opcount  = (select OPCount from @tempMonthly where Id = 11)
--,Nov_tcount = (select Tcount from @tempMonthly where Id = 11)
--,Nov_totalnet = (select TotalNet from @tempMonthly where Id = 11)
-- where Id = 1


-- update @tempYearly 
--SET Dec_ipcount = (select IPCount from @tempMonthly where Id = 12)
--,Dec_opcount  = (select OPCount from @tempMonthly where Id = 12)
--,Dec_tcount = (select Tcount from @tempMonthly where Id = 12)
--,Dec_totalnet = (select TotalNet from @tempMonthly where Id = 12)
-- where Id = 1


--select * from @tempYearly

--END WORKING	 

--   [MCRS].[ARReports_GetYearlyTotalSummaryAccounts]


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
