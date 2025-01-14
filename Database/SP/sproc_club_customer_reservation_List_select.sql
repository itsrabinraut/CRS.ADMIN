USE [CRS.UAT_V2]
GO
/****** Object:  StoredProcedure [dbo].[sproc_club_customer_reservation_List_select]    Script Date: 5/17/2024 4:54:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sproc_club_customer_reservation_List_select]
   @plan varchar(10)=null
   ,@paymentMethodId varchar(10)=null
  ,@FromDate varchar(20)=null
  ,@ToDate varchar(20)=null
  ,@NoOfPeople varchar(20)=null
  ,@Search nvarchar(200)=null
  ,@ReservationId varchar(20)=null
  ,@ClubId varchar(20)
   ,@Row Varchar(50) =	0
   ,@Fetch varchar(50) =	10
AS
BEGIN 
declare  @sqlCondition nvarchar(max)='',  @sql nvarchar(max)='',@TotalRecords varchar(MAx)=0,@sqlFetchQuery nvarchar(max)=''
	SET NOCOUNT ON;
 CREATE TABLE #ResultTable (
    UpdatedDate datetime,
	CreatedDate datetime,
	invoiceId varchar(20),
    VisitDateTime DATETIME,
	VisitDate DATE,
    NickName NVARCHAR(256),
    NoOfPeople INT,
    PlanName NVARCHAR(100),
	PlanId VARCHAR(10),
    ReservationId varchar(20),
    Price DECIMAL(18, 2),
    HostNames NVARCHAR(MAX) -- Adjust the size according to your requirements
	,paymentMethod NVARCHAR(50)
	,paymentMethodId VARCHAR(5)
	,Status varchar(15)
	,MobileNumber varchar(20)
);
INSERT INTO #ResultTable (UpdatedDate,CreatedDate,invoiceId,VisitDateTime,VisitDate, NickName, NoOfPeople, PlanName, PlanId,ReservationId, Price, HostNames,paymentMethod,paymentMethodId,Status,MobileNumber)
SELECT 
    (select top(1) b1.TriggerActionLocalDate
     from tbl_reservation_detail a1 
     left join tbl_reservation_detail_audit b1 on b1.sno = a1.ReservationId and b1.TriggerAction = 'Update' 
     where a1.ReservationId =b.reservationid  order by 1 desc) UpdatedDate
	 ,(select top(1) b1.TriggerActionLocalDate
     from tbl_reservation_detail a1 
     left join tbl_reservation_detail_audit b1 on b1.sno = a1.ReservationId and b1.TriggerAction = 'Insert' 
     where a1.sno =b.reservationid  order by 1 asc) CreatedDate
	,invoiceId
  ,CONCAT(VisitDate, ' ', VisitTime) AS VisitDateTime
  ,VisitDate 
   ,a.FullName AS NickName,
    NoOfPeople,
    d.PlanName,
	d.PlanId,
    cast(a.ReservationId as varchar(20)) ReservationId,
    d.Price * NoOfPeople as Price,
    STUFF((SELECT ', ' + c.HostName
              FROM dbo.tbl_reservation_detail AS a_inner
              INNER JOIN dbo.tbl_reservation_host_detail AS b_inner ON b_inner.ReservationId = a_inner.ReservationId
              INNER JOIN dbo.tbl_host_details AS c ON c.HostId = b_inner.HostId
                                                   AND c.AgentId = a_inner.ClubId
              WHERE a_inner.ReservationId = b.ReservationId
              FOR XML PATH('')), 1, 2, '') AS HostNames
   ,sd.StaticDataLabel  paymentMethod 
   ,b.PaymentType
    , CASE 
        WHEN  (
		      (CASE WHEN ISNULL(b.ManualRemarkId, '') <> '' THEN b.IsManual  END) = 'Y' OR 
		      (CASE WHEN ISNULL(b.ManualRemarkId, '') = '' THEN  b.OTPVerificationStatus END) = 'A' 
			  ) 
			  AND b.TransactionStatus = 'S'  THEN 'Confirmed' 
         WHEN (
		      (CASE WHEN ISNULL(b.ManualRemarkId, '') <> '' THEN b.IsManual  END) = 'Y' OR 
		      (CASE WHEN ISNULL(b.ManualRemarkId, '') = '' THEN  b.OTPVerificationStatus END) = 'P' 
			  ) 
		    AND b.TransactionStatus = 'C' THEN 'Canceled' 
        ELSE 'Pending' 
    END AS Status
   ,c.MobileNumber
FROM dbo.tbl_customer_reservation_otp a WITH (NOLOCK)
INNER JOIN dbo.tbl_reservation_detail b WITH (NOLOCK) ON b.ReservationId = a.ReservationId
INNER JOIN dbo.tbl_reservation_plan_detail d WITH (NOLOCK) ON d.ReservationId = b.ReservationId
                                                             AND d.PlanDetailId = b.PlanDetailId
left JOIN dbo.tbl_static_data sd WITH (NOLOCK) ON b.PaymentType = sd.StaticDataValue and StaticDataType=10
INNER JOIN  tbl_customer  as c WITH (NOLOCK) on c.AgentId=b.CustomerId                                                            
WHERE ClubId=@ClubId 





IF @plan IS NOT NULL
    SET @sqlCondition = @sqlCondition + ' AND PlanId = ''' + @plan + ''' '

IF @paymentMethodId IS NOT NULL
    SET @sqlCondition = @sqlCondition + ' AND paymentMethodId = ''' + @paymentMethodId + ''' '

IF @ReservationId IS NOT NULL
	
    SET @sqlCondition = @sqlCondition + ' AND ReservationId = ''' + @ReservationId + ''' '
		

IF @NoOfPeople IS NOT NULL
    SET @sqlCondition = @sqlCondition + ' AND NoOfPeople = ' + @NoOfPeople + ' '

IF @Search IS NOT NULL
BEGIN
    SET @sqlCondition = @sqlCondition + ' AND (HostNames LIKE N''%' + REPLACE(@Search, ',', '%') + '%'' OR cast(invoiceId as varchar(50)) LIKE ''%' + REPLACE(@Search, ',', '%') + '%''   OR  NickName LIKE N''%' + REPLACE(@Search, ',', '%') + '%''  OR  MobileNumber LIKE N''%' + REPLACE(@Search, ',', '%') + '%'' ) '
END

IF isnull(@FromDate,'')<>'' AND isnull(@ToDate,'') <>''
BEGIN

    SET @sqlCondition = @sqlCondition + 'AND VisitDate  BETWEEN ''' + @FromDate + ''' AND ''' + @ToDate + ''' '
END
 --print @sqlCondition
Set @TotalRecords='(SELECT Count(*)  FROM #ResultTable where 1=1 '   + @sqlCondition +')'

SET @sql = 'SELECT '+@TotalRecords+' RowsTotal, * FROM #ResultTable where 1=1' 

SET @sqlFetchQuery = ' ORDER BY ReservationId ASC OFFSET ' + CAST(@Row AS VARCHAR(50)) + ' ROWS FETCH NEXT ' + CAST(@Fetch AS VARCHAR(50)) + ' ROW ONLY';

SET @sql = @sql + @sqlCondition+ @sqlFetchQuery;
	 
print @sql -- Check the generated SQL

exec sp_executesql  @sql
drop table #ResultTable 
END ;

