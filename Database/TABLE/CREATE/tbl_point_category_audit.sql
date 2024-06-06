USE [CRS_V2]
GO

/****** Object:  Table [dbo].[tbl_point_category_audit]    Script Date: 6/4/2024 12:39:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_point_category_audit](
	[AuditId] [bigint] IDENTITY(1,2) PRIMARY KEY NOT NULL,
	[Id] VARCHAR(10) NULL,
	[CategoryName] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[RoleType] [int] NULL,
	[Status] [varchar](5) NULL,
	[IsDefault] [bit] NULL,
	[ActionDate] [datetime] NULL,
	[ActionIp] [varchar](50) NULL,
	[ActionUser] [varchar](50) NULL,
	TriggerLogUser	nvarchar(200) NULL,
	TriggerAction	nvarchar(100) NULL,
	TriggerActionLocalDate	datetime NULL,
	TriggerActionUTCDate	datetime NULL
)

