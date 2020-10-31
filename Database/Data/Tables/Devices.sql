CREATE TABLE [Data].[Devices] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [SerialNumber]   NVARCHAR (50)    NULL,
    [DateRegistered] DATETIME2 (7)    NULL,
    CONSTRAINT [PK_Devices] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);



