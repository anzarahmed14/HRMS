BEGIN TRANSACTION;
CREATE TABLE [AddressTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(20) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_AddressTypes] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[AddressTypes]'))
    SET IDENTITY_INSERT [AddressTypes] ON;
INSERT INTO [AddressTypes] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Description], [IsActive], [IsDeleted], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('40000000-0000-0000-0000-000000000001', N'CURRENT', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Current residential address', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Current'),
('40000000-0000-0000-0000-000000000002', N'PERMANENT', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Permanent residential address', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Permanent'),
('40000000-0000-0000-0000-000000000003', N'TEMPORARY', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Temporary residential address', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Temporary');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[AddressTypes]'))
    SET IDENTITY_INSERT [AddressTypes] OFF;

CREATE UNIQUE INDEX [IX_AddressTypes_Code] ON [AddressTypes] ([Code]);

CREATE UNIQUE INDEX [IX_AddressTypes_Name] ON [AddressTypes] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826152827_AddAddressType', N'9.0.11');

COMMIT;
GO

