using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Persistence.Migrations.Identity;

public partial class MakeIdentityAssignmentsAuditable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // =========================================================
        // UserRoles
        // =========================================================

        migrationBuilder.DropPrimaryKey(
            name: "PK_UserRoles",
            table: "UserRoles");

        migrationBuilder.AddColumn<Guid>(
            name: "Id",
            table: "UserRoles",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "UserRoles",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "CreatedOn",
            table: "UserRoles",
            type: "datetimeoffset",
            nullable: false,
            defaultValueSql: "SYSUTCDATETIME()");

        migrationBuilder.AddColumn<Guid>(
            name: "DeletedBy",
            table: "UserRoles",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "DeletedOn",
            table: "UserRoles",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "UserRoles",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<Guid>(
            name: "ModifiedBy",
            table: "UserRoles",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "ModifiedOn",
            table: "UserRoles",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.Sql("""
            UPDATE [UserRoles]
            SET [Id] = NEWID()
            WHERE [Id] IS NULL;
            """);

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "UserRoles",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_UserRoles",
            table: "UserRoles",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_UserId",
            table: "UserRoles",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_UserId_RoleId",
            table: "UserRoles",
            columns: new[] { "UserId", "RoleId" },
            unique: true,
            filter: "[IsDeleted] = 0");

        // =========================================================
        // RolePermissions
        // =========================================================

        migrationBuilder.DropPrimaryKey(
            name: "PK_RolePermissions",
            table: "RolePermissions");

        migrationBuilder.AddColumn<Guid>(
            name: "Id",
            table: "RolePermissions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "RolePermissions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "CreatedOn",
            table: "RolePermissions",
            type: "datetimeoffset",
            nullable: false,
            defaultValueSql: "SYSUTCDATETIME()");

        migrationBuilder.AddColumn<Guid>(
            name: "DeletedBy",
            table: "RolePermissions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "DeletedOn",
            table: "RolePermissions",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "RolePermissions",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<Guid>(
            name: "ModifiedBy",
            table: "RolePermissions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "ModifiedOn",
            table: "RolePermissions",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.Sql("""
            UPDATE [RolePermissions]
            SET [Id] = NEWID()
            WHERE [Id] IS NULL;
            """);

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "RolePermissions",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_RolePermissions",
            table: "RolePermissions",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_RolePermissions_RoleId",
            table: "RolePermissions",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_RolePermissions_RoleId_PermissionId",
            table: "RolePermissions",
            columns: new[] { "RoleId", "PermissionId" },
            unique: true,
            filter: "[IsDeleted] = 0");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_UserRoles",
            table: "UserRoles");

        migrationBuilder.DropIndex(
            name: "IX_UserRoles_UserId",
            table: "UserRoles");

        migrationBuilder.DropIndex(
            name: "IX_UserRoles_UserId_RoleId",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "CreatedOn",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "DeletedBy",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "DeletedOn",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "ModifiedBy",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "ModifiedOn",
            table: "UserRoles");

        migrationBuilder.AddPrimaryKey(
            name: "PK_UserRoles",
            table: "UserRoles",
            columns: new[] { "UserId", "RoleId" });

        migrationBuilder.DropPrimaryKey(
            name: "PK_RolePermissions",
            table: "RolePermissions");

        migrationBuilder.DropIndex(
            name: "IX_RolePermissions_RoleId",
            table: "RolePermissions");

        migrationBuilder.DropIndex(
            name: "IX_RolePermissions_RoleId_PermissionId",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "CreatedOn",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "DeletedBy",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "DeletedOn",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "ModifiedBy",
            table: "RolePermissions");

        migrationBuilder.DropColumn(
            name: "ModifiedOn",
            table: "RolePermissions");

        migrationBuilder.AddPrimaryKey(
            name: "PK_RolePermissions",
            table: "RolePermissions",
            columns: new[] { "RoleId", "PermissionId" });
    }
}
