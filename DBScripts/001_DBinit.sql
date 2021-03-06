USE [master]
GO
/****** Object:  Database [CrossCountry]    Script Date: 11/28/2015 6:06:49 PM ******/
CREATE DATABASE [CrossCountry]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CrossCountry', FILENAME = N'C:\Users\<username>\CrossCountry.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CrossCountry_log', FILENAME = N'C:\Users\<username>\CrossCountry_log.ldf' , SIZE = 1024KB , MAXSIZE = 4096KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CrossCountry] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CrossCountry].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CrossCountry] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CrossCountry] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CrossCountry] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CrossCountry] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CrossCountry] SET ARITHABORT OFF 
GO
ALTER DATABASE [CrossCountry] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CrossCountry] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CrossCountry] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CrossCountry] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CrossCountry] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CrossCountry] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CrossCountry] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CrossCountry] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CrossCountry] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CrossCountry] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CrossCountry] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CrossCountry] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CrossCountry] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CrossCountry] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CrossCountry] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CrossCountry] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CrossCountry] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CrossCountry] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CrossCountry] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CrossCountry] SET  MULTI_USER 
GO
ALTER DATABASE [CrossCountry] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CrossCountry] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CrossCountry] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CrossCountry] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [CrossCountry]
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[OrganizationId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MascotName] [nvarchar](50) NULL,
	[MascotImageFileLocation] [nvarchar](max) NULL,
	[Tagline] [nvarchar](200) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Race]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Race](
	[RaceId] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [bigint] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[StartedOn] [datetime] NULL,
	[CompletedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[GenderRestriction] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Race] PRIMARY KEY CLUSTERED 
(
	[RaceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RunnerClassification]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RunnerClassification](
	[RunnerClassificationId] [int] IDENTITY(1,1) NOT NULL,
	[RunnerClassificationName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_StudentClassification] PRIMARY KEY CLUSTERED 
(
	[RunnerClassificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RunnerRaceRecord]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RunnerRaceRecord](
	[RunnerRaceRecordId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RaceId] [bigint] NOT NULL,
	[RunnerClassificationId] [int] NOT NULL,
	[VarsityLevelId] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
 CONSTRAINT [PK_RunnerRaceRecord] PRIMARY KEY CLUSTERED 
(
	[RunnerRaceRecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RunnerRaceRecordSegment]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RunnerRaceRecordSegment](
	[RunnerRaceRecordSegmentId] [bigint] IDENTITY(1,1) NOT NULL,
	[RunnerRaceRecordId] [bigint] NOT NULL,
	[ElapsedTimeInSeconds] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
 CONSTRAINT [PK_RunnerRaceRecordSegment] PRIMARY KEY CLUSTERED 
(
	[RunnerRaceRecordSegmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[OrganizationId] [bigint] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Firstname] [nvarchar](50) NOT NULL,
	[Lastname] [nvarchar](50) NOT NULL,
	[Middlename] [nvarchar](50) NULL,
	[Email] [nvarchar](max) NULL,
	[GraduationYear] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[EligibleForRaces] [bit] NOT NULL,
	[DefaultVarsityLevelId] [int] NULL,
	[DefaultRunnerClassificationId] [int] NULL,
	[Gender] [nvarchar](12) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VarsityLevel]    Script Date: 1/2/2016 11:16:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VarsityLevel](
	[VarsityLevelId] [int] IDENTITY(1,1) NOT NULL,
	[VarsityLevelName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_VarsityLevel] PRIMARY KEY CLUSTERED 
(
	[VarsityLevelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Race]  WITH CHECK ADD  CONSTRAINT [FK_Race_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([OrganizationId])
GO
ALTER TABLE [dbo].[Race] CHECK CONSTRAINT [FK_Race_Organization]
GO
ALTER TABLE [dbo].[RunnerRaceRecord]  WITH CHECK ADD  CONSTRAINT [FK_RunnerRaceRecord_Race] FOREIGN KEY([RaceId])
REFERENCES [dbo].[Race] ([RaceId])
GO
ALTER TABLE [dbo].[RunnerRaceRecord] CHECK CONSTRAINT [FK_RunnerRaceRecord_Race]
GO
ALTER TABLE [dbo].[RunnerRaceRecord]  WITH CHECK ADD  CONSTRAINT [FK_RunnerRaceRecord_RunnerClassification] FOREIGN KEY([RunnerClassificationId])
REFERENCES [dbo].[RunnerClassification] ([RunnerClassificationId])
GO
ALTER TABLE [dbo].[RunnerRaceRecord] CHECK CONSTRAINT [FK_RunnerRaceRecord_RunnerClassification]
GO
ALTER TABLE [dbo].[RunnerRaceRecord]  WITH CHECK ADD  CONSTRAINT [FK_RunnerRaceRecord_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[RunnerRaceRecord] CHECK CONSTRAINT [FK_RunnerRaceRecord_User]
GO
ALTER TABLE [dbo].[RunnerRaceRecord]  WITH CHECK ADD  CONSTRAINT [FK_RunnerRaceRecord_VarsityLevel] FOREIGN KEY([VarsityLevelId])
REFERENCES [dbo].[VarsityLevel] ([VarsityLevelId])
GO
ALTER TABLE [dbo].[RunnerRaceRecord] CHECK CONSTRAINT [FK_RunnerRaceRecord_VarsityLevel]
GO
ALTER TABLE [dbo].[RunnerRaceRecordSegment]  WITH CHECK ADD  CONSTRAINT [FK_RunnerRaceRecordSegment_RunnerRaceRecord] FOREIGN KEY([RunnerRaceRecordId])
REFERENCES [dbo].[RunnerRaceRecord] ([RunnerRaceRecordId])
GO
ALTER TABLE [dbo].[RunnerRaceRecordSegment] CHECK CONSTRAINT [FK_RunnerRaceRecordSegment_RunnerRaceRecord]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([OrganizationId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Organization]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
USE [master]
GO
ALTER DATABASE [CrossCountry] SET  READ_WRITE 
GO
