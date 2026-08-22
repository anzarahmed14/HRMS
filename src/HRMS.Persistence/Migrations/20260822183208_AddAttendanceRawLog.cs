using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceRawLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceRawLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PunchDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PunchType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ExternalRecordId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RawData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
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
                    table.PrimaryKey("PK_AttendanceRawLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRawLogs_AttendanceDeviceId",
                table: "AttendanceRawLogs",
                column: "AttendanceDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRawLogs_EmployeeId_PunchDateTime",
                table: "AttendanceRawLogs",
                columns: new[] { "EmployeeId", "PunchDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRawLogs_ExternalRecordId",
                table: "AttendanceRawLogs",
                column: "ExternalRecordId",
                unique: true,
                filter: "[ExternalRecordId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRawLogs");
        }
    }
}
