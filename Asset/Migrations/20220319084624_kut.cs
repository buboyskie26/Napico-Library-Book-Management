using Microsoft.EntityFrameworkCore.Migrations;

namespace Asset.Migrations
{
    public partial class kut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryTopicId",
                table: "CheckoutHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutHistory_LibraryTopicId",
                table: "CheckoutHistory",
                column: "LibraryTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutHistory_LibraryTopics_LibraryTopicId",
                table: "CheckoutHistory",
                column: "LibraryTopicId",
                principalTable: "LibraryTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutHistory_LibraryTopics_LibraryTopicId",
                table: "CheckoutHistory");

            migrationBuilder.DropIndex(
                name: "IX_CheckoutHistory_LibraryTopicId",
                table: "CheckoutHistory");

            migrationBuilder.DropColumn(
                name: "LibraryTopicId",
                table: "CheckoutHistory");
        }
    }
}
