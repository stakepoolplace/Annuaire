USE [Annuaire]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 07/03/2025 21:43:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SocieteId] [int] NOT NULL,
	[Civilite] [nvarchar](max) NULL,
	[Nom] [nvarchar](max) NULL,
	[Prenom] [nvarchar](max) NULL,
	[Fonction] [nvarchar](max) NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InfoContacts]    Script Date: 07/03/2025 21:43:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InfoContacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TypeInfo] [nvarchar](max) NULL,
	[Info] [nvarchar](max) NULL,
	[ContactId] [int] NOT NULL,
 CONSTRAINT [PK_InfoContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Societes]    Script Date: 07/03/2025 21:43:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Societes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nom] [nvarchar](max) NULL,
	[Adresse] [nvarchar](max) NULL,
	[Adresse2] [nvarchar](max) NULL,
	[CodePostal] [nvarchar](max) NULL,
	[Ville] [nvarchar](max) NULL,
	[TelStandard] [nvarchar](max) NULL,
 CONSTRAINT [PK_Societes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Contacts] ON 
GO
INSERT [dbo].[Contacts] ([Id], [SocieteId], [Civilite], [Nom], [Prenom], [Fonction]) VALUES (1, 1, N'M.', N'Dupont', N'Jean', N'Directeur')
GO
INSERT [dbo].[Contacts] ([Id], [SocieteId], [Civilite], [Nom], [Prenom], [Fonction]) VALUES (2, 1, N'Mme', N'Martin', N'Sophie', N'Responsable RH')
GO
INSERT [dbo].[Contacts] ([Id], [SocieteId], [Civilite], [Nom], [Prenom], [Fonction]) VALUES (3, 2, N'M.', N'Durand', N'Pierre', N'Commercial')
GO
INSERT [dbo].[Contacts] ([Id], [SocieteId], [Civilite], [Nom], [Prenom], [Fonction]) VALUES (4, 2, N'Mme', N'Leroy', N'Marie', N'Comptable')
GO
SET IDENTITY_INSERT [dbo].[Contacts] OFF
GO
SET IDENTITY_INSERT [dbo].[InfoContacts] ON 
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (1, N'Email', N'jean.dupont@societea.com', 1)
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (2, N'Téléphone', N'0102030405', 1)
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (3, N'Email', N'sophie.martin@societea.com', 2)
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (4, N'Téléphone', N'0607080910', 2)
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (5, N'Email', N'pierre.durand@societeb.com', 3)
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (6, N'Téléphone', N'0506070809', 3)
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (7, N'Email', N'marie.leroy@societeb.com', 4)
GO
INSERT [dbo].[InfoContacts] ([Id], [TypeInfo], [Info], [ContactId]) VALUES (8, N'Téléphone', N'0405060708', 4)
GO
SET IDENTITY_INSERT [dbo].[InfoContacts] OFF
GO
SET IDENTITY_INSERT [dbo].[Societes] ON 
GO
INSERT [dbo].[Societes] ([Id], [Nom], [Adresse], [Adresse2], [CodePostal], [Ville], [TelStandard]) VALUES (1, N'Société A', N'Adresse 1', N'Adresse 2', N'75001', N'Paris', N'0102030405')
GO
INSERT [dbo].[Societes] ([Id], [Nom], [Adresse], [Adresse2], [CodePostal], [Ville], [TelStandard]) VALUES (2, N'Société B', N'Adresse 3', N'Adresse 4', N'69001', N'Lyon', N'0607080910')
GO
SET IDENTITY_INSERT [dbo].[Societes] OFF
GO
ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD  CONSTRAINT [FK_Contacts_Societes_SocieteId] FOREIGN KEY([SocieteId])
REFERENCES [dbo].[Societes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_Contacts_Societes_SocieteId]
GO
ALTER TABLE [dbo].[InfoContacts]  WITH CHECK ADD  CONSTRAINT [FK_InfoContacts_Contacts_ContactId] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InfoContacts] CHECK CONSTRAINT [FK_InfoContacts_Contacts_ContactId]
GO
