using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHeaven.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamingLists_Players_CreatorOfGamingListID",
                table: "GamingLists");

            migrationBuilder.DropIndex(
                name: "IX_GamingLists_CreatorOfGamingListID",
                table: "GamingLists");

            migrationBuilder.DropColumn(
                name: "CreatorOfGamingListID",
                table: "GamingLists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorOfGamingListID",
                table: "GamingLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GamingLists_CreatorOfGamingListID",
                table: "GamingLists",
                column: "CreatorOfGamingListID");

            migrationBuilder.AddForeignKey(
                name: "FK_GamingLists_Players_CreatorOfGamingListID",
                table: "GamingLists",
                column: "CreatorOfGamingListID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
