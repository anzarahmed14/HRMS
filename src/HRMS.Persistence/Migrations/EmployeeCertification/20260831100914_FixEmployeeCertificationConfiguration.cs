using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Persistence.Migrations.EmployeeCertification
{
    /// <inheritdoc />
    public partial class FixEmployeeCertificationConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CredentialUrl",
                table: "EmployeeCertifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CertificationNumber",
                table: "EmployeeCertifications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertifications_CertificationId",
                table: "EmployeeCertifications",
                column: "CertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertifications_EmployeeId",
                table: "EmployeeCertifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertifications_EmployeeId_CertificationId",
                table: "EmployeeCertifications",
                columns: new[] { "EmployeeId", "CertificationId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCertifications_Certifications_CertificationId",
                table: "EmployeeCertifications",
                column: "CertificationId",
                principalTable: "Certifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCertifications_Employees_EmployeeId",
                table: "EmployeeCertifications",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCertifications_Certifications_CertificationId",
                table: "EmployeeCertifications");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCertifications_Employees_EmployeeId",
                table: "EmployeeCertifications");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCertifications_CertificationId",
                table: "EmployeeCertifications");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCertifications_EmployeeId",
                table: "EmployeeCertifications");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCertifications_EmployeeId_CertificationId",
                table: "EmployeeCertifications");

            migrationBuilder.AlterColumn<string>(
                name: "CredentialUrl",
                table: "EmployeeCertifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CertificationNumber",
                table: "EmployeeCertifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
