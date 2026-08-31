BEGIN TRANSACTION;
CREATE TABLE [EmployeeDependents] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [RelationshipId] uniqueidentifier NOT NULL,
    [GenderId] uniqueidentifier NOT NULL,
    [DateOfBirth] date NOT NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Email] nvarchar(200) NULL,
    [IsDependent] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_EmployeeDependents] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeeDependents_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmployeeDependents_Genders_GenderId] FOREIGN KEY ([GenderId]) REFERENCES [Genders] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EmployeeDependents_Relationships_RelationshipId] FOREIGN KEY ([RelationshipId]) REFERENCES [Relationships] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_EmployeeDependents_EmployeeId] ON [EmployeeDependents] ([EmployeeId]);

CREATE INDEX [IX_EmployeeDependents_GenderId] ON [EmployeeDependents] ([GenderId]);

CREATE INDEX [IX_EmployeeDependents_RelationshipId] ON [EmployeeDependents] ([RelationshipId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260830184550_AddEmployeeDependent', N'9.0.11');

COMMIT;
GO

