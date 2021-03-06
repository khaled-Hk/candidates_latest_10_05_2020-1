USE [Candidates]
GO
/****** Object:  Table [dbo].[Branches]    Script Date: 10/7/2020 1:44:01 PM ******/
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
/****** Object:  Table [dbo].[Centers]    Script Date: 10/7/2020 1:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Centers](
	[CenterId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[OfficeId] [bigint] NULL,
	[Latitude] [varchar](100) NULL,
	[Longitude] [varchar](100) NULL,
	[ProfileId] [bigint] NULL,
	[ConstituencDetailId] [bigint] NULL,
 CONSTRAINT [PK_Centers] PRIMARY KEY CLUSTERED 
(
	[CenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChairDetails]    Script Date: 10/7/2020 1:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChairDetails](
	[ChairDetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralChairs] [int] NULL,
	[PrivateChairs] [int] NULL,
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
/****** Object:  Table [dbo].[Chairs]    Script Date: 10/7/2020 1:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chairs](
	[ChairId] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralChairs] [int] NULL,
	[PrivateChairs] [int] NULL,
	[GeneralChairRemaining] [int] NULL,
	[PrivateChairRemaining] [int] NULL,
	[ConstituencyId] [bigint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_Chairs] PRIMARY KEY CLUSTERED 
(
	[ChairId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Constituencies]    Script Date: 10/7/2020 1:44:01 PM ******/
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
/****** Object:  Table [dbo].[ConstituencyDetailChairs]    Script Date: 10/7/2020 1:44:01 PM ******/
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
/****** Object:  Table [dbo].[ConstituencyDetails]    Script Date: 10/7/2020 1:44:01 PM ******/
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
/****** Object:  Table [dbo].[Offices]    Script Date: 10/7/2020 1:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offices](
	[OfficeId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[BranchId] [bigint] NULL,
	[ProfileId] [bigint] NULL,
 CONSTRAINT [PK_Offices] PRIMARY KEY CLUSTERED 
(
	[OfficeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 10/7/2020 1:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ProfileType] [smallint] NULL,
	[IsActivate] [smallint] NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Regions]    Script Date: 10/7/2020 1:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regions](
	[RegionId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_Regions] PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stations]    Script Date: 10/7/2020 1:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stations](
	[StationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArabicName] [nvarchar](200) NULL,
	[EnglishName] [varchar](200) NULL,
	[Description] [nvarchar](600) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[CenterId] [bigint] NULL,
	[ProfileId] [bigint] NULL,
 CONSTRAINT [PK_Stations] PRIMARY KEY CLUSTERED 
(
	[StationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/7/2020 1:44:01 PM ******/
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
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[State] [smallint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Branches] ON 

INSERT [dbo].[Branches] ([BrancheId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ProfileId]) VALUES (1, N'فرع طرابلس', N'Tripoli branch', N'لايوجد', 1, CAST(N'2020-06-09T00:00:00.000' AS DateTime), 1, CAST(N'2020-10-07T10:34:46.440' AS DateTime), 9, NULL)
INSERT [dbo].[Branches] ([BrancheId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ProfileId]) VALUES (2, N'فرع بنغازي', N'Banghazi branch', N'لايوجد', 1, CAST(N'2018-02-02T00:00:00.000' AS DateTime), 1, CAST(N'2020-10-07T10:35:15.173' AS DateTime), 9, NULL)
INSERT [dbo].[Branches] ([BrancheId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ProfileId]) VALUES (3, N'الفرع الغربي', N'alfar3 garbi', NULL, 1, CAST(N'2020-10-07T13:03:40.113' AS DateTime), NULL, NULL, 1, NULL)
INSERT [dbo].[Branches] ([BrancheId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ProfileId]) VALUES (4, N'XXX', N'KKKK', NULL, 1, CAST(N'2020-10-07T13:04:38.713' AS DateTime), NULL, NULL, 1, NULL)
INSERT [dbo].[Branches] ([BrancheId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ProfileId]) VALUES (5, N'KKK', N'KKKK', NULL, 1, CAST(N'2020-10-07T13:04:50.187' AS DateTime), NULL, NULL, 1, NULL)
SET IDENTITY_INSERT [dbo].[Branches] OFF
SET IDENTITY_INSERT [dbo].[Constituencies] ON 

