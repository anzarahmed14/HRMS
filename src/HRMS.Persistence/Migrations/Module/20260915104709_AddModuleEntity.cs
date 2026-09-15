using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRMS.Persistence.Migrations.Module
{
    /// <inheritdoc />
    public partial class AddModuleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create the Modules table first so real module rows can
            // exist before Permissions.ModuleId is ever populated.
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            // 2. Seed the real HRMS modules (matches ModuleConfiguration.HasData).
            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Description", "IsActive", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), "IDENTITY", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, null, null, "Identity" },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "EMPLOYEE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, null, null, "Employee" },
                    { new Guid("50000000-0000-0000-0000-000000000003"), "FOUNDATION", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, null, null, "Foundation" },
                    { new Guid("50000000-0000-0000-0000-000000000004"), "COMPANIES", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, null, null, "Companies" },
                    { new Guid("50000000-0000-0000-0000-000000000005"), "DEPARTMENT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, null, null, "Department" },
                    { new Guid("50000000-0000-0000-0000-000000000006"), "ATTENDANCE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, null, null, "Attendance" },
                    { new Guid("50000000-0000-0000-0000-000000000007"), "LEAVE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, null, null, "Leave" }
                });

            // 3. Add Permissions.ModuleId as nullable first — the existing
            // Permissions rows have no valid module yet, so it cannot be
            // made NOT NULL (and must not default to Guid.Empty) until
            // every remaining row has been assigned a real module below.
            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: true);

            // 4. Remove confirmed test/debris data before enforcing the FK.
            // "Verify.TempPermission" has no code reference and no
            // RolePermission assignment, so this cannot orphan any role.
            migrationBuilder.Sql(
                "DELETE FROM [Permissions] WHERE [Name] = N'Verify.TempPermission';");

            // 5. Backfill the real, existing permissions to their correct
            // module, by name.
            migrationBuilder.Sql(
                "UPDATE [Permissions] SET [ModuleId] = '50000000-0000-0000-0000-000000000002' " +
                "WHERE [Name] IN (N'Employee.Create', N'Employee.Delete', N'Employee.Update', N'Employee.View');");

            migrationBuilder.Sql(
                "UPDATE [Permissions] SET [ModuleId] = '50000000-0000-0000-0000-000000000001' " +
                "WHERE [Name] = N'User.ResetPassword';");

            // 6. Every remaining Permissions row now has a valid ModuleId;
            // enforce the NOT NULL constraint the domain model requires.
            migrationBuilder.AlterColumn<Guid>(
                name: "ModuleId",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ModuleId",
                table: "Permissions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Code",
                table: "Modules",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Modules_ModuleId",
                table: "Permissions",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Modules_ModuleId",
                table: "Permissions");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_ModuleId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Permissions");
        }
    }
}
