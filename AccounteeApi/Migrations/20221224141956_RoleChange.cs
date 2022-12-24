using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccounteeApi.Migrations
{
    /// <inheritdoc />
    public partial class RoleChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_IdCompany",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteCompany",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditCompany",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CanDeleteCompany", "CanEditCompany" },
                values: new object[] { false, false });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_IdCompany",
                table: "Users",
                column: "IdCompany",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_IdCompany",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CanDeleteCompany",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanEditCompany",
                table: "Roles");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_IdCompany",
                table: "Users",
                column: "IdCompany",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