INSERT [dbo].[Constituencies] ([ConstituencyId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [OfficeId], [ProfileId], [RegionId]) VALUES (1, N'منطقة رئيسيه', N'Main one', NULL, 0, CAST(N'2020-10-07T09:38:15.600' AS DateTime), NULL, CAST(N'2020-10-07T09:38:35.830' AS DateTime), 1, NULL, NULL, 5)
SET IDENTITY_INSERT [dbo].[Constituencies] OFF
SET IDENTITY_INSERT [dbo].[ConstituencyDetails] ON 

INSERT [dbo].[ConstituencyDetails] ([ConstituencyDetailId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ConstituencyId], [RegionId], [ProfileId]) VALUES (1, N'منطقة فرعية', N'sub data', NULL, 0, CAST(N'2020-10-07T09:47:04.710' AS DateTime), NULL, NULL, 9, 1, 5, NULL)
INSERT [dbo].[ConstituencyDetails] ([ConstituencyDetailId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ConstituencyId], [RegionId], [ProfileId]) VALUES (2, N'منطقة فرعية', N'??????', NULL, 0, CAST(N'2020-10-07T09:47:51.010' AS DateTime), NULL, NULL, 9, 1, 5, NULL)
INSERT [dbo].[ConstituencyDetails] ([ConstituencyDetailId], [ArabicName], [EnglishName], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [ConstituencyId], [RegionId], [ProfileId]) VALUES (3, N'منطقة فرعية', N'Sub main', NULL, 0, CAST(N'2020-10-07T09:48:26.583' AS DateTime), NULL, NULL, 1, 1, 5, NULL)
SET IDENTITY_INSERT [dbo].[ConstituencyDetails] OFF
SET IDENTITY_INSERT [dbo].[Profile] ON 

INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (11, N'ملف 1', N'لايوجد', 1, CAST(N'2020-08-03T18:16:18.543' AS DateTime), 1, CAST(N'2020-08-04T02:30:00.300' AS DateTime), 2, CAST(N'2020-07-31T22:00:00.000' AS DateTime), CAST(N'2020-08-29T22:00:00.000' AS DateTime), 1, 0)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (12, N'ملف 3', N'لايوجد', 1, CAST(N'2020-08-03T18:23:49.597' AS DateTime), NULL, NULL, 2, CAST(N'2020-08-06T22:00:00.000' AS DateTime), CAST(N'2020-08-23T22:00:00.000' AS DateTime), 3, 0)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (13, N'KKK', N'', 1, CAST(N'2020-08-03T18:26:07.353' AS DateTime), 1, CAST(N'2020-08-04T02:29:56.977' AS DateTime), 2, CAST(N'2020-08-10T22:00:00.000' AS DateTime), CAST(N'2020-08-26T22:00:00.000' AS DateTime), 3, 0)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (14, N'KKK', N'LLL', 1, CAST(N'2020-08-03T18:27:38.343' AS DateTime), NULL, NULL, 2, CAST(N'2020-08-10T22:00:00.000' AS DateTime), CAST(N'2020-08-30T22:00:00.000' AS DateTime), 2, 0)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (15, N'LLL', N'no data', 1, CAST(N'2020-08-03T18:37:02.303' AS DateTime), 1, CAST(N'2020-08-04T15:28:33.720' AS DateTime), 2, CAST(N'2020-08-02T22:00:00.000' AS DateTime), CAST(N'2020-08-23T22:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (16, N'KKK', N'kkk', 1, CAST(N'2020-08-03T18:37:25.573' AS DateTime), 1, CAST(N'2020-08-04T02:45:02.653' AS DateTime), 1, CAST(N'2020-08-03T22:00:00.000' AS DateTime), CAST(N'2020-08-17T22:00:00.000' AS DateTime), 1, 0)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (17, N'kkk', N'', 1, CAST(N'2020-08-03T18:38:17.873' AS DateTime), 1, CAST(N'2020-08-04T02:45:08.173' AS DateTime), 2, CAST(N'2020-08-03T22:00:00.000' AS DateTime), CAST(N'2020-08-26T22:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (18, N'KKK', N'', 1, CAST(N'2020-08-03T18:38:40.883' AS DateTime), 1, CAST(N'2020-08-04T02:45:14.353' AS DateTime), 1, CAST(N'2020-08-16T22:00:00.000' AS DateTime), CAST(N'2020-08-05T22:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (19, N'file elections 10', N'no data', 1, CAST(N'2020-08-03T18:50:28.347' AS DateTime), 1, CAST(N'2020-08-04T02:58:04.603' AS DateTime), 2, CAST(N'2020-07-31T22:00:00.000' AS DateTime), CAST(N'2020-08-30T22:00:00.000' AS DateTime), 2, 1)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (20, N'XXXX', N'Test', 1, CAST(N'2020-08-04T02:55:41.693' AS DateTime), 1, CAST(N'2020-08-04T02:56:16.713' AS DateTime), 1, CAST(N'2020-07-31T22:00:00.000' AS DateTime), CAST(N'2020-08-30T22:00:00.000' AS DateTime), 2, 0)
INSERT [dbo].[Profile] ([ProfileId], [Name], [Description], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status], [StartDate], [EndDate], [ProfileType], [IsActivate]) VALUES (21, NULL, NULL, 1, CAST(N'2020-08-10T16:52:07.790' AS DateTime), 1, CAST(N'2020-10-05T10:07:41.650' AS DateTime), 1, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Profile] OFF
SET IDENTITY_INSERT [dbo].[Regions] ON 

INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (2, N'طرابلس', N'Tripoli', 1, CAST(N'2020-02-22T00:00:00.000' AS DateTime), 1, CAST(N'2020-08-10T19:17:26.640' AS DateTime), 9)
INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (3, N'الزاوية', N'Alzawia', 1, CAST(N'2018-01-11T00:00:00.000' AS DateTime), 1, CAST(N'2020-08-10T19:17:02.720' AS DateTime), 9)
INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (4, N'', N'', 1, CAST(N'2020-08-10T16:52:53.430' AS DateTime), 1, CAST(N'2020-08-10T19:16:51.417' AS DateTime), 9)
INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (5, N'الزنتان', N'Zintan', 1, CAST(N'2020-08-10T16:53:55.947' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (6, N'بنغازي', N'banghzi', 1, CAST(N'2020-08-10T19:11:22.693' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (7, N'زوارة', N'zwara', 1, CAST(N'2020-08-10T19:17:17.763' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (8, N'طرابلس', N'tripoli', 1, CAST(N'2020-08-10T19:17:37.753' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[Regions] ([RegionId], [ArabicName], [EnglishName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Status]) VALUES (9, N'AAA', N'aAAa', 1, CAST(N'2020-10-02T14:11:27.420' AS DateTime), 1, CAST(N'2020-10-05T10:07:24.223' AS DateTime), 9)
SET IDENTITY_INSERT [dbo].[Regions] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [LoginName], [Email], [Password], [Image], [Phone], [BirthDate], [LoginTryAttemptDate], [LoginTryAttempts], [LastLoginOn], [Gender], [UserType], [CreatedBy], [CreatedOn], [ModifiedOn], [ModifiedBy], [State]) VALUES (1, N'Abdullah', N'AbdullahElamir', N'abdullahelameer@gmail.com', N'Gbcut7ILUQ/WWbni+7VPCSZUIjDJyP33zVRYsH8OYq5orYeI1YPdhXMnrI19OsAr2pLgOXNn7mu3Wa77g+MH2KX/eqJ2wEk=', NULL, N'911111296', CAST(N'2019-02-02T00:00:00.000' AS DateTime), NULL, 0, CAST(N'2020-10-07T12:27:07.340' AS DateTime), 1, 1, 1, CAST(N'2010-11-02T00:00:00.000' AS DateTime), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UserType]  DEFAULT ((2)) FOR [UserType]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_State]  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[Branches]  WITH CHECK ADD  CONSTRAINT [FK_Branches_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([ProfileId])
GO
ALTER TABLE [dbo].[Branches] CHECK CONSTRAINT [FK_Branches_Profile]
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'State'
GO
