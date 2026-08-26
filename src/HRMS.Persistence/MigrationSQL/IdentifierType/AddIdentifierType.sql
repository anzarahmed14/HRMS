BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826163046_AddEmergencyContact', N'9.0.11');

CREATE TABLE [EmployeeBankAccounts] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [AccountHolderName] nvarchar(150) NOT NULL,
    [AccountNumber] nvarchar(50) NOT NULL,
    [BankName] nvarchar(150) NOT NULL,
    [IFSCCode] nvarchar(20) NOT NULL,
    [BranchName] nvarchar(150) NULL,
    [AccountType] nvarchar(30) NOT NULL,
    [IsPrimary] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmployeeBankAccounts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeeBankAccounts_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_EmployeeBankAccounts_EmployeeId] ON [EmployeeBankAccounts] ([EmployeeId]) WHERE [IsPrimary] = 1 AND [IsDeleted] = 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826164139_AddEmployeeBankAccount', N'9.0.11');

CREATE TABLE [IdentifierTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(30) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [IsSensitive] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_IdentifierTypes] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'IsSensitive', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[IdentifierTypes]'))
    SET IDENTITY_INSERT [IdentifierTypes] ON;
INSERT INTO [IdentifierTypes] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Description], [IsActive], [IsDeleted], [IsSensitive], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('44000000-0000-0000-0000-000000000001', N'PAN', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Indian PAN identifier.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Permanent Account Number'),
('44000000-0000-0000-0000-000000000002', N'UAN', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Employee UAN.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Universal Account Number'),
('44000000-0000-0000-0000-000000000003', N'PF', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Employee provident fund identifier.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Provident Fund Number'),
('44000000-0000-0000-0000-000000000004', N'ESIC', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Employee ESIC identifier.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'ESIC Number'),
('44000000-0000-0000-0000-000000000005', N'AADHAAR', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Indian Aadhaar identifier.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Aadhaar Number'),
('44000000-0000-0000-0000-000000000006', N'PASSPORT', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Passport identifier.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Passport Number'),
('44000000-0000-0000-0000-000000000007', N'DRIVING_LICENSE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Driving license identifier.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Driving License Number');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'IsSensitive', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[IdentifierTypes]'))
    SET IDENTITY_INSERT [IdentifierTypes] OFF;

CREATE UNIQUE INDEX [IX_IdentifierTypes_Code] ON [IdentifierTypes] ([Code]);

CREATE UNIQUE INDEX [IX_IdentifierTypes_Name] ON [IdentifierTypes] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826170155_AddIdentifierType', N'9.0.11');

COMMIT;
GO

