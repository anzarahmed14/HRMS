BEGIN TRANSACTION;
CREATE TABLE [MaritalStatuses] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(30) NOT NULL,
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
    CONSTRAINT [PK_MaritalStatuses] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[MaritalStatuses]'))
    SET IDENTITY_INSERT [MaritalStatuses] ON;
INSERT INTO [MaritalStatuses] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Description], [IsActive], [IsDeleted], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('46000000-0000-0000-0000-000000000001', N'SINGLE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Single', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Single'),
('46000000-0000-0000-0000-000000000002', N'MARRIED', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Married', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Married'),
('46000000-0000-0000-0000-000000000003', N'DIVORCED', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Divorced', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Divorced'),
('46000000-0000-0000-0000-000000000004', N'WIDOWED', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Widowed', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Widowed'),
('46000000-0000-0000-0000-000000000005', N'SEPARATED', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Separated', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Separated'),
('46000000-0000-0000-0000-000000000006', N'PREFER_NOT_TO_SAY', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Employee prefers not to disclose.', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Prefer Not To Say');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[MaritalStatuses]'))
    SET IDENTITY_INSERT [MaritalStatuses] OFF;

CREATE UNIQUE INDEX [IX_MaritalStatuses_Code] ON [MaritalStatuses] ([Code]);

CREATE UNIQUE INDEX [IX_MaritalStatuses_Name] ON [MaritalStatuses] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826172009_AddMaritalStatus', N'9.0.11');

COMMIT;
GO

