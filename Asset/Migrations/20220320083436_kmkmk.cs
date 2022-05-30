using Microsoft.EntityFrameworkCore.Migrations;

namespace Asset.Migrations
{
    public partial class kmkmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LibraryTitle",
                table: "CheckoutHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LibraryTitle",
                table: "CheckoutHistory");
        }
    }
}
