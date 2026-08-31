BEGIN TRANSACTION;
CREATE TABLE [EmployeeDocuments] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [DocumentTypeId] uniqueidentifier NOT NULL,
    [DocumentName] nvarchar(150) NOT NULL,
    [FileName] nvarchar(255) NOT NULL,
    [StorageKey] nvarchar(500) NOT NULL,
    [ContentType] nvarchar(100) NOT NULL,
    [FileSize] bigint NOT NULL,
    [UploadedOn] datetimeoffset NOT NULL,
    [IsVerified] bit NOT NULL,
    [VerifiedOn] datetimeoffset NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmployeeDocuments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeeDocuments_DocumentTypes_DocumentTypeId] FOREIGN KEY ([DocumentTypeId]) REFERENCES [DocumentTypes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmployeeDocuments_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_EmployeeDocuments_DocumentTypeId] ON [EmployeeDocuments] ([DocumentTypeId]);

CREATE INDEX [IX_EmployeeDocuments_EmployeeId] ON [EmployeeDocuments] ([EmployeeId]);

CREATE UNIQUE INDEX [IX_EmployeeDocuments_StorageKey] ON [EmployeeDocuments] ([StorageKey]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260831071210_AddEmployeeDocument', N'9.0.11');

COMMIT;
GO

