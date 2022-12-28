using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccounteeApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:measurement_units", "piece,milliliter,litre,kilogram,milligram,gram");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Budget = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCategoryEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCategoryEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeCategoryEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutcomeCategoryEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutcomeCategoryEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutcomeCategoryEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    CanCreateCompany = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditCompany = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteCompany = table.Column<bool>(type: "boolean", nullable: false),
                    CanReadUsers = table.Column<bool>(type: "boolean", nullable: false),
                    CanRegisterUsers = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditUsers = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteUsers = table.Column<bool>(type: "boolean", nullable: false),
                    CanReadRoles = table.Column<bool>(type: "boolean", nullable: false),
                    CanCreateRoles = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditRoles = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteRoles = table.Column<bool>(type: "boolean", nullable: false),
                    CanReadOutlay = table.Column<bool>(type: "boolean", nullable: false),
                    CanCreateOutlay = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditOutlay = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteOutlay = table.Column<bool>(type: "boolean", nullable: false),
                    CanUploadFiles = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutcomeEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdCategory = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastEdited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutcomeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutcomeEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutcomeEntity_OutcomeCategoryEntity_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "OutcomeCategoryEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCategory = table.Column<int>(type: "integer", nullable: false),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AmountUnit = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductEntity_ProductCategoryEntity_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "ProductCategoryEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRole = table.Column<int>(type: "integer", nullable: false),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Login = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordSalt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IncomePercent = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdCategory = table.Column<int>(type: "integer", nullable: false),
                    IdService = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastEdited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeEntity_IncomeCategoryEntity_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "IncomeCategoryEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeEntity_ServiceEntity_IdService",
                        column: x => x.IdService,
                        principalTable: "ServiceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutcomeProductEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdProduct = table.Column<int>(type: "integer", nullable: false),
                    IdOutcome = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutcomeProductEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutcomeProductEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutcomeProductEntity_OutcomeEntity_IdOutcome",
                        column: x => x.IdOutcome,
                        principalTable: "OutcomeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutcomeProductEntity_ProductEntity_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProductEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdProduct = table.Column<int>(type: "integer", nullable: false),
                    IdService = table.Column<int>(type: "integer", nullable: false),
                    ProductUsedAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    PricePerService = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProductEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceProductEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceProductEntity_ProductEntity_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceProductEntity_ServiceEntity_IdService",
                        column: x => x.IdService,
                        principalTable: "ServiceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserServiceEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    IdService = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServiceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserServiceEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServiceEntity_ServiceEntity_IdService",
                        column: x => x.IdService,
                        principalTable: "ServiceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServiceEntity_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomeProductEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdProduct = table.Column<int>(type: "integer", nullable: false),
                    IdIncome = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeProductEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeProductEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeProductEntity_IncomeEntity_IdIncome",
                        column: x => x.IdIncome,
                        principalTable: "IncomeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeProductEntity_ProductEntity_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIncomeEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    IdIncome = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIncomeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIncomeEntity_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserIncomeEntity_IncomeEntity_IdIncome",
                        column: x => x.IdIncome,
                        principalTable: "IncomeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserIncomeEntity_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CanCreateCompany", "CanCreateOutlay", "CanCreateRoles", "CanDeleteCompany", "CanDeleteOutlay", "CanDeleteRoles", "CanDeleteUsers", "CanEditCompany", "CanEditOutlay", "CanEditRoles", "CanEditUsers", "CanReadOutlay", "CanReadRoles", "CanReadUsers", "CanRegisterUsers", "CanUploadFiles", "Description", "IdCompany", "IsAdmin", "Name" },
                values: new object[] { 1, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, null, null, false, "Visitor" });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCategoryEntity_IdCompany",
                table: "IncomeCategoryEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEntity_IdCategory",
                table: "IncomeEntity",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEntity_IdCompany",
                table: "IncomeEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEntity_IdService",
                table: "IncomeEntity",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeProductEntity_IdCompany",
                table: "IncomeProductEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeProductEntity_IdIncome",
                table: "IncomeProductEntity",
                column: "IdIncome");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeProductEntity_IdProduct",
                table: "IncomeProductEntity",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeCategoryEntity_IdCompany",
                table: "OutcomeCategoryEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeEntity_IdCategory",
                table: "OutcomeEntity",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeEntity_IdCompany",
                table: "OutcomeEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeProductEntity_IdCompany",
                table: "OutcomeProductEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeProductEntity_IdOutcome",
                table: "OutcomeProductEntity",
                column: "IdOutcome");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeProductEntity_IdProduct",
                table: "OutcomeProductEntity",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryEntity_IdCompany",
                table: "ProductCategoryEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntity_IdCategory",
                table: "ProductEntity",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntity_IdCompany_Name",
                table: "ProductEntity",
                columns: new[] { "IdCompany", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_IdCompany_Name",
                table: "Roles",
                columns: new[] { "IdCompany", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEntity_IdCompany_Name",
                table: "ServiceEntity",
                columns: new[] { "IdCompany", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProductEntity_IdCompany",
                table: "ServiceProductEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProductEntity_IdProduct",
                table: "ServiceProductEntity",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProductEntity_IdService",
                table: "ServiceProductEntity",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomeEntity_IdCompany",
                table: "UserIncomeEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomeEntity_IdIncome",
                table: "UserIncomeEntity",
                column: "IdIncome");

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomeEntity_IdUser",
                table: "UserIncomeEntity",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCompany_Login",
                table: "Users",
                columns: new[] { "IdCompany", "Login" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdRole",
                table: "Users",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_UserServiceEntity_IdCompany",
                table: "UserServiceEntity",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_UserServiceEntity_IdService",
                table: "UserServiceEntity",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_UserServiceEntity_IdUser",
                table: "UserServiceEntity",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeProductEntity");

            migrationBuilder.DropTable(
                name: "OutcomeProductEntity");

            migrationBuilder.DropTable(
                name: "ServiceProductEntity");

            migrationBuilder.DropTable(
                name: "UserIncomeEntity");

            migrationBuilder.DropTable(
                name: "UserServiceEntity");

            migrationBuilder.DropTable(
                name: "OutcomeEntity");

            migrationBuilder.DropTable(
                name: "ProductEntity");

            migrationBuilder.DropTable(
                name: "IncomeEntity");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "OutcomeCategoryEntity");

            migrationBuilder.DropTable(
                name: "ProductCategoryEntity");

            migrationBuilder.DropTable(
                name: "IncomeCategoryEntity");

            migrationBuilder.DropTable(
                name: "ServiceEntity");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
