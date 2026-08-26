BEGIN TRANSACTION;
CREATE TABLE [Countries] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(10) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Countries]'))
    SET IDENTITY_INSERT [Countries] ON;
INSERT INTO [Countries] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [IsActive], [IsDeleted], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('41000000-0000-0000-0000-000000000001', N'IN', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'India'),
('41000000-0000-0000-0000-000000000002', N'US', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'United States'),
('41000000-0000-0000-0000-000000000003', N'GB', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'United Kingdom'),
('41000000-0000-0000-0000-000000000004', N'AE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'United Arab Emirates');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Countries]'))
    SET IDENTITY_INSERT [Countries] OFF;

CREATE UNIQUE INDEX [IX_Countries_Code] ON [Countries] ([Code]);

CREATE UNIQUE INDEX [IX_Countries_Name] ON [Countries] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826153337_AddCountry', N'9.0.11');

COMMIT;
GO

