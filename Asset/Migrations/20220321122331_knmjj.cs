using Microsoft.EntityFrameworkCore.Migrations;

namespace Asset.Migrations
{
    public partial class knmjj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryTopicId",
                table: "Holds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LibraryTopicId",
                table: "HoldHistories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Holds_LibraryTopicId",
                table: "Holds",
                column: "LibraryTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_HoldHistories_LibraryTopicId",
                table: "HoldHistories",
                column: "LibraryTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoldHistories_LibraryTopics_LibraryTopicId",
                table: "HoldHistories",
                column: "LibraryTopicId",
                principalTable: "LibraryTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Holds_LibraryTopics_LibraryTopicId",
                table: "Holds",
                column: "LibraryTopicId",
                principalTable: "LibraryTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoldHistories_LibraryTopics_LibraryTopicId",
                table: "HoldHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Holds_LibraryTopics_LibraryTopicId",
                table: "Holds");

            migrationBuilder.DropIndex(
                name: "IX_Holds_LibraryTopicId",
                table: "Holds");

            migrationBuilder.DropIndex(
                name: "IX_HoldHistories_LibraryTopicId",
                table: "HoldHistories");

            migrationBuilder.DropColumn(
                name: "LibraryTopicId",
                table: "Holds");

            migrationBuilder.DropColumn(
                name: "LibraryTopicId",
                table: "HoldHistories");
        }
    }
}
