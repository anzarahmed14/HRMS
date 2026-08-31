BEGIN TRANSACTION;
CREATE TABLE [EmployeeExperiences] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [CompanyName] nvarchar(200) NOT NULL,
    [JobTitle] nvarchar(150) NOT NULL,
    [EmploymentType] nvarchar(50) NOT NULL,
    [StartDate] date NOT NULL,
    [EndDate] date NULL,
    [Location] nvarchar(150) NULL,
    [Responsibilities] nvarchar(2000) NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmployeeExperiences] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeeExperiences_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_EmployeeExperiences_EmployeeId] ON [EmployeeExperiences] ([EmployeeId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260831074007_AddEmployeeExperience', N'9.0.11');

COMMIT;
GO

