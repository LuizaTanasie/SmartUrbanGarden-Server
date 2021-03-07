CREATE TABLE [Data].[Devices] (
    [ID]                 UNIQUEIDENTIFIER NOT NULL,
    [SerialNumber]       NVARCHAR (50)    NULL,
    [DateRegistered]     DATETIME2 (7)    NULL,
    [HowMuchWaterID]     INT              NULL,
    [HowMuchLightID]     INT              NULL,
    [HowMuchHumidityID]  INT              NULL,
    [IdealTemperatureID] INT              NULL,
    [PlantName]          NVARCHAR (50)    NULL,
    [PlantSpecies]       NVARCHAR (100)   NULL,
    CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Devices_MeasurementIdealAmounts] FOREIGN KEY ([HowMuchWaterID]) REFERENCES [Catalogs].[MeasurementIdealAmounts] ([ID]),
    CONSTRAINT [FK_Devices_MeasurementIdealAmounts1] FOREIGN KEY ([HowMuchLightID]) REFERENCES [Catalogs].[MeasurementIdealAmounts] ([ID]),
    CONSTRAINT [FK_Devices_MeasurementIdealAmounts2] FOREIGN KEY ([HowMuchHumidityID]) REFERENCES [Catalogs].[MeasurementIdealAmounts] ([ID]),
    CONSTRAINT [FK_Devices_MeasurementIdealAmounts3] FOREIGN KEY ([IdealTemperatureID]) REFERENCES [Catalogs].[MeasurementIdealAmounts] ([ID])
);











