USE [master]
GO
/****** Object:  Database [ToolManage]    Script Date: 2020/1/12 下午 12:20:17 ******/
CREATE DATABASE [ToolManage]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ToolManage', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.ZHOU_CHAO\MSSQL\DATA\ToolManage.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ToolManage_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.ZHOU_CHAO\MSSQL\DATA\ToolManage_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ToolManage] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ToolManage].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ToolManage] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ToolManage] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ToolManage] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ToolManage] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ToolManage] SET ARITHABORT OFF 
GO
ALTER DATABASE [ToolManage] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ToolManage] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ToolManage] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ToolManage] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ToolManage] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ToolManage] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ToolManage] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ToolManage] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ToolManage] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ToolManage] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ToolManage] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ToolManage] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ToolManage] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ToolManage] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ToolManage] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ToolManage] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ToolManage] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ToolManage] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ToolManage] SET  MULTI_USER 
GO
ALTER DATABASE [ToolManage] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ToolManage] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ToolManage] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ToolManage] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ToolManage] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ToolManage] SET QUERY_STORE = OFF
GO
USE [ToolManage]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workCellId] [int] NOT NULL,
	[userName] [char](20) NOT NULL,
	[passWord] [char](32) NOT NULL,
	[name] [nchar](10) NOT NULL,
	[phone] [char](11) NULL,
	[jobNumber] [int] NULL,
	[jurisdiction] [char](1) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsumeReturn]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsumeReturn](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[accountId] [int] NOT NULL,
	[toolEntityId] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[borrowReturn] [bit] NOT NULL,
	[remark] [nvarchar](50) NULL,
	[borrowLocation] [nvarchar](10) NULL,
 CONSTRAINT [PK_consumeReturn] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inner]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inner](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[detail] [nvarchar](50) NOT NULL,
	[type] [char](1) NOT NULL,
 CONSTRAINT [PK_Inner] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InOutStock]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InOutStock](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NOT NULL,
	[recordId] [int] NOT NULL,
	[bringId] [int] NOT NULL,
	[inOut] [bit] NOT NULL,
	[lineId] [int] NULL,
	[toolEntityId] [int] NOT NULL,
	[location] [varchar](20) NULL,
 CONSTRAINT [PK_inOutStock] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Maintenance]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Maintenance](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[accountId] [int] NOT NULL,
	[toolEntityId] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[project] [char](2) NOT NULL,
	[remark] [nvarchar](50) NULL,
 CONSTRAINT [PK_Maintenance] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchasingApplication]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchasingApplication](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[applicationId] [int] NOT NULL,
	[applicationDate] [datetime] NOT NULL,
	[picture] [image] NULL,
	[billNo] [char](16) NOT NULL,
	[toolDefId] [int] NOT NULL,
	[remark] [nvarchar](50) NULL,
	[state] [char](1) NOT NULL,
	[firstTrialId] [int] NULL,
	[firstTrialDate] [datetime] NULL,
	[finalTrialId] [int] NULL,
	[finalTrialDate] [datetime] NULL,
	[workCellId] [int] NOT NULL,
 CONSTRAINT [PK_PurchasingApplication] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RepairApplication]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RepairApplication](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[applicationId] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[toolEntityId] [int] NOT NULL,
	[describe] [nvarchar](50) NOT NULL,
	[picture] [image] NULL,
	[handleId] [int] NULL,
	[state] [char](1) NOT NULL,
	[workCellId] [int] NOT NULL,
 CONSTRAINT [PK_RepairApplication] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScrapApplication]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScrapApplication](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[applicationId] [int] NOT NULL,
	[toolEntityId] [int] NOT NULL,
	[lifeCount] [int] NOT NULL,
	[reason] [nvarchar](50) NOT NULL,
	[date] [datetime] NOT NULL,
	[state] [char](1) NOT NULL,
	[firstTrialId] [int] NULL,
	[firstTrialDate] [datetime] NULL,
	[finalTrialId] [int] NULL,
	[finalTrialDate] [datetime] NULL,
	[workCellId] [int] NOT NULL,
 CONSTRAINT [PK_ScrapApplication] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToolDef]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolDef](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](20) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[familyId] [int] NOT NULL,
	[model] [nvarchar](50) NOT NULL,
	[partNo] [nvarchar](50) NOT NULL,
	[UPL] [int] NOT NULL,
	[usedForId] [int] NOT NULL,
	[ownerId] [int] NOT NULL,
	[recordDate] [datetime] NOT NULL,
	[recordId] [int] NOT NULL,
	[editDate] [datetime] NOT NULL,
	[editId] [int] NOT NULL,
	[workCellId] [int] NOT NULL,
	[PMPeriod] [int] NOT NULL,
 CONSTRAINT [PK_ToolDef] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToolEntity]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolEntity](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[toolDefId] [int] NOT NULL,
	[usedCount] [int] NOT NULL,
	[location] [varchar](20) NOT NULL,
	[picture] [image] NOT NULL,
	[billNo] [char](16) NOT NULL,
	[regDate] [datetime] NOT NULL,
	[productionDate] [datetime] NOT NULL,
	[bringId] [int] NOT NULL,
	[state] [char](1) NOT NULL,
 CONSTRAINT [PK_ToolEntity] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkCell]    Script Date: 2020/1/12 下午 12:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkCell](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[contactName] [nchar](10) NOT NULL,
	[contactPhone] [char](11) NOT NULL,
 CONSTRAINT [PK_WorkCell] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PurchasingApplication] ADD  CONSTRAINT [DF_PurchasingApplication_state]  DEFAULT ((0)) FOR [state]
