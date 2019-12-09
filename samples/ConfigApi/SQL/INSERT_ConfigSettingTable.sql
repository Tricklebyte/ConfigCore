USE ConfigDb
TRUNCATE TABLE configsetting
SET IDENTITY_INSERT [dbo].[ConfigSetting] ON
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (1, N'DbConfigClient_DefaultDb', N'Setting1', N'Setting 1 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (2, N'DbConfigClient_DefaultDb', N'Setting2', N'Setting 2 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (3, N'DbConfigClient_DefaultDb', N'Setting3', N'Setting 3 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (4, N'DbConfigClient_DefaultDb', N'Setting4', N'Setting 4 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (5, N'DbConfigClient_DefaultDb', N'Setting5', N'Setting 5 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (6, N'DbConfigClient_CustomDb', N'Setting1', N'Setting 1 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (7, N'DbConfigClient_CustomDb', N'Setting2', N'Setting 2 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (8, N'DbConfigClient_CustomDb', N'Setting3', N'Setting 3 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (9, N'DbConfigClient_CustomDb', N'Setting4', N'Setting 4 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (10, N'DbConfigClient_CustomDb', N'Setting5', N'Setting 5 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (11, N'test_host', N'Setting1', N'Setting 1 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (12, N'test_host', N'Setting2', N'Setting 2 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (13, N'test_host', N'Setting3', N'Setting 3 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (14, N'test_host', N'Setting4', N'Setting 4 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (15, N'test_host', N'Setting5', N'Setting 5 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (16, N'CustomAppName', N'Setting1', N'Setting 1 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (17, N'CustomAppName', N'Setting2', N'Setting 2 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (18, N'CustomAppName', N'Setting3', N'Setting 3 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (19, N'CustomAppName', N'Setting4', N'Setting 4 - DBSource Value')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (20, N'CustomAppName', N'Setting5', N'Setting 5 - DBSource Value')

SET IDENTITY_INSERT [dbo].[ConfigSetting] OFF