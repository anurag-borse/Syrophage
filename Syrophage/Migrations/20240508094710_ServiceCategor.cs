using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Syrophage.Migrations
{
    /// <inheritdoc />
    public partial class ServiceCategor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "ServiceCategories");

            migrationBuilder.RenameColumn(
                name: "CategoryPictureUrl",
                table: "ServiceCategories",
                newName: "ServiceCategoryName");

            migrationBuilder.RenameColumn(
                name: "CategoryDescription",
                table: "ServiceCategories",
                newName: "ServiceCategoryPictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "ServiceCategoryDescription",
                table: "ServiceCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceCategoryDescription",
                table: "ServiceCategories");

            migrationBuilder.RenameColumn(
                name: "ServiceCategoryPictureUrl",
                table: "ServiceCategories",
                newName: "CategoryDescription");

            migrationBuilder.RenameColumn(
                name: "ServiceCategoryName",
                table: "ServiceCategories",
                newName: "CategoryPictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "ServiceCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
