using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveSoftDeleteFilteredIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveYears_CompanyId_Code",
                table: "LeaveYears");

            migrationBuilder.DropIndex(
                name: "IX_LeaveYears_CompanyId_Name",
                table: "LeaveYears");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_CompanyId_Code",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_CompanyId_Name",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeavePolicyRules_LeavePolicyId_LeaveTypeId",
                table: "LeavePolicyRules");

            migrationBuilder.DropIndex(
                name: "IX_LeavePolicies_CompanyId_Code",
                table: "LeavePolicies");

            migrationBuilder.DropIndex(
                name: "IX_LeavePolicies_CompanyId_Name",
                table: "LeavePolicies");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLeaveEntitlements_EmployeeId_LeaveYearId_LeaveTypeId",
                table: "EmployeeLeaveEntitlements");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveYears_CompanyId_Code",
                table: "LeaveYears",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveYears_CompanyId_Name",
                table: "LeaveYears",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_CompanyId_Code",
                table: "LeaveTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_CompanyId_Name",
                table: "LeaveTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LeavePolicyRules_LeavePolicyId_LeaveTypeId",
                table: "LeavePolicyRules",
                columns: new[] { "LeavePolicyId", "LeaveTypeId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LeavePolicies_CompanyId_Code",
                table: "LeavePolicies",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LeavePolicies_CompanyId_Name",
                table: "LeavePolicies",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeaveEntitlements_EmployeeId_LeaveYearId_LeaveTypeId",
                table: "EmployeeLeaveEntitlements",
                columns: new[] { "EmployeeId", "LeaveYearId", "LeaveTypeId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveYears_CompanyId_Code",
                table: "LeaveYears");

            migrationBuilder.DropIndex(
                name: "IX_LeaveYears_CompanyId_Name",
                table: "LeaveYears");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_CompanyId_Code",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_CompanyId_Name",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeavePolicyRules_LeavePolicyId_LeaveTypeId",
                table: "LeavePolicyRules");

            migrationBuilder.DropIndex(
                name: "IX_LeavePolicies_CompanyId_Code",
                table: "LeavePolicies");

            migrationBuilder.DropIndex(
                name: "IX_LeavePolicies_CompanyId_Name",
                table: "LeavePolicies");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLeaveEntitlements_EmployeeId_LeaveYearId_LeaveTypeId",
                table: "EmployeeLeaveEntitlements");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveYears_CompanyId_Code",
                table: "LeaveYears",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveYears_CompanyId_Name",
                table: "LeaveYears",
                columns: new[] { "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_CompanyId_Code",
                table: "LeaveTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_CompanyId_Name",
                table: "LeaveTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeavePolicyRules_LeavePolicyId_LeaveTypeId",
                table: "LeavePolicyRules",
                columns: new[] { "LeavePolicyId", "LeaveTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeavePolicies_CompanyId_Code",
                table: "LeavePolicies",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeavePolicies_CompanyId_Name",
                table: "LeavePolicies",
                columns: new[] { "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeaveEntitlements_EmployeeId_LeaveYearId_LeaveTypeId",
                table: "EmployeeLeaveEntitlements",
                columns: new[] { "EmployeeId", "LeaveYearId", "LeaveTypeId" },
                unique: true);
        }
    }
}
