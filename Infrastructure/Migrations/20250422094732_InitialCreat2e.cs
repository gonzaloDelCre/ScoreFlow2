using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat2e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamID",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Users_UserID",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamID",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_UserID",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "TeamID",
                table: "Players",
                newName: "Goals");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Teams",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Teams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Club",
                table: "Teams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoachPlayerID",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stadium",
                table: "Teams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Players",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Players",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeamPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamID = table.Column<int>(type: "int", nullable: false),
                    PlayerID = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleInTeam = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPlayers_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPlayers_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CoachPlayerID",
                table: "Teams",
                column: "CoachPlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_PlayerID",
                table: "TeamPlayers",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_TeamID",
                table: "TeamPlayers",
                column: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Players_CoachPlayerID",
                table: "Teams",
                column: "CoachPlayerID",
                principalTable: "Players",
                principalColumn: "PlayerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Players_CoachPlayerID",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "TeamPlayers");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CoachPlayerID",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Club",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CoachPlayerID",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Stadium",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Goals",
                table: "Players",
                newName: "TeamID");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamID",
                table: "Players",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserID",
                table: "Players",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamID",
                table: "Players",
                column: "TeamID",
                principalTable: "Teams",
                principalColumn: "TeamID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Users_UserID",
                table: "Players",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
