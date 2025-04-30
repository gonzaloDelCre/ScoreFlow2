﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoalDifference",
                table: "Standings",
                newName: "GoalsFor");

            migrationBuilder.AddColumn<int>(
                name: "GoalsAgainst",
                table: "Standings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalsAgainst",
                table: "Standings");

            migrationBuilder.RenameColumn(
                name: "GoalsFor",
                table: "Standings",
                newName: "GoalDifference");
        }
    }
}
