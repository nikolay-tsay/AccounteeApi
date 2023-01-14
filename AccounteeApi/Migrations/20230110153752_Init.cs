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
                .Annotation("Npgsql:Enum:category_targets", "product,income,outcome,service")
                .Annotation("Npgsql:Enum:measurement_units", "piece,milliliter,litre,kilogram,milligram,gram")
                .Annotation("Npgsql:Enum:user_languages", "english,russian");

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
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Target = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Companies_IdCompany",
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
                    CanReadProducts = table.Column<bool>(type: "boolean", nullable: false),
                    CanCreateProducts = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditProducts = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteProducts = table.Column<bool>(type: "boolean", nullable: false),
                    CanReadServices = table.Column<bool>(type: "boolean", nullable: false),
                    CanCreateServices = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditServices = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteServices = table.Column<bool>(type: "boolean", nullable: false),
                    CanReadCategories = table.Column<bool>(type: "boolean", nullable: false),
                    CanCreateCategories = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditCategories = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteCategories = table.Column<bool>(type: "boolean", nullable: false),
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
                name: "Outcomes",
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
                    table.PrimaryKey("PK_Outcomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outcomes_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Outcomes_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCategory = table.Column<int>(type: "integer", nullable: true),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AmountUnit = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    IdCategory = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRole = table.Column<int>(type: "integer", nullable: false),
                    IdCompany = table.Column<int>(type: "integer", nullable: true),
                    UserLanguage = table.Column<int>(type: "integer", nullable: false),
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
                name: "OutcomeProducts",
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
                    table.PrimaryKey("PK_OutcomeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutcomeProducts_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutcomeProducts_Outcomes_IdOutcome",
                        column: x => x.IdOutcome,
                        principalTable: "Outcomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutcomeProducts_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
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
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incomes_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incomes_Services_IdService",
                        column: x => x.IdService,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProducts",
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
                    table.PrimaryKey("PK_ServiceProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceProducts_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceProducts_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceProducts_Services_IdService",
                        column: x => x.IdService,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserServices",
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
                    table.PrimaryKey("PK_UserServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserServices_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServices_Services_IdService",
                        column: x => x.IdService,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServices_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomeProducts",
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
                    table.PrimaryKey("PK_IncomeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeProducts_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeProducts_Incomes_IdIncome",
                        column: x => x.IdIncome,
                        principalTable: "Incomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeProducts_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIncomes",
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
                    table.PrimaryKey("PK_UserIncomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIncomes_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserIncomes_Incomes_IdIncome",
                        column: x => x.IdIncome,
                        principalTable: "Incomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserIncomes_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CanCreateCategories", "CanCreateCompany", "CanCreateOutlay", "CanCreateProducts", "CanCreateRoles", "CanCreateServices", "CanDeleteCategories", "CanDeleteCompany", "CanDeleteOutlay", "CanDeleteProducts", "CanDeleteRoles", "CanDeleteServices", "CanDeleteUsers", "CanEditCategories", "CanEditCompany", "CanEditOutlay", "CanEditProducts", "CanEditRoles", "CanEditServices", "CanEditUsers", "CanReadCategories", "CanReadOutlay", "CanReadProducts", "CanReadRoles", "CanReadServices", "CanReadUsers", "CanRegisterUsers", "CanUploadFiles", "Description", "IdCompany", "IsAdmin", "Name" },
                values: new object[] { 1, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, null, null, false, "Visitor" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IdCompany_Name_Target",
                table: "Categories",
                columns: new[] { "IdCompany", "Name", "Target" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomeProducts_IdCompany",
                table: "IncomeProducts",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeProducts_IdIncome",
                table: "IncomeProducts",
                column: "IdIncome");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeProducts_IdProduct",
                table: "IncomeProducts",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IdCategory",
                table: "Incomes",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IdCompany",
                table: "Incomes",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IdService",
                table: "Incomes",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeProducts_IdCompany",
                table: "OutcomeProducts",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeProducts_IdOutcome",
                table: "OutcomeProducts",
                column: "IdOutcome");

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeProducts_IdProduct",
                table: "OutcomeProducts",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Outcomes_IdCategory",
                table: "Outcomes",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Outcomes_IdCompany",
                table: "Outcomes",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IdCategory",
                table: "Products",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IdCompany_Name",
                table: "Products",
                columns: new[] { "IdCompany", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_IdCompany_Name",
                table: "Roles",
                columns: new[] { "IdCompany", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProducts_IdCompany",
                table: "ServiceProducts",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProducts_IdProduct",
                table: "ServiceProducts",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProducts_IdService",
                table: "ServiceProducts",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_Services_IdCategory",
                table: "Services",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Services_IdCompany_Name",
                table: "Services",
                columns: new[] { "IdCompany", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomes_IdCompany",
                table: "UserIncomes",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomes_IdIncome",
                table: "UserIncomes",
                column: "IdIncome");

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomes_IdUser",
                table: "UserIncomes",
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
                name: "IX_UserServices_IdCompany",
                table: "UserServices",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_UserServices_IdService",
                table: "UserServices",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_UserServices_IdUser",
                table: "UserServices",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeProducts");

            migrationBuilder.DropTable(
                name: "OutcomeProducts");

            migrationBuilder.DropTable(
                name: "ServiceProducts");

            migrationBuilder.DropTable(
                name: "UserIncomes");

            migrationBuilder.DropTable(
                name: "UserServices");

            migrationBuilder.DropTable(
                name: "Outcomes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
