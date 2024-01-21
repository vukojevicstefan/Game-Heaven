using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHeaven.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
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

            migrationBuilder.DropTable(
                name: "PlayerGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game_GamingLists",
                table: "Game_GamingLists");

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

            migrationBuilder.CreateTable(
                name: "PlayerGames",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "int", nullable: false),
                    GameID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGames", x => new { x.PlayerID, x.GameID });
                    table.ForeignKey(
                        name: "FK_PlayerGames_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGames_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGames_GameID",
                table: "PlayerGames",
                column: "GameID");

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
        }
    }
}
