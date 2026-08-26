BEGIN TRANSACTION;
CREATE TABLE [States] (
    [Id] uniqueidentifier NOT NULL,
    [CountryId] uniqueidentifier NOT NULL,
    [Code] nvarchar(20) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [ModifiedOn] datetimeoffset NULL,
    [DeletedBy] uniqueidentifier NULL,
    [DeletedOn] datetimeoffset NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_States] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_States_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CountryId', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[States]'))
    SET IDENTITY_INSERT [States] ON;
INSERT INTO [States] ([Id], [Code], [CountryId], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [IsActive], [IsDeleted], [ModifiedBy], [ModifiedOn], [Name])
VALUES ('42000000-0000-0000-0000-000000000001', N'MH', '41000000-0000-0000-0000-000000000001', NULL, '0001-01-01T00:00:00.0000000+00:00', NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, NULL, N'Maharashtra');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CountryId', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'IsActive', N'IsDeleted', N'ModifiedBy', N'ModifiedOn', N'Name') AND [object_id] = OBJECT_ID(N'[States]'))
    SET IDENTITY_INSERT [States] OFF;

CREATE INDEX [IX_States_CountryId] ON [States] ([CountryId]);

CREATE UNIQUE INDEX [IX_States_CountryId_Code] ON [States] ([CountryId], [Code]);

CREATE UNIQUE INDEX [IX_States_CountryId_Name] ON [States] ([CountryId], [Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826153851_AddState', N'9.0.11');

COMMIT;
GO

