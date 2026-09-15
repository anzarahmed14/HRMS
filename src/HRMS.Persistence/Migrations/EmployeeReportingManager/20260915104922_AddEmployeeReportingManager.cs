using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Persistence.Migrations.EmployeeReportingManager
{
    /// <inheritdoc />
    public partial class AddEmployeeReportingManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReportingManagerId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReportingManagerId",
                table: "Employees",
                column: "ReportingManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_ReportingManagerId",
                table: "Employees",
                column: "ReportingManagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_ReportingManagerId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ReportingManagerId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ReportingManagerId",
                table: "Employees");
        }
    }
}
