using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Jornada",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueID",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_LeagueID",
                table: "Matches",
                column: "LeagueID");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Leagues_LeagueID",
                table: "Matches",
                column: "LeagueID",
                principalTable: "Leagues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Leagues_LeagueID",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_LeagueID",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Jornada",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "LeagueID",
                table: "Matches");
        }
    }
}
