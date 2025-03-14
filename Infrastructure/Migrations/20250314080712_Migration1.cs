using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueEntityLeagueID",
                table: "TeamLeagues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeagueEntityLeagueID",
                table: "Standings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagues_LeagueEntityLeagueID",
                table: "TeamLeagues",
                column: "LeagueEntityLeagueID");

            migrationBuilder.CreateIndex(
                name: "IX_Standings_LeagueEntityLeagueID",
                table: "Standings",
                column: "LeagueEntityLeagueID");

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_Leagues_LeagueEntityLeagueID",
                table: "Standings",
                column: "LeagueEntityLeagueID",
                principalTable: "Leagues",
                principalColumn: "LeagueID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamLeagues_Leagues_LeagueEntityLeagueID",
                table: "TeamLeagues",
                column: "LeagueEntityLeagueID",
                principalTable: "Leagues",
                principalColumn: "LeagueID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standings_Leagues_LeagueEntityLeagueID",
                table: "Standings");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamLeagues_Leagues_LeagueEntityLeagueID",
                table: "TeamLeagues");

            migrationBuilder.DropIndex(
                name: "IX_TeamLeagues_LeagueEntityLeagueID",
                table: "TeamLeagues");

            migrationBuilder.DropIndex(
                name: "IX_Standings_LeagueEntityLeagueID",
                table: "Standings");

            migrationBuilder.DropColumn(
                name: "LeagueEntityLeagueID",
                table: "TeamLeagues");

            migrationBuilder.DropColumn(
                name: "LeagueEntityLeagueID",
                table: "Standings");
        }
    }
}
