USE [HIS]
GO
/****** Object:  StoredProcedure [MCRS].[SalesPromotionReport_IncomeHistorical]    Script Date: 10/2/2016 1:05:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<[dbo].[SP_SalesPromotionIncomeCensus_Historical],>
-- =============================================
ALTER PROCEDURE [MCRS].[SalesPromotionReport_IncomeHistorical] --'2016-Oct-01','2016-Oct-03','0'
(@stDate datetime,@enDate datetime,@mhid int )                        

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
                   
Create Table #SalesPromotionIncomeCensus(Trans_Date nvarchar(10),                      
   code nvarchar(10),name nvarchar(100),                  
   x1 nvarchar(10), x2 nvarchar(50),                  
   y1 nvarchar(10), y2 nvarchar(50),                  
   trans_total int, total_disc numeric(10,2),                      
   total_amount numeric(10,2))                      
             
Create Index #SalesPromotion ON #SalesPromotionIncomeCensus([code],[Trans_Date])                    
                      
Create Table #SalesPromotionIncomeCensus_Summary(xYear nvarchar(4),code int,name nvarchar(50),                      
 Jan_Total int, Jan_Discount numeric(12,2), Jan_Amount numeric(12,2),                      
 Feb_Total int, Feb_Discount numeric(12,2), Feb_Amount numeric(12,2),                      
 Mar_Total int, Mar_Discount numeric(12,2), Mar_Amount numeric(12,2),                      
 Apr_Total int, Apr_Discount numeric(12,2), Apr_Amount numeric(12,2),                      
 May_Total int, May_Discount numeric(12,2), May_Amount numeric(12,2),                      
 Jun_Total int, Jun_Discount numeric(12,2), Jun_Amount numeric(12,2),                      
 Jul_Total int, Jul_Discount numeric(12,2), Jul_Amount numeric(12,2),                      
 Aug_Total int, Aug_Discount numeric(12,2), Aug_Amount numeric(12,2),                      
 Sep_Total int, Sep_Discount numeric(12,2), Sep_Amount numeric(12,2),                      
 Oct_Total int, Oct_Discount numeric(12,2), Oct_Amount numeric(12,2),                      
 Nov_Total int, Nov_Discount numeric(12,2), Nov_Amount numeric(12,2),                      
 Dec_Total int, Dec_Discount numeric(12,2), Dec_Amount numeric(12,2),                      
 ToDate_Total int, ToDate_Discount numeric(12,2), ToDate_Amount numeric(12,2),                      
 Grand_Total int, Grand_Discount numeric(12,2), Grand_Amount numeric(12,2))                      
                      
--Delete from SalesPromotion                      
                      
--insert into SalesPromotion                      
--select * from OPENROWSET('MSDASQL',                        
-- 'DRIVER={SQL Server};SERVER=130.1.4.215;UID=smsdev;PWD=smsdev',                        
-- CORPORATE_IT.dbo.salespromotion) as f                      
                      
                      
--Delete from MOH                      
                      
--Insert Into MOH                      
--select * from OPENROWSET('MSDASQL',                        
-- 'DRIVER={SQL Server};SERVER=130.1.4.215;UID=smsdev;PWD=smsdev',                        
-- CORPORATE_IT.dbo.MOH) as f                      
               
               Insert Into #SalesPromotionIncomeCensus                        
 select convert(nvarchar(10),convert(datetime,convert(nvarchar,a.billdatetime,105),105),121) TRANS_DATE,                      
 f.mohid code,                      
 g.mohname name,                  
 e.empcode x1,                    
 e.name x2,                  
 d.deptcode y1,                    
 d.name y2,                  
 count(distinct a.registrationno) TRANS_TOTAL,                        
 Sum(a.Discount) TOTAL_DISC, Sum(a.billamount) TOTAL_AMT                        
 from opcompanybilldetail a                      
 left join opdiscountdetail b on a.opbillid = b.opbillid                       
 left join opbill c on a.opbillid = c.id                       
 left join employee e on a.doctorid = e.id                       
 left join department d on e.departmentid = d.id                       
 left join SalesPromotion f                       
 on a.registrationno = f.pin                        
 left join MOH g                     
 on f.mohid = g.id                        
 where                         
 a.billdatetime >= @stDate         
 and a.billdatetime < @enDate                        
 and b.opdiscounttype in (19,20)                         
 and a.serviceid in (3,2,5,7,18)    
 --and a.discount > 0              
 and d.deptcode is not null              
 and f.mohid is not null                         
 GROUP BY convert(nvarchar(10),convert(datetime,convert(nvarchar,a.billdatetime,105),105),121),f.mohid,g.mohname,                  
 e.empcode , e.name , d.deptcode ,   d.name 
 
                     
--Select * from #SalesPromotionIncomeCensus                      
                      
Insert into #SalesPromotionIncomeCensus_Summary(xYear,code,name)                      
select left(trans_date,4),code,name from #SalesPromotionIncomeCensus                      
group by left(trans_date,4),code,name                      
                      
--select * from #SalesPromotionIncomeCensus_Summary                      
--                      
--                      
--                      
--select *--convert(datetime,trans_date,105),*                      
--from #SalesPromotionIncomeCensus                      
                      
update #SalesPromotionIncomeCensus_Summary                      
set Jan_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-01%'),                      
    Jan_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-01%'),                      
    Jan_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-01%'),                     
    Feb_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-02%'),                    
    Feb_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-02%'),                      
    Feb_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus         
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-02%'),                      
    Mar_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-03%'),                    
    Mar_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-03%'),                      
    Mar_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-03%'),                      
    Apr_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-04%'),                    
    Apr_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-04%'),                      
    Apr_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-04%'),                      
    May_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-05%'),                    
    May_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-05%'),                      
    May_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-05%'),                      
    Jun_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-06%'),          
    Jun_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-06%'),                      
    Jun_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-06%'),                      
    Jul_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-07%'),                    
    Jul_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-07%'),                      
    Jul_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-07%'),                      
    Aug_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-08%'),                    
    Aug_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-08%'),                      
    Aug_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-08%'),                      
    Sep_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-09%'),                    
    Sep_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-09%'),                      
    Sep_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-09%'),                      
    Oct_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-10%'),                    
    Oct_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-10%'),                      
    Oct_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-10%'),                      
    Nov_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-11%'),                    
    Nov_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-11%'),                      
    Nov_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-11%'),                      
    Dec_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                
 Where code = #SalesPromotionIncomeCensus_Summary.code                      
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-12%'),                    
    Dec_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-12%'),                      
    Dec_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 Where code = #SalesPromotionIncomeCensus_Summary.code                       
    and trans_date like #SalesPromotionIncomeCensus_Summary.xyear + '-12%'),                      
    ToDate_Total = (Select isnull(sum(trans_total),0) from #SalesPromotionIncomeCensus                      
 --Where code = #SalesPromotionIncomeCensus_Summary.code                       
 --   and convert(datetime,trans_date,110) >= convert(datetime,'01-jan-' + #SalesPromotionIncomeCensus_Summary.xyear,105) and                       
 --       convert(datetime,trans_date,110) < convert(datetime,@toDate + #SalesPromotionIncomeCensus_Summary.xyear,105)),                    
 --   ToDate_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                      
 --Where code = #SalesPromotionIncomeCensus_Summary.code                      
 --   and convert(datetime,trans_date,110) >= convert(datetime,'01-jan-' + #SalesPromotionIncomeCensus_Summary.xyear,105) and                       
 --       convert(datetime,trans_date,110) < convert(datetime,@toDate + #SalesPromotionIncomeCensus_Summary.xyear,105)),                      
 --   ToDate_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                      
 --Where code = #SalesPromotionIncomeCensus_Summary.code                       
 --   and convert(datetime,trans_date,110) >= convert(datetime,'01-jan-' + #SalesPromotionIncomeCensus_Summary.xyear,105) and                       
 --       convert(datetime,trans_date,110) < convert(datetime,@toDate + #SalesPromotionIncomeCensus_Summary.xyear,105))                      
                      
 Where code = #SalesPromotionIncomeCensus_Summary.code),                 
    --and convert(datetime,trans_date,110) >= convert(datetime,'01-jan-' + #SalesPromotionIncomeCensus_Summary.xyear,110) and                       
        --convert(datetime,trans_date,110) < convert(datetime,@toDate + #SalesPromotionIncomeCensus_Summary.xyear,110)),                    
    ToDate_Discount = (Select isnull(sum(total_disc),0) from #SalesPromotionIncomeCensus                   
 Where code = #SalesPromotionIncomeCensus_Summary.code),              
    --and convert(datetime,trans_date,110) >= convert(datetime,'01-jan-' + #SalesPromotionIncomeCensus_Summary.xyear,110) and                       
        --convert(datetime,trans_date,110) < convert(datetime,@toDate + #SalesPromotionIncomeCensus_Summary.xyear,110)),                      
    ToDate_Amount = (Select isnull(sum(total_amount),0) from #SalesPromotionIncomeCensus                   
 Where code = #SalesPromotionIncomeCensus_Summary.code                      
    --and convert(datetime,trans_date,110) >= convert(datetime,'01-jan-' + #SalesPromotionIncomeCensus_Summary.xyear,110) and                       
    --    convert(datetime,trans_date,110) < convert(datetime,@toDate + #SalesPromotionIncomeCensus_Summary.xyear,110)
     )                 
Update #SalesPromotionIncomeCensus_Summary                      
set Grand_Total = Jan_total + Feb_total + Mar_total + Apr_total + May_total + Jun_total +                       
      Jul_total + Aug_total + Sep_total + Oct_total + Nov_total + Dec_total,                    
    Grand_Discount = Jan_discount + Feb_discount + Mar_discount + Apr_discount + May_discount + Jun_discount +                       
      Jul_discount + Aug_discount + Sep_discount + Oct_discount + Nov_discount + Dec_discount,                       
    Grand_Amount = Jan_Amount + Feb_Amount + Mar_Amount + Apr_Amount + May_Amount + Jun_Amount +                       
      Jul_Amount + Aug_Amount + Sep_Amount + Oct_Amount + Nov_Amount + Dec_Amount                       
        
Truncate Table AASalesPromotionIncomeCensus_Historical        
        
Insert Into AASalesPromotionIncomeCensus_Historical                      
select * from #SalesPromotionIncomeCensus_Summary               
              
select * from #SalesPromotionIncomeCensus_Summary                     



END
