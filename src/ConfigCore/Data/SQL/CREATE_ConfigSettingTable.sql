USE [ConfigDb]
GO

/****** Object: Table [dbo].[ConfigSetting] Script Date: 11/29/2019 9:22:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

