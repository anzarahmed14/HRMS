BEGIN TRANSACTION;
CREATE TABLE [DocumentTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(40) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
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
    CONSTRAINT [PK_DocumentTypes] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'IsSensitive', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] ON;
INSERT INTO [DocumentTypes] ([Id], [Code], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Description], [IsActive], [IsDeleted], [IsSensitive], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('47000000-0000-0000-0000-000000000001', N'PAN_CARD', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Permanent Account Number document.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'PAN Card'),
('47000000-0000-0000-0000-000000000002', N'AADHAAR_CARD', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Aadhaar identity document.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Aadhaar Card'),
('47000000-0000-0000-0000-000000000003', N'PASSPORT', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Passport document.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Passport'),
('47000000-0000-0000-0000-000000000004', N'DRIVING_LICENSE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Driving license document.', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, NULL, N'Driving License'),
('47000000-0000-0000-0000-000000000005', N'ADDRESS_PROOF', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Proof of residential address.', CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, NULL, N'Address Proof'),
('47000000-0000-0000-0000-000000000006', N'OFFER_LETTER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Employee offer letter.', CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, NULL, N'Offer Letter'),
('47000000-0000-0000-0000-000000000007', N'JOINING_LETTER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Employee joining letter.', CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, NULL, N'Joining Letter'),
('47000000-0000-0000-0000-000000000008', N'EXPERIENCE_LETTER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Previous employment experience letter.', CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, NULL, N'Experience Letter'),
('47000000-0000-0000-0000-000000000009', N'EDUCATION_CERTIFICATE', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Educational qualification certificate.', CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, NULL, N'Education Certificate'),
('47000000-0000-0000-0000-000000000010', N'OTHER', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, N'Other employee document.', CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, NULL, N'Other');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Description', N'IsActive', N'IsDeleted', N'IsSensitive', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] OFF;

CREATE UNIQUE INDEX [IX_DocumentTypes_Code] ON [DocumentTypes] ([Code]);

CREATE UNIQUE INDEX [IX_DocumentTypes_Name] ON [DocumentTypes] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260831070026_AddDocumentType', N'9.0.11');

COMMIT;
GO

