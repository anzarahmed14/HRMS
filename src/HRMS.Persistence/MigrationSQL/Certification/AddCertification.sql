BEGIN TRANSACTION;
CREATE TABLE [Certifications] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [IssuingOrganization] nvarchar(150) NULL,
    [Description] nvarchar(500) NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Certifications] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'IssuingOrganization', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Certifications]'))
    SET IDENTITY_INSERT [Certifications] ON;
INSERT INTO [Certifications] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Description], [IsActive], [IsDeleted], [IssuingOrganization], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('48000000-0000-0000-0000-000000000001', N'AZURE_ADMINISTRATOR', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'Microsoft', NULL, NULL, N'Microsoft Certified: Azure Administrator Associate'),
('48000000-0000-0000-0000-000000000002', N'AZURE_DEVELOPER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'Microsoft', NULL, NULL, N'Microsoft Certified: Azure Developer Associate'),
('48000000-0000-0000-0000-000000000003', N'AWS_DEVELOPER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'Amazon Web Services', NULL, NULL, N'AWS Certified Developer'),
('48000000-0000-0000-0000-000000000004', N'AWS_SOLUTIONS_ARCHITECT', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'Amazon Web Services', NULL, NULL, N'AWS Certified Solutions Architect'),
('48000000-0000-0000-0000-000000000005', N'CSHARP', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'Microsoft', NULL, NULL, N'C# Certification'),
('48000000-0000-0000-0000-000000000006', N'JAVA', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'Oracle', NULL, NULL, N'Java Certification'),
('48000000-0000-0000-0000-000000000007', N'PMP', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'PMI', NULL, NULL, N'Project Management Professional'),
('48000000-0000-0000-0000-000000000008', N'SCRUM_MASTER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N'Scrum Alliance', NULL, NULL, N'Certified ScrumMaster');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'IssuingOrganization', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[Certifications]'))
    SET IDENTITY_INSERT [Certifications] OFF;

CREATE UNIQUE INDEX [IX_Certifications_Code] ON [Certifications] ([Code]);

CREATE UNIQUE INDEX [IX_Certifications_Name] ON [Certifications] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260831080549_AddCertification', N'9.0.11');

COMMIT;
GO

