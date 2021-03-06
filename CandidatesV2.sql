USE [master]
GO
/****** Object:  Database [Candidates]    Script Date: 01/01/2021 19:25:37 ******/
CREATE DATABASE [Candidates]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Candidates', FILENAME = N'C:\Program Files (x86)\Plesk\Databases\MSSQL\MSSQL14.MSSQLSERVER2017\MSSQL\DATA\Candidates.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Candidates_log', FILENAME = N'C:\Program Files (x86)\Plesk\Databases\MSSQL\MSSQL14.MSSQLSERVER2017\MSSQL\DATA\Candidates_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Candidates] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Candidates].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Candidates] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Candidates] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Candidates] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Candidates] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Candidates] SET ARITHABORT OFF 
GO
ALTER DATABASE [Candidates] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Candidates] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Candidates] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Candidates] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Candidates] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Candidates] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Candidates] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Candidates] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Candidates] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Candidates] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Candidates] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Candidates] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Candidates] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Candidates] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Candidates] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Candidates] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Candidates] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Candidates] SET RECOVERY FULL 
GO
ALTER DATABASE [Candidates] SET  MULTI_USER 
GO
ALTER DATABASE [Candidates] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Candidates] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Candidates] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Candidates] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Candidates] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Candidates] SET QUERY_STORE = OFF
GO
USE [Candidates]
GO
/****** Object:  User [Candidates]    Script Date: 01/01/2021 19:25:40 ******/
CREATE USER [Candidates] FOR LOGIN [Candidates] WITH DEFAULT_SCHEMA=[Candidates]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [Candidates]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [Candidates]
GO
ALTER ROLE [db_datareader] ADD MEMBER [Candidates]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [Candidates]
GO
/****** Object:  Schema [Candidates]    Script Date: 01/01/2021 19:25:40 ******/
CREATE SCHEMA [Candidates]
GO
/****** Object:  Table [dbo].[Branches]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branches](
	[BrancheId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[ProfileId] [bigint] NULL,
 CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED 
(
	[BrancheId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidateAttachments]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidateAttachments](
	[CandidateAttachmentId] [bigint] NOT NULL,
	[CandidateId] [bigint] NULL,
	[BirthDateCertificate] [nvarchar](300) NULL,
	[NIDCertificate] [nvarchar](300) NULL,
	[FamilyPaper] [nvarchar](300) NULL,
	[AbsenceOfPrecedents] [nvarchar](300) NULL,
	[PaymentReceipt] [nvarchar](300) NULL,
	[Status] [smallint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_CandidateAttachments_1] PRIMARY KEY CLUSTERED 
(
	[CandidateAttachmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidateContacts]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidateContacts](
	[CandidateContactId] [bigint] IDENTITY(1,1) NOT NULL,
	[Object] [varchar](100) NULL,
	[ObjectType] [smallint] NULL,
	[CandidateId] [bigint] NULL,
	[Status] [smallint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_CandidateContacts] PRIMARY KEY CLUSTERED 
(
	[CandidateContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidateRepresentatives]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidateRepresentatives](
	[CandidateRepresentativeId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[FatherName] [nvarchar](100) NULL,
	[GrandFatherName] [nvarchar](100) NULL,
	[SurName] [nvarchar](100) NULL,
	[NID] [varchar](12) NULL,
	[MotherName] [nvarchar](100) NULL,
	[Gender] [smallint] NULL,
	[BirthDate] [datetime] NULL,
	[Phone] [varchar](20) NULL,
	[HomePhone] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[CandidateId] [bigint] NULL,
 CONSTRAINT [PK_CandidateRepresentatives] PRIMARY KEY CLUSTERED 
(
	[CandidateRepresentativeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Candidates]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Candidates](
	[CandidateId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[FatherName] [nvarchar](100) NULL,
	[GrandFatherName] [nvarchar](100) NULL,
	[SurName] [nvarchar](100) NULL,
	[NID] [varchar](12) NULL,
	[MotherName] [nvarchar](100) NULL,
	[Gender] [smallint] NULL,
	[BirthDate] [datetime] NULL,
	[Phone] [varchar](20) NULL,
	[HomePhone] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[Qualification] [nvarchar](200) NULL,
	[ProfileId] [bigint] NULL,
	[OfficeId] [bigint] NULL,
	[ConstituencyId] [bigint] NULL,
	[SubConstituencyId] [bigint] NULL,
	[Levels] [smallint] NULL,
	[CompetitionType] [smallint] NULL,
	[EntityId] [bigint] NULL,
	[Status] [smallint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED 
(
	[CandidateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidateUsers]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidateUsers](
	[CandidateUserId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](250) NULL,
	[Image] [varbinary](max) NULL,
	[Phone] [nvarchar](25) NULL,
	[BirthDate] [datetime] NULL,
	[LoginTryAttemptDate] [datetime] NULL,
	[LoginTryAttempts] [smallint] NULL,
	[LastLoginOn] [datetime] NULL,
	[Gender] [smallint] NULL,
	[UserType] [smallint] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[Status] [smallint] NULL,
	[CandidateId] [bigint] NULL,
 CONSTRAINT [PK_CandidateUsers] PRIMARY KEY CLUSTERED 
(
	[CandidateUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Centers]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Centers](
	[CenterId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[OfficeId] [bigint] NULL,
	[Latitude] [varchar](100) NULL,
	[Longitude] [varchar](100) NULL,
	[ProfileId] [bigint] NULL,
	[ConstituencDetailId] [bigint] NULL,
	[Status] [smallint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Centers] PRIMARY KEY CLUSTERED 
(
	[CenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChairDetails]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChairDetails](
	[ChairDetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralChairs] [int] NULL,
	[PrivateChairs] [int] NULL,
	[RelativeChairs] [int] NULL,
	[ChairId] [bigint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_ChairDetails] PRIMARY KEY CLUSTERED 
(
	[ChairDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chairs]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chairs](
	[ChairId] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralChairs] [int] NULL,
	[PrivateChairs] [int] NULL,
	[RelativeChairs] [bigint] NULL,
	[GeneralChairRemaining] [int] NULL,
	[PrivateChairRemaining] [int] NULL,
	[RelativeChairRemaining] [int] NULL,
	[ConstituencyId] [bigint] NULL,
	[Status] [smallint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Chairs] PRIMARY KEY CLUSTERED 
(
	[ChairId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Constituencies]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Constituencies](
	[ConstituencyId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[OfficeId] [bigint] NULL,
	[ProfileId] [bigint] NULL,
	[RegionId] [bigint] NULL,
 CONSTRAINT [PK_Constituencies] PRIMARY KEY CLUSTERED 
(
	[ConstituencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConstituencyDetailChairs]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConstituencyDetailChairs](
	[ConstituencyDetailChairId] [bigint] IDENTITY(1,1) NOT NULL,
	[ConstituencyDetailId] [bigint] NULL,
	[ChairDetailId] [bigint] NULL,
 CONSTRAINT [PK_ConstituencyDetailChairs] PRIMARY KEY CLUSTERED 
(
	[ConstituencyDetailChairId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConstituencyDetails]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConstituencyDetails](
	[ConstituencyDetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[ConstituencyId] [bigint] NOT NULL,
	[RegionId] [bigint] NULL,
	[ProfileId] [bigint] NULL,
 CONSTRAINT [PK_SubConstituencyId] PRIMARY KEY CLUSTERED 
(
	[ConstituencyDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Endorsements]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Endorsements](
	[EndorsementId] [bigint] IDENTITY(1,1) NOT NULL,
	[CandidateId] [bigint] NULL,
	[NID] [varchar](13) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_Endorsements] PRIMARY KEY CLUSTERED 
(
	[EndorsementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entities]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entities](
	[EntityId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](300) NULL,
	[Number] [bigint] NULL,
	[Descriptions] [nvarchar](500) NULL,
	[Phone] [varchar](25) NULL,
	[logo] [varbinary](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Address] [nvarchar](250) NULL,
	[ProfileId] [bigint] NULL,
	[Status] [smallint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_Entities] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityAttachments]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityAttachments](
	[EntityAttachmentId] [bigint] IDENTITY(1,1) NOT NULL,
	[NameHeadEntity] [nvarchar](300) NULL,
	[LegalAgreementPoliticalEntity] [nvarchar](300) NULL,
	[PoliticalEntitySymbol] [nvarchar](300) NULL,
	[CampaignAccountNumber] [nvarchar](300) NULL,
	[Status] [smallint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_EntityAttachments] PRIMARY KEY CLUSTERED 
(
	[EntityAttachmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityRepresentatives]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityRepresentatives](
	[EntityRepresentativeId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[FatherName] [nvarchar](100) NULL,
	[GrandFatherName] [nvarchar](100) NULL,
	[SurName] [nvarchar](100) NULL,
	[NID] [varchar](12) NULL,
	[MotherName] [nvarchar](100) NULL,
	[Gender] [smallint] NULL,
	[BirthDate] [datetime] NULL,
	[Phone] [varchar](20) NULL,
	[HomePhone] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[EntityId] [bigint] NULL,
 CONSTRAINT [PK_EntityRepresentatives] PRIMARY KEY CLUSTERED 
(
	[EntityRepresentativeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityUsers]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityUsers](
	[EntityUserId] [bigint] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](250) NULL,
	[Image] [varbinary](max) NULL,
	[Phone] [nvarchar](25) NULL,
	[BirthDate] [datetime] NULL,
	[LoginTryAttemptDate] [datetime] NULL,
	[LoginTryAttempts] [smallint] NULL,
	[LastLoginOn] [datetime] NULL,
	[Gender] [smallint] NULL,
	[UserType] [smallint] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[Status] [smallint] NULL,
	[EntityId] [bigint] NULL,
 CONSTRAINT [PK_EntityUsers] PRIMARY KEY CLUSTERED 
(
	[EntityUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Offices]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offices](
	[OfficeId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[BranchId] [bigint] NULL,
	[ProfileId] [bigint] NULL,
	[Status] [smallint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Offices] PRIMARY KEY CLUSTERED 
(
	[OfficeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ProfileType] [smallint] NULL,
	[IsActivate] [smallint] NULL,
	[Status] [smallint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Regions]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regions](
	[RegionId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Status] [smallint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Regions] PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stations]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stations](
	[StationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CenterId] [bigint] NULL,
	[ProfileId] [bigint] NULL,
	[Status] [smallint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Stations] PRIMARY KEY CLUSTERED 
(
	[StationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 01/01/2021 19:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[LoginName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](250) NULL,
	[Image] [varbinary](max) NULL,
	[Phone] [nvarchar](25) NULL,
	[BirthDate] [datetime] NULL,
	[LoginTryAttemptDate] [datetime] NULL,
	[LoginTryAttempts] [smallint] NULL,
	[LastLoginOn] [datetime] NULL,
	[Gender] [smallint] NULL,
	[UserType] [smallint] NULL,
	[Status] [smallint] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CandidateUsers] ADD  CONSTRAINT [DF_CandidateUsers_UserType]  DEFAULT ((2)) FOR [UserType]
GO
ALTER TABLE [dbo].[CandidateUsers] ADD  CONSTRAINT [DF_Table_1_State]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[EntityUsers] ADD  CONSTRAINT [DF_EntityUsers_UserType]  DEFAULT ((2)) FOR [UserType]
GO
ALTER TABLE [dbo].[EntityUsers] ADD  CONSTRAINT [DF_EntityUsers_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UserType]  DEFAULT ((2)) FOR [UserType]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_State]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Branches]  WITH CHECK ADD  CONSTRAINT [FK_Branches_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([ProfileId])
GO
ALTER TABLE [dbo].[Branches] CHECK CONSTRAINT [FK_Branches_Profile]
GO
ALTER TABLE [dbo].[CandidateAttachments]  WITH CHECK ADD  CONSTRAINT [FK_CandidateAttachments_Candidates] FOREIGN KEY([CandidateId])
REFERENCES [dbo].[Candidates] ([CandidateId])
GO
ALTER TABLE [dbo].[CandidateAttachments] CHECK CONSTRAINT [FK_CandidateAttachments_Candidates]
GO
ALTER TABLE [dbo].[CandidateRepresentatives]  WITH CHECK ADD  CONSTRAINT [FK_CandidateRepresentatives_Candidates] FOREIGN KEY([CandidateId])
REFERENCES [dbo].[Candidates] ([CandidateId])
GO
ALTER TABLE [dbo].[CandidateRepresentatives] CHECK CONSTRAINT [FK_CandidateRepresentatives_Candidates]
GO
ALTER TABLE [dbo].[Centers]  WITH CHECK ADD  CONSTRAINT [FK_Centers_ConstituencyDetails] FOREIGN KEY([ConstituencDetailId])
REFERENCES [dbo].[ConstituencyDetails] ([ConstituencyDetailId])
GO
ALTER TABLE [dbo].[Centers] CHECK CONSTRAINT [FK_Centers_ConstituencyDetails]
GO
ALTER TABLE [dbo].[Centers]  WITH CHECK ADD  CONSTRAINT [FK_Centers_Offices] FOREIGN KEY([OfficeId])
REFERENCES [dbo].[Offices] ([OfficeId])
GO
ALTER TABLE [dbo].[Centers] CHECK CONSTRAINT [FK_Centers_Offices]
GO
ALTER TABLE [dbo].[Centers]  WITH CHECK ADD  CONSTRAINT [FK_Centers_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([ProfileId])
GO
ALTER TABLE [dbo].[Centers] CHECK CONSTRAINT [FK_Centers_Profile]
GO
ALTER TABLE [dbo].[ChairDetails]  WITH CHECK ADD  CONSTRAINT [FK_ChairDetails_Chairs] FOREIGN KEY([ChairId])
REFERENCES [dbo].[Chairs] ([ChairId])
GO
ALTER TABLE [dbo].[ChairDetails] CHECK CONSTRAINT [FK_ChairDetails_Chairs]
GO
ALTER TABLE [dbo].[Chairs]  WITH CHECK ADD  CONSTRAINT [FK_Chairs_Constituencies] FOREIGN KEY([ConstituencyId])
REFERENCES [dbo].[Constituencies] ([ConstituencyId])
GO
ALTER TABLE [dbo].[Chairs] CHECK CONSTRAINT [FK_Chairs_Constituencies]
GO
ALTER TABLE [dbo].[Constituencies]  WITH CHECK ADD  CONSTRAINT [FK_Constituencies_Offices] FOREIGN KEY([OfficeId])
REFERENCES [dbo].[Offices] ([OfficeId])
GO
ALTER TABLE [dbo].[Constituencies] CHECK CONSTRAINT [FK_Constituencies_Offices]
GO
ALTER TABLE [dbo].[Constituencies]  WITH CHECK ADD  CONSTRAINT [FK_Constituencies_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([ProfileId])
GO
ALTER TABLE [dbo].[Constituencies] CHECK CONSTRAINT [FK_Constituencies_Profile]
GO
ALTER TABLE [dbo].[Constituencies]  WITH CHECK ADD  CONSTRAINT [FK_Constituencies_Regions] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Regions] ([RegionId])
GO
ALTER TABLE [dbo].[Constituencies] CHECK CONSTRAINT [FK_Constituencies_Regions]
GO
ALTER TABLE [dbo].[ConstituencyDetailChairs]  WITH CHECK ADD  CONSTRAINT [FK_ConstituencyDetailChairs_ChairDetails] FOREIGN KEY([ChairDetailId])
REFERENCES [dbo].[ChairDetails] ([ChairDetailId])
GO
ALTER TABLE [dbo].[ConstituencyDetailChairs] CHECK CONSTRAINT [FK_ConstituencyDetailChairs_ChairDetails]
GO
ALTER TABLE [dbo].[ConstituencyDetailChairs]  WITH CHECK ADD  CONSTRAINT [FK_ConstituencyDetailChairs_Constituencies] FOREIGN KEY([ConstituencyDetailId])
REFERENCES [dbo].[Constituencies] ([ConstituencyId])
GO
ALTER TABLE [dbo].[ConstituencyDetailChairs] CHECK CONSTRAINT [FK_ConstituencyDetailChairs_Constituencies]
GO
ALTER TABLE [dbo].[ConstituencyDetails]  WITH CHECK ADD  CONSTRAINT [FK_ConstituencyDetails_Constituencies] FOREIGN KEY([ConstituencyId])
REFERENCES [dbo].[Constituencies] ([ConstituencyId])
GO
ALTER TABLE [dbo].[ConstituencyDetails] CHECK CONSTRAINT [FK_ConstituencyDetails_Constituencies]
GO
ALTER TABLE [dbo].[ConstituencyDetails]  WITH CHECK ADD  CONSTRAINT [FK_ConstituencyDetails_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([ProfileId])
GO
ALTER TABLE [dbo].[ConstituencyDetails] CHECK CONSTRAINT [FK_ConstituencyDetails_Profile]
GO
ALTER TABLE [dbo].[ConstituencyDetails]  WITH CHECK ADD  CONSTRAINT [FK_ConstituencyDetails_Regions] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Regions] ([RegionId])
GO
ALTER TABLE [dbo].[ConstituencyDetails] CHECK CONSTRAINT [FK_ConstituencyDetails_Regions]
GO
ALTER TABLE [dbo].[Endorsements]  WITH CHECK ADD  CONSTRAINT [FK_Endorsements_Candidates] FOREIGN KEY([CandidateId])
REFERENCES [dbo].[Candidates] ([CandidateId])
GO
ALTER TABLE [dbo].[Endorsements] CHECK CONSTRAINT [FK_Endorsements_Candidates]
GO
ALTER TABLE [dbo].[EntityRepresentatives]  WITH CHECK ADD  CONSTRAINT [FK_EntityRepresentatives_Entities] FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entities] ([EntityId])
GO
ALTER TABLE [dbo].[EntityRepresentatives] CHECK CONSTRAINT [FK_EntityRepresentatives_Entities]
GO
ALTER TABLE [dbo].[EntityUsers]  WITH CHECK ADD  CONSTRAINT [FK_EntityUsers_Entities] FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entities] ([EntityId])
GO
ALTER TABLE [dbo].[EntityUsers] CHECK CONSTRAINT [FK_EntityUsers_Entities]
GO
ALTER TABLE [dbo].[Offices]  WITH CHECK ADD  CONSTRAINT [FK_Offices_Offices] FOREIGN KEY([BranchId])
REFERENCES [dbo].[Branches] ([BrancheId])
GO
ALTER TABLE [dbo].[Offices] CHECK CONSTRAINT [FK_Offices_Offices]
GO
ALTER TABLE [dbo].[Offices]  WITH CHECK ADD  CONSTRAINT [FK_Offices_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([ProfileId])
GO
ALTER TABLE [dbo].[Offices] CHECK CONSTRAINT [FK_Offices_Profile]
GO
ALTER TABLE [dbo].[Stations]  WITH CHECK ADD  CONSTRAINT [FK_Stations_Centers] FOREIGN KEY([CenterId])
REFERENCES [dbo].[Centers] ([CenterId])
GO
ALTER TABLE [dbo].[Stations] CHECK CONSTRAINT [FK_Stations_Centers]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1- نقال
2- تلفون ارضي
3- ايميل' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CandidateContacts', @level2type=N'COLUMN',@level2name=N'ObjectType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1- عام 
2- خاص
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Candidates', @level2type=N'COLUMN',@level2name=N'CompetitionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1- if null is candidate
2- if not null is entity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Candidates', @level2type=N'COLUMN',@level2name=N'EntityId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1- مندوب 
2- ممثل' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CandidateUsers', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CandidateUsers', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1- مندوب 
2- ممثل' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityUsers', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityUsers', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-admin
2-user
3-doctor A
4-doctor B
5-doctor C
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Status'
GO
USE [master]
GO
ALTER DATABASE [Candidates] SET  READ_WRITE 
GO
