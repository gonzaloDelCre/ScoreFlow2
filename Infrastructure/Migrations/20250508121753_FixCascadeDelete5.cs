using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvents_Matches_MatchEntityMatchID",
                table: "MatchEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistics_Matches_MatchEntityMatchID",
                table: "PlayerStatistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Standings_Leagues_LeagueEntityLeagueID",
                table: "Standings");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Players_CoachPlayerID",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "MatchReferees");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TeamLeagues");

            migrationBuilder.DropTable(
                name: "Referees");

            migrationBuilder.DropIndex(
                name: "IX_Standings_LeagueEntityLeagueID",
                table: "Standings");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStatistics_MatchEntityMatchID",
                table: "PlayerStatistics");

            migrationBuilder.DropIndex(
                name: "IX_MatchEvents_MatchEntityMatchID",
                table: "MatchEvents");

            migrationBuilder.DropColumn(
                name: "LeagueEntityLeagueID",
                table: "Standings");

            migrationBuilder.DropColumn(
                name: "MatchEntityMatchID",
                table: "PlayerStatistics");

            migrationBuilder.DropColumn(
                name: "MatchEntityMatchID",
                table: "MatchEvents");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TeamPlayers",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "StandingID",
                table: "Standings",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "StatID",
                table: "PlayerStatistics",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "EventID",
                table: "MatchEvents",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MatchID",
                table: "Matches",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "LeagueID",
                table: "Leagues",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "Players",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Matches",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Leagues",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Players_CoachPlayerID",
                table: "Teams",
                column: "CoachPlayerID",
                principalTable: "Players",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Players_CoachPlayerID",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "TeamPlayers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Standings",
                newName: "StandingID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PlayerStatistics",
                newName: "StatID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MatchEvents",
                newName: "EventID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Matches",
                newName: "MatchID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Leagues",
                newName: "LeagueID");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "LeagueEntityLeagueID",
                table: "Standings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchEntityMatchID",
                table: "PlayerStatistics",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "MatchEntityMatchID",
                table: "MatchEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Referees",
                columns: table => new
                {
                    RefereeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referees", x => x.RefereeID);
                });

            migrationBuilder.CreateTable(
                name: "TeamLeagues",
                columns: table => new
                {
                    TeamID = table.Column<int>(type: "int", nullable: false),
                    LeagueID = table.Column<int>(type: "int", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeagueEntityLeagueID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLeagues", x => new { x.TeamID, x.LeagueID });
                    table.ForeignKey(
                        name: "FK_TeamLeagues_Leagues_LeagueEntityLeagueID",
                        column: x => x.LeagueEntityLeagueID,
                        principalTable: "Leagues",
                        principalColumn: "LeagueID");
                    table.ForeignKey(
                        name: "FK_TeamLeagues_Leagues_LeagueID",
                        column: x => x.LeagueID,
                        principalTable: "Leagues",
                        principalColumn: "LeagueID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamLeagues_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchReferees",
                columns: table => new
                {
                    MatchID = table.Column<int>(type: "int", nullable: false),
                    RefereeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchReferees", x => new { x.MatchID, x.RefereeID });
                    table.ForeignKey(
                        name: "FK_MatchReferees_Matches_MatchID",
                        column: x => x.MatchID,
                        principalTable: "Matches",
                        principalColumn: "MatchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchReferees_Referees_RefereeID",
                        column: x => x.RefereeID,
                        principalTable: "Referees",
                        principalColumn: "RefereeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standings_LeagueEntityLeagueID",
                table: "Standings",
                column: "LeagueEntityLeagueID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStatistics_MatchEntityMatchID",
                table: "PlayerStatistics",
                column: "MatchEntityMatchID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_MatchEntityMatchID",
                table: "MatchEvents",
                column: "MatchEntityMatchID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchReferees_RefereeID",
                table: "MatchReferees",
                column: "RefereeID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserID",
                table: "Notifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagues_LeagueEntityLeagueID",
                table: "TeamLeagues",
                column: "LeagueEntityLeagueID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagues_LeagueID",
                table: "TeamLeagues",
                column: "LeagueID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_Leagues_LeagueEntityLeagueID",
                table: "Standings",
                column: "LeagueEntityLeagueID",
                principalTable: "Leagues",
                principalColumn: "LeagueID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Players_CoachPlayerID",
                table: "Teams",
                column: "CoachPlayerID",
                principalTable: "Players",
                principalColumn: "PlayerID");
        }
    }
}
