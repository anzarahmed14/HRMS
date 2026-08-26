using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRMS.Persistence.Migrations.IdentifierType
{
    /// <inheritdoc />
    public partial class AddIdentifierType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentifierTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_IdentifierTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentifierTypes",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Description", "IsActive", "IsDeleted", "IsSensitive", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { new Guid("44000000-0000-0000-0000-000000000001"), "PAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Indian PAN identifier.", true, false, true, null, null, "Permanent Account Number" },
                    { new Guid("44000000-0000-0000-0000-000000000002"), "UAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Employee UAN.", true, false, true, null, null, "Universal Account Number" },
                    { new Guid("44000000-0000-0000-0000-000000000003"), "PF", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Employee provident fund identifier.", true, false, true, null, null, "Provident Fund Number" },
                    { new Guid("44000000-0000-0000-0000-000000000004"), "ESIC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Employee ESIC identifier.", true, false, true, null, null, "ESIC Number" },
                    { new Guid("44000000-0000-0000-0000-000000000005"), "AADHAAR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Indian Aadhaar identifier.", true, false, true, null, null, "Aadhaar Number" },
                    { new Guid("44000000-0000-0000-0000-000000000006"), "PASSPORT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Passport identifier.", true, false, true, null, null, "Passport Number" },
                    { new Guid("44000000-0000-0000-0000-000000000007"), "DRIVING_LICENSE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Driving license identifier.", true, false, true, null, null, "Driving License Number" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifierTypes_Code",
                table: "IdentifierTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentifierTypes_Name",
                table: "IdentifierTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifierTypes");
        }
    }
}
