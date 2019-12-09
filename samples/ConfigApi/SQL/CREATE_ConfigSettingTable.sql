USE [ConfigDb]
GO

CREATE TABLE [dbo].[ConfigSetting] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [AppId]        NVARCHAR (150) NOT NULL,
    [SettingKey]   NVARCHAR (150) NOT NULL,
    [SettingValue] NVARCHAR (500) NOT NULL,
	CONSTRAINT [UQ_AppIdSettingKey] UNIQUE NONCLUSTERED
    (
        [appId], [SettingKey]
    )
);

