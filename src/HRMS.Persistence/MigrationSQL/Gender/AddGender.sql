BEGIN TRANSACTION;
CREATE TABLE [EmployeeGovernmentIdentifiers] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [IdentifierTypeId] uniqueidentifier NOT NULL,
    [IdentifierNumber] nvarchar(100) NOT NULL,
    [IssueDate] date NULL,
    [ExpiryDate] date NULL,
    [IsVerified] bit NOT NULL,
    [VerifiedOn] datetimeoffset NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmployeeGovernmentIdentifiers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeeGovernmentIdentifiers_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmployeeGovernmentIdentifiers_IdentifierTypes_IdentifierTypeId] FOREIGN KEY ([IdentifierTypeId]) REFERENCES [IdentifierTypes] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_EmployeeGovernmentIdentifiers_EmployeeId] ON [EmployeeGovernmentIdentifiers] ([EmployeeId]);

CREATE UNIQUE INDEX [IX_EmployeeGovernmentIdentifiers_EmployeeId_IdentifierTypeId] ON [EmployeeGovernmentIdentifiers] ([EmployeeId], [IdentifierTypeId]) WHERE [IsDeleted] = 0;

CREATE INDEX [IX_EmployeeGovernmentIdentifiers_IdentifierTypeId] ON [EmployeeGovernmentIdentifiers] ([IdentifierTypeId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826170529_AddEmployeeGovernmentIdentifier', N'9.0.11');

CREATE TABLE [Genders] (
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
    CONSTRAINT [PK_Genders] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Genders]'))
    SET IDENTITY_INSERT [Genders] ON;
INSERT INTO [Genders] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Description], [IsActive], [IsDeleted], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('45000000-0000-0000-0000-000000000001', N'MALE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Male', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Male'),
('45000000-0000-0000-0000-000000000002', N'FEMALE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Female', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Female'),
('45000000-0000-0000-0000-000000000003', N'NON_BINARY', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Non-binary', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Non-Binary'),
('45000000-0000-0000-0000-000000000004', N'OTHER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Other', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Other'),
('45000000-0000-0000-0000-000000000005', N'PREFER_NOT_TO_SAY', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Employee prefers not to disclose.', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Prefer Not To Say');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Genders]'))
    SET IDENTITY_INSERT [Genders] OFF;

CREATE UNIQUE INDEX [IX_Genders_Code] ON [Genders] ([Code]);

CREATE UNIQUE INDEX [IX_Genders_Name] ON [Genders] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826171737_AddGender', N'9.0.11');

COMMIT;
GO

