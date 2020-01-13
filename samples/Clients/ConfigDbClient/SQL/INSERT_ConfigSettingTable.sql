-- ConfigCore Samples
-- Insert setting values for unit testing and demo client

SET IDENTITY_INSERT [dbo].[ConfigSetting] ON
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (1, N'ConfigDbClient', N'Setting1', N'Setting 1 - Value for ConfigDbClient')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (2, N'ConfigDbClient', N'Setting2', N'Setting 2 - Value for ConfigDbClient')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (3, N'ConfigDbClient', N'Setting3', N'Setting 3 - Value for ConfigDbClient')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (4, N'ConfigDbClient', N'Setting4', N'Setting 4 - Value for ConfigDbClient')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (5, N'ConfigDbClient', N'Setting5', N'Setting 5 - Value for ConfigDbClient')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (6, N'testhost', N'Setting1', N'Setting 1 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (7, N'testhost', N'Setting2', N'Setting 2 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (8, N'testhost', N'Setting3', N'Setting 3 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (9, N'testhost', N'Setting4', N'Setting 4 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (10, N'testhost', N'Setting5', N'Setting 5 - Value for test host')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (11, N'CustomAppName', N'Setting1', N'Setting 1 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (12, N'CustomAppName', N'Setting2', N'Setting 2 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (13, N'CustomAppName', N'Setting3', N'Setting 3 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (14, N'CustomAppName', N'Setting4', N'Setting 4 - Value for CustomAppName')
INSERT INTO [dbo].[ConfigSetting] ([Id], [AppId], [SettingKey], [SettingValue]) VALUES (15, N'CustomAppName', N'Setting5', N'Setting 5 - Value for CustomAppName')
SET IDENTITY_INSERT [dbo].[ConfigSetting] OFF
