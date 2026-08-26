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

COMMIT;
GO

