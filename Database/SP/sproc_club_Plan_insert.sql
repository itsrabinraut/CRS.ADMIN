USE [CRS_V2]
GO
/****** Object:  StoredProcedure [dbo].[sproc_club_Plan_insert]    Script Date: 5/31/2024 3:44:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[sproc_club_Plan_insert]
(
@ClubId bigint
)
As
SET NOCOUNT ON
DECLARE @Count int, @date datetime,@lastentry varchar(100),@LastOrderTime varchar(100)
		SELECT 
			@lastentry=LastEntrySyokai,
			@LastOrderTime=LastOrderTime
		FROM 
			tbl_club_details 
		WHERE 
			AgentId=@ClubID

    IF (@lastentry is null OR @LastOrderTime is null)
		Begin
			Return
		END

		IF NOT EXISTS ( select ClubID From tbl_club_plan Where ClubId=@ClubID)
			BEGIN
				Create table #tempDescriptionTable
				(
					ClubPlanTypeID int, 
					Desval varchar(100)
				)
	
				INSERT INTO #tempDescriptionTable(clubPlanTypeId,Desval)VALUES(2,@lastentry) -- lastentry time
				INSERT INTO #tempDescriptionTable(clubPlanTypeId,Desval)VALUES(3,@LastOrderTime)-- last order time
				INSERT INTO #tempDescriptionTable(clubPlanTypeId,Desval)VALUES(4,'10') -- no of people

				SET @date= GetDate()
				SELECT @Count =COUNT(1) FROM TBL_PLANS WHERE PLANSTATUS='A'

				CREATE TABLE #TMPPLAN
				(
					PlanId bigint
				)
				INSERT INTO #tmpPlan
				(PlanId)
				SELECT 
					PlanId 
				FROM
						tbl_plans 
				WHERE 
					PlanStatus='A'

				WHILE (@Count>0)
				BEGIN
				DECLARE @planId BIGINT
				SELECT TOP(1) @planId=PlanId FROM #tmpPlan

					INSERT INTO tbl_club_plan
					(
						ClubId,
						ClubPlanType,
						ClubPlanTypeId,
						PlanListId,
						Description,
						CreatedDate,
						CreatedPlatform
					)
					SELECT 
						@ClubID,
						StaticDataType,
						StaticDataValue,
						@planId,
						(SELECT DesVal FROM #tempDescriptionTable WHERE ClubPlanTypeID=sd.StaticDataValue),
						@date,
						'CLUB'
					FROM 
						tbl_static_data  sd
					WHERE 
						StaticDataType=38

					DELETE TOP(1) FROM #tmpPlan
					SET @Count=@Count-1
				END
	

				UPDATE tbl_club_plan SET 
					[Description]=PlanListId
				WHERE  
					ClubId=@ClubId 
					AND [Description] IS NULL 
					AND CreatedDate=@date

				DROP TABLE #TMPPLAN
				DROP TABLE #TEMPDESCRIPTIONTABLE
		END
SET NOCOUNT OFF