using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchEntityMatchID",
                table: "PlayerStatistics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchEntityMatchID",
                table: "MatchEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStatistics_MatchEntityMatchID",
                table: "PlayerStatistics",
                column: "MatchEntityMatchID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_MatchEntityMatchID",
                table: "MatchEvents",
                column: "MatchEntityMatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvents_Matches_MatchEntityMatchID",
                table: "MatchEvents",
                column: "MatchEntityMatchID",
                principalTable: "Matches",
                principalColumn: "MatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistics_Matches_MatchEntityMatchID",
                table: "PlayerStatistics",
                column: "MatchEntityMatchID",
                principalTable: "Matches",
                principalColumn: "MatchID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvents_Matches_MatchEntityMatchID",
                table: "MatchEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistics_Matches_MatchEntityMatchID",
                table: "PlayerStatistics");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStatistics_MatchEntityMatchID",
                table: "PlayerStatistics");

            migrationBuilder.DropIndex(
                name: "IX_MatchEvents_MatchEntityMatchID",
                table: "MatchEvents");

            migrationBuilder.DropColumn(
                name: "MatchEntityMatchID",
                table: "PlayerStatistics");

            migrationBuilder.DropColumn(
                name: "MatchEntityMatchID",
                table: "MatchEvents");
        }
    }
}
