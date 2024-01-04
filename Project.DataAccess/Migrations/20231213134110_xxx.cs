using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class xxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "Company",
                 columns: table => new
                 {
                     ID = table.Column<int>(type: "int", nullable: false)
                         .Annotation("SqlServer:Identity", "1, 1"),
                     City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                     PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_Company", x => x.ID);
                 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                 name: "Company");

        }
    }
}
