using Microsoft.EntityFrameworkCore.Migrations;

namespace Asset.Migrations
{
    public partial class jgu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LibraryTitle",
                table: "CheckoutHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LibraryTitle",
                table: "CheckoutHistory",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
