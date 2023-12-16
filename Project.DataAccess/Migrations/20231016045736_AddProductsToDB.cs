using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "categories",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Charles Duhigg", "This book explores the science behind habit formation and provides valuable insights on how to change and build habits for personal and professional success. ", " 978-0812981605", 100.0, 95.0, 85.0, 90.0, "The Power of Habit: Why We Do What We Do in Life and Business" },
                    { 2, "James Clear", "James Clear's book focuses on the power of small changes and how they can lead to significant personal development. It offers practical strategies for building positive habits and breaking destructive ones. ", "978-0735211292", 99.0, 95.0, 85.0, 90.0, "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones" },
                    { 3, "Dale Carnegie", " A timeless classic, this book offers valuable advice on building meaningful relationships, improving communication, and becoming more influential in both personal and professional settings.", "978-0671027032", 90.0, 88.0, 83.0, 85.0, "How to Win Friends and Influence People" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
