use ConfigDb
truncate table configsetting
SET IDENTITY_INSERT [dbo].[ConfigSetting] ON
--DB Config Client Samples
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (1, N'ConfigClient_DbDefault', N'Setting1',  N'Setting 1 - Value for ConfigClient_DbDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (2, N'ConfigClient_DbDefault', N'Setting2',  N'Setting 2 - Value for ConfigClient_DbDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (3, N'ConfigClient_DbDefault', N'Setting3',  N'Setting 3 - Value for ConfigClient_DbDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (4, N'ConfigClient_DbDefault', N'Setting4',  N'Setting 4 - Value for ConfigClient_DbDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (5, N'ConfigClient_DbDefault', N'Setting5',  N'Setting 5 - Value for ConfigClient_DbDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (6, N'ConfigClient_DbCustom', N'Setting1',   N'Setting 1 - Value for ConfigClient_DbCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (7, N'ConfigClient_DbCustom', N'Setting2',   N'Setting 2 - Value for ConfigClient_DbCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (8, N'ConfigClient_DbCustom', N'Setting3',   N'Setting 3 - Value for ConfigClient_DbCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (9, N'ConfigClient_DbCustom', N'Setting4',   N'Setting 4 - Value for ConfigClient_DbCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (10, N'ConfigClient_DbCustom', N'Setting5',  N'Setting 5 - Value for ConfigClient_DbCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (11, N'ConfigClient_DbStartup', N'Setting1', N'Setting 1 - Value for ConfigClient_DbStartup')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (12, N'ConfigClient_DbStartup', N'Setting2', N'Setting 2 - Value for ConfigClient_DbStartup')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (13, N'ConfigClient_DbStartup', N'Setting3', N'Setting 3 - Value for ConfigClient_DbStartup')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (14, N'ConfigClient_DbStartup', N'Setting4', N'Setting 4 - Value for ConfigClient_DbStartup')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (15, N'ConfigClient_DbStartup', N'Setting5', N'Setting 5 - Value for ConfigClient_DbStartup')

--Api Client Samples
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (16, N'ConfigClient_ApiDefault', N'Setting1', N'Setting 1 - Value for ConfigClient_ApiDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (17, N'ConfigClient_ApiDefault', N'Setting2', N'Setting 2 - Value for ConfigClient_ApiDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (18, N'ConfigClient_ApiDefault', N'Setting3', N'Setting 3 - Value for ConfigClient_ApiDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (19, N'ConfigClient_ApiDefault', N'Setting4', N'Setting 4 - Value for ConfigClient_ApiDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (20, N'ConfigClient_ApiDefault', N'Setting5', N'Setting 5 - Value for ConfigClient_ApiDefault')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (21, N'ConfigClient_ApiCustom', N'Setting1', N'Setting 1 - Value for ConfigClient_ApiCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (22, N'ConfigClient_ApiCustom', N'Setting2', N'Setting 2 - Value for ConfigClient_ApiCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (23, N'ConfigClient_ApiCustom', N'Setting3', N'Setting 3 - Value for ConfigClient_ApiCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (24, N'ConfigClient_ApiCustom', N'Setting4', N'Setting 4 - Value for ConfigClient_ApiCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (25, N'ConfigClient_ApiCustom', N'Setting5', N'Setting 5 - Value for ConfigClient_ApiCustom')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (27, N'ConfigClient_ApiStartup', N'Setting2', N'Setting 2 - Value for ConfigClient_ApiStartup')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (28, N'ConfigClient_ApiStartup', N'Setting3', N'Setting 3 - Value for ConfigClient_ApiStartup')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (29, N'ConfigClient_ApiStartup', N'Setting4', N'Setting 4 - Value for ConfigClient_ApiStartup')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (30, N'ConfigClient_ApiStartup', N'Setting5', N'Setting 5 - Value for ConfigClient_ApiStartup')


-- Unit testing samples
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (31, N'testhost', N'Setting1', N'Setting 1 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (32, N'testhost', N'Setting2', N'Setting 2 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (33, N'testhost', N'Setting3', N'Setting 3 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (34, N'testhost', N'Setting4', N'Setting 4 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (35, N'testhost', N'Setting5', N'Setting 5 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (36, N'CustomAppName', N'Setting1', N'Setting 1 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (37, N'CustomAppName', N'Setting2', N'Setting 2 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (38, N'CustomAppName', N'Setting3', N'Setting 3 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (39, N'CustomAppName', N'Setting4', N'Setting 4 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (40, N'CustomAppName', N'Setting5', N'Setting 5 - Value for CustomAppName')
SET IDENTITY_INSERT [dbo].[ConfigSetting] OFF