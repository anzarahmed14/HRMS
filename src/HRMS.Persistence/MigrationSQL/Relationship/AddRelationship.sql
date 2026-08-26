BEGIN TRANSACTION;
CREATE TABLE [Relationships] (
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
    CONSTRAINT [PK_Relationships] PRIMARY KEY ([Id])
);

CREATE TABLE [EmergencyContacts] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [RelationshipId] uniqueidentifier NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [AlternatePhoneNumber] nvarchar(20) NULL,
    [Email] nvarchar(200) NULL,
    [IsPrimary] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmergencyContacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmergencyContacts_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmergencyContacts_Relationships_RelationshipId] FOREIGN KEY ([RelationshipId]) REFERENCES [Relationships] ([Id]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Relationships]'))
    SET IDENTITY_INSERT [Relationships] ON;
INSERT INTO [Relationships] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Description], [IsActive], [IsDeleted], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('43000000-0000-0000-0000-000000000001', N'SPOUSE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Spouse', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Spouse'),
('43000000-0000-0000-0000-000000000002', N'FATHER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Father', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Father'),
('43000000-0000-0000-0000-000000000003', N'MOTHER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Mother', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Mother'),
('43000000-0000-0000-0000-000000000004', N'BROTHER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Brother', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Brother'),
('43000000-0000-0000-0000-000000000005', N'SISTER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Sister', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Sister'),
('43000000-0000-0000-0000-000000000006', N'SON', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Son', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Son'),
('43000000-0000-0000-0000-000000000007', N'DAUGHTER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Daughter', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Daughter'),
('43000000-0000-0000-0000-000000000008', N'OTHER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Other relationship', CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Other');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Relationships]'))
    SET IDENTITY_INSERT [Relationships] OFF;

CREATE INDEX [IX_EmergencyContacts_EmployeeId] ON [EmergencyContacts] ([EmployeeId]);

CREATE INDEX [IX_EmergencyContacts_RelationshipId] ON [EmergencyContacts] ([RelationshipId]);

CREATE UNIQUE INDEX [IX_Relationships_Code] ON [Relationships] ([Code]);

CREATE UNIQUE INDEX [IX_Relationships_Name] ON [Relationships] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826162347_AddRelationship', N'9.0.11');

COMMIT;
GO

