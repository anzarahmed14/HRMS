BEGIN TRANSACTION;
CREATE TABLE [EmploymentStatuses] (
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
    CONSTRAINT [PK_EmploymentStatuses] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_EmploymentStatuses_Code] ON [EmploymentStatuses] ([Code]);

CREATE UNIQUE INDEX [IX_EmploymentStatuses_Name] ON [EmploymentStatuses] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826105022_AddEmploymentStatus', N'9.0.11');

COMMIT;
GO

