CREATE TABLE [Data].[DeviceUsers] (
    [DeviceID]                      UNIQUEIDENTIFIER NOT NULL,
    [Email]                         NVARCHAR (100)   NOT NULL,
    [TemperatureWarningEmailSentAt] DATETIME2 (7)    NULL,
    [HumidityWarningEmailSentAt]    DATETIME2 (7)    NULL,
    [SoilWarningEmailSentAt]        DATETIME2 (7)    NULL,
    [LightWarningEmailSentAt]       DATETIME2 (7)    NULL,
    CONSTRAINT [PK_DeviceUsers] PRIMARY KEY NONCLUSTERED ([DeviceID] ASC, [Email] ASC),
    CONSTRAINT [FK_DeviceUsers_Devices] FOREIGN KEY ([DeviceID]) REFERENCES [Data].[Devices] ([ID])
);



