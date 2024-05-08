﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Syrophage.Migrations
{
    /// <inheritdoc />
    public partial class AddQuatation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quatations_Data",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreparedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreparedFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboutUs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Methodology = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expectation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Term1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Term2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Term3 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quatations_Data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Quatations_Services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quatations_Services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OneTimeCharges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HalfYearlyCharges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnualCharges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotationFormDataid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceData_Quatations_Data_QuotationFormDataid",
                        column: x => x.QuotationFormDataid,
                        principalTable: "Quatations_Data",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceData_QuotationFormDataid",
                table: "ServiceData",
                column: "QuotationFormDataid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quatations_Services");

            migrationBuilder.DropTable(
                name: "ServiceData");

            migrationBuilder.DropTable(
                name: "Quatations_Data");
        }
    }
}
