﻿CREATE TABLE [Data].[Measurements] (
    [ID]                     UNIQUEIDENTIFIER NOT NULL,
    [Temperature]            DECIMAL (18, 2)  NULL,
    [Humidity]               DECIMAL (18, 2)  NULL,
    [SoilMoisturePercentage] DECIMAL (18, 2)  NULL,
    [DeviceID]               UNIQUEIDENTIFIER NOT NULL,
    [MeasuredAtTime]         DATETIME2 (7)    NOT NULL,
    [ReceivedAtTime]         DATETIME2 (7)    NOT NULL,
    [LightPercentage]        DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_Measurements] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Measurements_Devices] FOREIGN KEY ([DeviceID]) REFERENCES [Data].[Devices] ([ID])
);









