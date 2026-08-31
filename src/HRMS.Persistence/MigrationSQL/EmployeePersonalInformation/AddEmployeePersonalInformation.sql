BEGIN TRANSACTION;
ALTER TABLE [Employees] ADD [GenderId] uniqueidentifier NULL;

ALTER TABLE [Employees] ADD [MaritalStatusId] uniqueidentifier NULL;

UPDATE Employees
SET GenderId = '45000000-0000-0000-0000-000000000005'
WHERE GenderId IS NULL;

UPDATE Employees
SET MaritalStatusId = '46000000-0000-0000-0000-000000000006'
WHERE MaritalStatusId IS NULL;

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'GenderId');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Employees] ALTER COLUMN [GenderId] uniqueidentifier NOT NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'MaritalStatusId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Employees] ALTER COLUMN [MaritalStatusId] uniqueidentifier NOT NULL;

CREATE INDEX [IX_Employees_GenderId] ON [Employees] ([GenderId]);

CREATE INDEX [IX_Employees_MaritalStatusId] ON [Employees] ([MaritalStatusId]);

ALTER TABLE [Employees] ADD CONSTRAINT [FK_Employees_Genders_GenderId] FOREIGN KEY ([GenderId]) REFERENCES [Genders] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Employees] ADD CONSTRAINT [FK_Employees_MaritalStatuses_MaritalStatusId] FOREIGN KEY ([MaritalStatusId]) REFERENCES [MaritalStatuses] ([Id]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260830180149_AddEmployeePersonalInformation', N'9.0.11');

COMMIT;
GO

