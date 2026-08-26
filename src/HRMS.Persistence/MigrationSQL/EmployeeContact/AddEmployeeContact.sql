BEGIN TRANSACTION;
CREATE TABLE [EmployeeContacts] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [ContactType] nvarchar(30) NOT NULL,
    [ContactValue] nvarchar(200) NOT NULL,
    [IsPrimary] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmployeeContacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeeContacts_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_EmployeeContacts_EmployeeId] ON [EmployeeContacts] ([EmployeeId]);

CREATE INDEX [IX_EmployeeContacts_EmployeeId_ContactType] ON [EmployeeContacts] ([EmployeeId], [ContactType]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826161024_AddEmployeeContact', N'9.0.11');

COMMIT;
GO

