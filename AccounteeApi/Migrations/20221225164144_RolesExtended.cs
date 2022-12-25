using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccounteeApi.Migrations
{
    /// <inheritdoc />
    public partial class RolesExtended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanRead",
                table: "Roles",
                newName: "CanRegisterUsers");

            migrationBuilder.RenameColumn(
                name: "CanEdit",
                table: "Roles",
                newName: "CanReadUsers");

            migrationBuilder.RenameColumn(
                name: "CanDelete",
                table: "Roles",
                newName: "CanReadRoles");

            migrationBuilder.RenameColumn(
                name: "CanCreate",
                table: "Roles",
                newName: "CanReadOutlay");

            migrationBuilder.AddColumn<bool>(
                name: "CanCreateOutlay",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanCreateRoles",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteOutlay",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteRoles",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteUsers",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditOutlay",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditRoles",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditUsers",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CanCreateOutlay", "CanCreateRoles", "CanDeleteOutlay", "CanDeleteRoles", "CanDeleteUsers", "CanEditOutlay", "CanEditRoles", "CanEditUsers" },
                values: new object[] { false, false, false, false, false, false, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanCreateOutlay",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanCreateRoles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanDeleteOutlay",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanDeleteRoles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanDeleteUsers",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanEditOutlay",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanEditRoles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanEditUsers",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "CanRegisterUsers",
                table: "Roles",
                newName: "CanRead");

            migrationBuilder.RenameColumn(
                name: "CanReadUsers",
                table: "Roles",
                newName: "CanEdit");

            migrationBuilder.RenameColumn(
                name: "CanReadRoles",
                table: "Roles",
                newName: "CanDelete");

            migrationBuilder.RenameColumn(
                name: "CanReadOutlay",
                table: "Roles",
                newName: "CanCreate");
        }
    }
}