GO
ALTER TABLE [dbo].[RepairApplication] ADD  CONSTRAINT [DF_RepairApplication_state]  DEFAULT ((0)) FOR [state]
GO
ALTER TABLE [dbo].[ScrapApplication] ADD  CONSTRAINT [DF_ScrapApplication_lifeCount]  DEFAULT ((0)) FOR [lifeCount]
GO
ALTER TABLE [dbo].[ScrapApplication] ADD  CONSTRAINT [DF_ScrapApplication_state]  DEFAULT ((0)) FOR [state]
GO
ALTER TABLE [dbo].[ToolEntity] ADD  CONSTRAINT [DF_ToolEntity_usedCount]  DEFAULT ((0)) FOR [usedCount]
GO
ALTER TABLE [dbo].[ToolEntity] ADD  CONSTRAINT [DF_ToolEntity_state]  DEFAULT ((0)) FOR [state]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_WorkCell] FOREIGN KEY([workCellId])
REFERENCES [dbo].[WorkCell] ([id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_WorkCell]
GO
ALTER TABLE [dbo].[ConsumeReturn]  WITH CHECK ADD  CONSTRAINT [FK_consumeReturn_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ConsumeReturn] CHECK CONSTRAINT [FK_consumeReturn_Account]
GO
ALTER TABLE [dbo].[ConsumeReturn]  WITH CHECK ADD  CONSTRAINT [FK_consumeReturn_ToolEntity] FOREIGN KEY([toolEntityId])
REFERENCES [dbo].[ToolEntity] ([id])
GO
ALTER TABLE [dbo].[ConsumeReturn] CHECK CONSTRAINT [FK_consumeReturn_ToolEntity]
GO
ALTER TABLE [dbo].[InOutStock]  WITH CHECK ADD  CONSTRAINT [FK_inOutStock_Account] FOREIGN KEY([bringId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[InOutStock] CHECK CONSTRAINT [FK_inOutStock_Account]
GO
ALTER TABLE [dbo].[InOutStock]  WITH CHECK ADD  CONSTRAINT [FK_inOutStock_Inner] FOREIGN KEY([lineId])
REFERENCES [dbo].[Inner] ([id])
GO
ALTER TABLE [dbo].[InOutStock] CHECK CONSTRAINT [FK_inOutStock_Inner]
GO
ALTER TABLE [dbo].[InOutStock]  WITH CHECK ADD  CONSTRAINT [FK_inOutStock_ToolEntity] FOREIGN KEY([toolEntityId])
REFERENCES [dbo].[ToolEntity] ([id])
GO
ALTER TABLE [dbo].[InOutStock] CHECK CONSTRAINT [FK_inOutStock_ToolEntity]
GO
ALTER TABLE [dbo].[Maintenance]  WITH CHECK ADD  CONSTRAINT [FK_Maintenance_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Maintenance] CHECK CONSTRAINT [FK_Maintenance_Account]
GO
ALTER TABLE [dbo].[Maintenance]  WITH CHECK ADD  CONSTRAINT [FK_Maintenance_ToolEntity] FOREIGN KEY([toolEntityId])
REFERENCES [dbo].[ToolEntity] ([id])
GO
ALTER TABLE [dbo].[Maintenance] CHECK CONSTRAINT [FK_Maintenance_ToolEntity]
GO
ALTER TABLE [dbo].[PurchasingApplication]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingApplication_Account] FOREIGN KEY([applicationId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[PurchasingApplication] CHECK CONSTRAINT [FK_PurchasingApplication_Account]
GO
ALTER TABLE [dbo].[PurchasingApplication]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingApplication_Account1] FOREIGN KEY([finalTrialId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[PurchasingApplication] CHECK CONSTRAINT [FK_PurchasingApplication_Account1]
GO
ALTER TABLE [dbo].[PurchasingApplication]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingApplication_Account2] FOREIGN KEY([firstTrialId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[PurchasingApplication] CHECK CONSTRAINT [FK_PurchasingApplication_Account2]
GO
ALTER TABLE [dbo].[PurchasingApplication]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingApplication_ToolDef] FOREIGN KEY([toolDefId])
REFERENCES [dbo].[ToolDef] ([id])
GO
ALTER TABLE [dbo].[PurchasingApplication] CHECK CONSTRAINT [FK_PurchasingApplication_ToolDef]
GO
ALTER TABLE [dbo].[PurchasingApplication]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingApplication_WorkCell] FOREIGN KEY([workCellId])
REFERENCES [dbo].[WorkCell] ([id])
GO
ALTER TABLE [dbo].[PurchasingApplication] CHECK CONSTRAINT [FK_PurchasingApplication_WorkCell]
GO
ALTER TABLE [dbo].[RepairApplication]  WITH CHECK ADD  CONSTRAINT [FK_RepairApplication_Account] FOREIGN KEY([applicationId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[RepairApplication] CHECK CONSTRAINT [FK_RepairApplication_Account]
GO
ALTER TABLE [dbo].[RepairApplication]  WITH CHECK ADD  CONSTRAINT [FK_RepairApplication_Account1] FOREIGN KEY([handleId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[RepairApplication] CHECK CONSTRAINT [FK_RepairApplication_Account1]
GO
ALTER TABLE [dbo].[RepairApplication]  WITH CHECK ADD  CONSTRAINT [FK_RepairApplication_ToolEntity] FOREIGN KEY([toolEntityId])
REFERENCES [dbo].[ToolEntity] ([id])
GO
ALTER TABLE [dbo].[RepairApplication] CHECK CONSTRAINT [FK_RepairApplication_ToolEntity]
GO
ALTER TABLE [dbo].[RepairApplication]  WITH CHECK ADD  CONSTRAINT [FK_RepairApplication_WorkCell] FOREIGN KEY([workCellId])
REFERENCES [dbo].[WorkCell] ([id])
GO
ALTER TABLE [dbo].[RepairApplication] CHECK CONSTRAINT [FK_RepairApplication_WorkCell]
GO
ALTER TABLE [dbo].[ScrapApplication]  WITH CHECK ADD  CONSTRAINT [FK_ScrapApplication_Account] FOREIGN KEY([applicationId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ScrapApplication] CHECK CONSTRAINT [FK_ScrapApplication_Account]
GO
ALTER TABLE [dbo].[ScrapApplication]  WITH CHECK ADD  CONSTRAINT [FK_ScrapApplication_Account1] FOREIGN KEY([finalTrialId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ScrapApplication] CHECK CONSTRAINT [FK_ScrapApplication_Account1]
GO
ALTER TABLE [dbo].[ScrapApplication]  WITH CHECK ADD  CONSTRAINT [FK_ScrapApplication_Account2] FOREIGN KEY([firstTrialId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ScrapApplication] CHECK CONSTRAINT [FK_ScrapApplication_Account2]
GO
ALTER TABLE [dbo].[ScrapApplication]  WITH CHECK ADD  CONSTRAINT [FK_ScrapApplication_ToolEntity] FOREIGN KEY([toolEntityId])
REFERENCES [dbo].[ToolEntity] ([id])
GO
ALTER TABLE [dbo].[ScrapApplication] CHECK CONSTRAINT [FK_ScrapApplication_ToolEntity]
GO
ALTER TABLE [dbo].[ScrapApplication]  WITH CHECK ADD  CONSTRAINT [FK_ScrapApplication_WorkCell] FOREIGN KEY([workCellId])
REFERENCES [dbo].[WorkCell] ([id])
GO
ALTER TABLE [dbo].[ScrapApplication] CHECK CONSTRAINT [FK_ScrapApplication_WorkCell]
GO
ALTER TABLE [dbo].[ToolDef]  WITH CHECK ADD  CONSTRAINT [FK_ToolDef_Account_On_editId] FOREIGN KEY([editId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ToolDef] CHECK CONSTRAINT [FK_ToolDef_Account_On_editId]
GO
ALTER TABLE [dbo].[ToolDef]  WITH CHECK ADD  CONSTRAINT [FK_ToolDef_Account_On_ownerId] FOREIGN KEY([ownerId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ToolDef] CHECK CONSTRAINT [FK_ToolDef_Account_On_ownerId]
GO
ALTER TABLE [dbo].[ToolDef]  WITH CHECK ADD  CONSTRAINT [FK_ToolDef_Account_On_recordId] FOREIGN KEY([recordId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ToolDef] CHECK CONSTRAINT [FK_ToolDef_Account_On_recordId]
GO
ALTER TABLE [dbo].[ToolDef]  WITH CHECK ADD  CONSTRAINT [FK_ToolDef_Inner_On_familyId] FOREIGN KEY([familyId])
REFERENCES [dbo].[Inner] ([id])
GO
ALTER TABLE [dbo].[ToolDef] CHECK CONSTRAINT [FK_ToolDef_Inner_On_familyId]
GO
ALTER TABLE [dbo].[ToolDef]  WITH CHECK ADD  CONSTRAINT [FK_ToolDef_Inner_On_usedForId] FOREIGN KEY([usedForId])
REFERENCES [dbo].[Inner] ([id])
GO
ALTER TABLE [dbo].[ToolDef] CHECK CONSTRAINT [FK_ToolDef_Inner_On_usedForId]
GO
ALTER TABLE [dbo].[ToolDef]  WITH CHECK ADD  CONSTRAINT [FK_ToolDef_WorkCell_On_workCellId] FOREIGN KEY([workCellId])
REFERENCES [dbo].[WorkCell] ([id])
GO
ALTER TABLE [dbo].[ToolDef] CHECK CONSTRAINT [FK_ToolDef_WorkCell_On_workCellId]
GO
ALTER TABLE [dbo].[ToolEntity]  WITH CHECK ADD  CONSTRAINT [FK_ToolEntity_Account_On_bringId] FOREIGN KEY([bringId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[ToolEntity] CHECK CONSTRAINT [FK_ToolEntity_Account_On_bringId]
GO
ALTER TABLE [dbo].[ToolEntity]  WITH CHECK ADD  CONSTRAINT [FK_ToolEntity_ToolDef_On_toolDefId] FOREIGN KEY([toolDefId])
REFERENCES [dbo].[ToolDef] ([id])
GO
ALTER TABLE [dbo].[ToolEntity] CHECK CONSTRAINT [FK_ToolEntity_ToolDef_On_toolDefId]
GO
USE [master]
GO
ALTER DATABASE [ToolManage] SET  READ_WRITE 
GO
