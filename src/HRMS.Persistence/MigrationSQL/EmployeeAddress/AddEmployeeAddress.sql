BEGIN TRANSACTION;
CREATE TABLE [EmployeeAddresses] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [AddressTypeId] uniqueidentifier NOT NULL,
    [CountryId] uniqueidentifier NOT NULL,
    [StateId] uniqueidentifier NOT NULL,
    [AddressLine1] nvarchar(250) NOT NULL,
    [AddressLine2] nvarchar(250) NULL,
    [City] nvarchar(100) NOT NULL,
    [PostalCode] nvarchar(20) NOT NULL,
    [IsPrimary] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmployeeAddresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeeAddresses_AddressTypes_AddressTypeId] FOREIGN KEY ([AddressTypeId]) REFERENCES [AddressTypes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmployeeAddresses_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmployeeAddresses_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmployeeAddresses_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_EmployeeAddresses_AddressTypeId] ON [EmployeeAddresses] ([AddressTypeId]);

CREATE INDEX [IX_EmployeeAddresses_CountryId] ON [EmployeeAddresses] ([CountryId]);

CREATE INDEX [IX_EmployeeAddresses_EmployeeId] ON [EmployeeAddresses] ([EmployeeId]);

CREATE INDEX [IX_EmployeeAddresses_StateId] ON [EmployeeAddresses] ([StateId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826154702_AddEmployeeAddress', N'9.0.11');

COMMIT;
GO

