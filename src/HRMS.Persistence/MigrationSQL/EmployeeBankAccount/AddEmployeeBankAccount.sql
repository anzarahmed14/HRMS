BEGIN TRANSACTION;
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

COMMIT;
GO

