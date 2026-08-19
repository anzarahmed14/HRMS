using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveYearStatusAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "LeaveYearStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "LeaveYearStatuses",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "LeaveYearStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "LeaveYearStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LeaveYearStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "LeaveYearStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "LeaveYearStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaveYearStatuses",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn" },
                values: new object[] { null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "LeaveYearStatuses",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn" },
                values: new object[] { null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "LeaveYearStatuses",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn" },
                values: new object[] { null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LeaveYearStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "LeaveYearStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "LeaveYearStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "LeaveYearStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LeaveYearStatuses");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "LeaveYearStatuses");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "LeaveYearStatuses");
        }
    }
}
