using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Syrophage.Migrations
{
    /// <inheritdoc />
    public partial class adminimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Contact", "ProfileImageUrl" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Admins");
        }
    }
}
