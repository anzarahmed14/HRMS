using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Persistence.Migrations.EmployeeAddressIntegrity
{
    /// <inheritdoc />
    public partial class AddEmployeeAddressIntegrity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                table: "EmployeeAddresses");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                table: "EmployeeAddresses",
                column: "EmployeeId",
                unique: true,
                filter: "[IsPrimary] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_EmployeeId_AddressTypeId",
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "AddressTypeId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                table: "EmployeeAddresses");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAddresses_EmployeeId_AddressTypeId",
                table: "EmployeeAddresses");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                table: "EmployeeAddresses",
                column: "EmployeeId");
        }
    }
}
