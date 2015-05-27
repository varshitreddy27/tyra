/****** Object:  Table [dbo].[SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemApplication](
	[ApplicationId] [int] NOT NULL,
	[ApplicationName] [varchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Module]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Module](
	[ApplicationId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[LabelText] [varchar](50) NULL,
	[OrderIndex] [int] NULL,
	[Remarks] [varchar](300) NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Module_1] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique id to identify a module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Module', @level2type=N'COLUMN',@level2name=N'ModuleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lable text. this can be used in menus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Module', @level2type=N'COLUMN',@level2name=N'LabelText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'List of sales3 application modules' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Module'
GO
/****** Object:  Table [dbo].[Feature]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Feature](
	[ApplicationId] [int] NOT NULL,
	[FeatureId] [int] NOT NULL,
	[LabelText] [varchar](50) NULL,
	[Remark] [varchar](300) NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Feature_1] PRIMARY KEY CLUSTERED 
(
	[FeatureId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'List of sales3 application sub module features' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Feature'
GO
/****** Object:  Table [dbo].[SubModule]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubModule](
	[ApplicationId] [int] NOT NULL,
	[SubModuleId] [int] NOT NULL,
	[LabelText] [varchar](50) NULL,
	[Remarks] [varchar](300) NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SubModule_1] PRIMARY KEY CLUSTERED 
(
	[SubModuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[ApplicationId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Role_1] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SubModuleFeature]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubModuleFeature](
	[SubModuleId] [int] NOT NULL,
	[FeatureId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SubModuleFeature_1] PRIMARY KEY CLUSTERED 
(
	[SubModuleId] ASC,
	[FeatureId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'List of sales3 application sub module and it''s features' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubModuleFeature'
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[ApplicationId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserRole_1] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleSubModule]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleSubModule](
	[ModuleId] [int] NOT NULL,
	[SubModuleId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[OrderIndex] [int] NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ModuleSubModule_1] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC,
	[SubModuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleManagement]    Script Date: 07/19/2011 11:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RoleManagement](
	[ManageId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[SubModuleId] [int] NULL,
	[FeatureId] [int] NULL,
	[Action] [varchar](3) NULL,
	[Created] [datetime] NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdaterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RoleManagement] PRIMARY KEY CLUSTERED 
(
	[ManageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'It was hard to set composit key in this table. so i went with this key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleManagement', @level2type=N'COLUMN',@level2name=N'ManageId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'if the value is "1" then full access. value "2" means read only access' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleManagement', @level2type=N'COLUMN',@level2name=N'Action'
GO
/****** Object:  Default [DF_RoleManagement_ManageId]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[RoleManagement] ADD  CONSTRAINT [DF_RoleManagement_ManageId]  DEFAULT (newid()) FOR [ManageId]
GO
/****** Object:  ForeignKey [FK_Module_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[Module]  WITH CHECK ADD  CONSTRAINT [FK_Module_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [FK_Module_SystemApplication]
GO
/****** Object:  ForeignKey [FK_ModuleSubModule_Module]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[ModuleSubModule]  WITH CHECK ADD  CONSTRAINT [FK_ModuleSubModule_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
GO
ALTER TABLE [dbo].[ModuleSubModule] CHECK CONSTRAINT [FK_ModuleSubModule_Module]
GO
/****** Object:  ForeignKey [FK_ModuleSubModule_SubModule]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[ModuleSubModule]  WITH CHECK ADD  CONSTRAINT [FK_ModuleSubModule_SubModule] FOREIGN KEY([SubModuleId])
REFERENCES [dbo].[SubModule] ([SubModuleId])
GO
ALTER TABLE [dbo].[ModuleSubModule] CHECK CONSTRAINT [FK_ModuleSubModule_SubModule]
GO
/****** Object:  ForeignKey [FK_ModuleSubModule_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[ModuleSubModule]  WITH CHECK ADD  CONSTRAINT [FK_ModuleSubModule_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[ModuleSubModule] CHECK CONSTRAINT [FK_ModuleSubModule_SystemApplication]
GO
/****** Object:  ForeignKey [FK_SubModule_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[SubModule]  WITH CHECK ADD  CONSTRAINT [FK_SubModule_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[SubModule] CHECK CONSTRAINT [FK_SubModule_SystemApplication]
GO
/****** Object:  ForeignKey [FK_SubModuleFeature_Feature]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[SubModuleFeature]  WITH CHECK ADD  CONSTRAINT [FK_SubModuleFeature_Feature] FOREIGN KEY([FeatureId])
REFERENCES [dbo].[Feature] ([FeatureId])
GO
ALTER TABLE [dbo].[SubModuleFeature] CHECK CONSTRAINT [FK_SubModuleFeature_Feature]
GO
/****** Object:  ForeignKey [FK_SubModuleFeature_SubModule]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[SubModuleFeature]  WITH CHECK ADD  CONSTRAINT [FK_SubModuleFeature_SubModule] FOREIGN KEY([SubModuleId])
REFERENCES [dbo].[SubModule] ([SubModuleId])
GO
ALTER TABLE [dbo].[SubModuleFeature] CHECK CONSTRAINT [FK_SubModuleFeature_SubModule]
GO
/****** Object:  ForeignKey [FK_SubModuleFeature_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[SubModuleFeature]  WITH CHECK ADD  CONSTRAINT [FK_SubModuleFeature_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[SubModuleFeature] CHECK CONSTRAINT [FK_SubModuleFeature_SystemApplication]
GO
/****** Object:  ForeignKey [FK_Feature_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[Feature]  WITH CHECK ADD  CONSTRAINT [FK_Feature_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[Feature] CHECK CONSTRAINT [FK_Feature_SystemApplication]
GO
/****** Object:  ForeignKey [FK_Role_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[Role]  WITH CHECK ADD  CONSTRAINT [FK_Role_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[Role] CHECK CONSTRAINT [FK_Role_SystemApplication]
GO
/****** Object:  ForeignKey [FK_RoleManagement_Feature]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[RoleManagement]  WITH CHECK ADD  CONSTRAINT [FK_RoleManagement_Feature] FOREIGN KEY([FeatureId])
REFERENCES [dbo].[Feature] ([FeatureId])
GO
ALTER TABLE [dbo].[RoleManagement] CHECK CONSTRAINT [FK_RoleManagement_Feature]
GO
/****** Object:  ForeignKey [FK_RoleManagement_Module]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[RoleManagement]  WITH CHECK ADD  CONSTRAINT [FK_RoleManagement_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
GO
ALTER TABLE [dbo].[RoleManagement] CHECK CONSTRAINT [FK_RoleManagement_Module]
GO
/****** Object:  ForeignKey [FK_RoleManagement_Role]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[RoleManagement]  WITH CHECK ADD  CONSTRAINT [FK_RoleManagement_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[RoleManagement] CHECK CONSTRAINT [FK_RoleManagement_Role]
GO
/****** Object:  ForeignKey [FK_RoleManagement_SubModule]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[RoleManagement]  WITH CHECK ADD  CONSTRAINT [FK_RoleManagement_SubModule] FOREIGN KEY([SubModuleId])
REFERENCES [dbo].[SubModule] ([SubModuleId])
GO
ALTER TABLE [dbo].[RoleManagement] CHECK CONSTRAINT [FK_RoleManagement_SubModule]
GO
/****** Object:  ForeignKey [FK_RoleManagement_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[RoleManagement]  WITH CHECK ADD  CONSTRAINT [FK_RoleManagement_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[RoleManagement] CHECK CONSTRAINT [FK_RoleManagement_SystemApplication]
GO
/****** Object:  ForeignKey [FK_UserRole_Role]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO
/****** Object:  ForeignKey [FK_UserRole_SystemApplication]    Script Date: 07/19/2011 11:10:19 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_SystemApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[SystemApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_SystemApplication]
GO
