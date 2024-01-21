using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHeaven.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_GamingLists_Games_GameID",
                table: "Game_GamingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_GamingLists_GamingLists_GamingListID",
                table: "Game_GamingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_GamingLists_Players_OwnerOfGamingListsID",
                table: "Game_GamingLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game_GamingLists",
                table: "Game_GamingLists");

            migrationBuilder.DropIndex(
                name: "IX_Game_GamingLists_GameID",
                table: "Game_GamingLists");

            migrationBuilder.DropIndex(
                name: "IX_Game_GamingLists_OwnerOfGamingListsID",
                table: "Game_GamingLists");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Game_GamingLists");

            migrationBuilder.DropColumn(
                name: "OwnerOfGamingListsID",
                table: "Game_GamingLists");

            migrationBuilder.AddColumn<int>(
                name: "CreatorOfGamingListID",
                table: "GamingLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GamingListID",
                table: "Game_GamingLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GameID",
                table: "Game_GamingLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game_GamingLists",
                table: "Game_GamingLists",
                columns: new[] { "GameID", "GamingListID" });

            migrationBuilder.CreateIndex(
                name: "IX_GamingLists_CreatorOfGamingListID",
                table: "GamingLists",
                column: "CreatorOfGamingListID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_GamingLists_Games_GameID",
                table: "Game_GamingLists",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_GamingLists_GamingLists_GamingListID",
                table: "Game_GamingLists",
                column: "GamingListID",
                principalTable: "GamingLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamingLists_Players_CreatorOfGamingListID",
                table: "GamingLists",
                column: "CreatorOfGamingListID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_GamingLists_Games_GameID",
                table: "Game_GamingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_GamingLists_GamingLists_GamingListID",
                table: "Game_GamingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_GamingLists_Players_CreatorOfGamingListID",
                table: "GamingLists");

            migrationBuilder.DropIndex(
                name: "IX_GamingLists_CreatorOfGamingListID",
                table: "GamingLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game_GamingLists",
                table: "Game_GamingLists");

            migrationBuilder.DropColumn(
                name: "CreatorOfGamingListID",
                table: "GamingLists");

            migrationBuilder.AlterColumn<int>(
                name: "GamingListID",
                table: "Game_GamingLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GameID",
                table: "Game_GamingLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Game_GamingLists",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "OwnerOfGamingListsID",
                table: "Game_GamingLists",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game_GamingLists",
                table: "Game_GamingLists",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_GamingLists_GameID",
                table: "Game_GamingLists",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_GamingLists_OwnerOfGamingListsID",
                table: "Game_GamingLists",
                column: "OwnerOfGamingListsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_GamingLists_Games_GameID",
                table: "Game_GamingLists",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_GamingLists_GamingLists_GamingListID",
                table: "Game_GamingLists",
                column: "GamingListID",
                principalTable: "GamingLists",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_GamingLists_Players_OwnerOfGamingListsID",
                table: "Game_GamingLists",
                column: "OwnerOfGamingListsID",
                principalTable: "Players",
                principalColumn: "ID");
        }
    }
}
