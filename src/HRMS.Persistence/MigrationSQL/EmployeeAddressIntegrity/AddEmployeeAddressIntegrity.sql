BEGIN TRANSACTION;
DROP INDEX [IX_EmployeeAddresses_EmployeeId] ON [EmployeeAddresses];

CREATE UNIQUE INDEX [IX_EmployeeAddresses_EmployeeId] ON [EmployeeAddresses] ([EmployeeId]) WHERE [IsPrimary] = 1 AND [IsDeleted] = 0;

CREATE UNIQUE INDEX [IX_EmployeeAddresses_EmployeeId_AddressTypeId] ON [EmployeeAddresses] ([EmployeeId], [AddressTypeId]) WHERE [IsDeleted] = 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826160039_AddEmployeeAddressIntegrity', N'9.0.11');

COMMIT;
GO

