using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Syrophage.Migrations
{
    /// <inheritdoc />
    public partial class AddQuatationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quatations_fix",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cemail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CAboutUs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CMethodology = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quatations_fix", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Quatations_fix",
                columns: new[] { "id", "CAboutUs", "CMethodology", "CPhoneNo", "Cemail", "Cname" },
                values: new object[] { 1, "At Syrophage, we're not just another startup; we're a passionate team of individuals driven \r\nby innovation and collaboration. Our name, derived from the initials of our founding \r\nmembers, symbolizes the unity and collective spirit that defines our organization. By \r\ncombining the words 'synergy' and 'age,' we embrace the power of collaboration and the \r\npromise of a new era. Additionally, we believe in giving back; for every service, we contribute \r\na portion to society, ensuring our impact extends beyond business.", "Our mission is to empower businesses to thrive by offering a comprehensive suite of \r\nservices, including but not limited to admin supplies, corporate advisories, employee \r\nengagement activities, customized gifts, greetings mail support, corporate events \r\nmanagement, and much more. We aim to be the go-to partner for corporations seeking to \r\nunlock their full potential and foster a harmonious work environment.", "74475 08124", "Syrophage@gmail.com", "SYROPHAGE IN PRIVATE LIMITED" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quatations_fix");
        }
    }
}
