﻿// <auto-generated />
using System;
using Infrastructure.Persistence.Conection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250410121130_InitialCreate3")]
    partial class InitialCreate3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Persistence.Leagues.Entities.LeagueEntity", b =>
                {
                    b.Property<int>("LeagueID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeagueID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LeagueID");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Infrastructure.Persistence.MatchEvents.Entities.MatchEventEntity", b =>
                {
                    b.Property<int>("EventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventType")
                        .HasColumnType("int");

                    b.Property<int?>("MatchEntityMatchID")
                        .HasColumnType("int");

                    b.Property<int>("MatchID")
                        .HasColumnType("int");

                    b.Property<int>("Minute")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerID")
                        .HasColumnType("int");

                    b.HasKey("EventID");

                    b.HasIndex("MatchEntityMatchID");

                    b.HasIndex("MatchID");

                    b.HasIndex("PlayerID");

                    b.ToTable("MatchEvents");
                });

            modelBuilder.Entity("Infrastructure.Persistence.MatchReferees.Entities.MatchRefereeEntity", b =>
                {
                    b.Property<int>("MatchID")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("RefereeID")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.HasKey("MatchID", "RefereeID");

                    b.HasIndex("RefereeID");

                    b.ToTable("MatchReferees");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Matches.Entities.MatchEntity", b =>
                {
                    b.Property<int>("MatchID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MatchID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScoreTeam1")
                        .HasColumnType("int");

                    b.Property<int>("ScoreTeam2")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Team1ID")
                        .HasColumnType("int");

                    b.Property<int>("Team2ID")
                        .HasColumnType("int");

                    b.HasKey("MatchID");

                    b.HasIndex("Team1ID");

                    b.HasIndex("Team2ID");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Notifications.Entities.NotificationEntity", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("NotificationID");

                    b.HasIndex("UserID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Infrastructure.Persistence.PlayerStatistics.Entities.PlayerStatisticEntity", b =>
                {
                    b.Property<int>("StatID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatID"));

                    b.Property<int>("Assists")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Goals")
                        .HasColumnType("int");

                    b.Property<int?>("MatchEntityMatchID")
                        .HasColumnType("int");

                    b.Property<int>("MatchID")
                        .HasColumnType("int");

                    b.Property<int?>("MinutesPlayed")
                        .HasColumnType("int");

                    b.Property<int>("PlayerID")
                        .HasColumnType("int");

                    b.Property<int>("RedCards")
                        .HasColumnType("int");

                    b.Property<int>("YellowCards")
                        .HasColumnType("int");

                    b.HasKey("StatID");

                    b.HasIndex("MatchEntityMatchID");

                    b.HasIndex("MatchID");

                    b.HasIndex("PlayerID");

                    b.ToTable("PlayerStatistics");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Players.Entities.PlayerEntity", b =>
                {
                    b.Property<int>("PlayerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeamID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PlayerID");

                    b.HasIndex("TeamID");

                    b.HasIndex("UserID");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Referees.Entities.RefereeEntity", b =>
                {
                    b.Property<int>("RefereeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RefereeID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("RefereeID");

                    b.ToTable("Referees");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Standings.Entities.StandingEntity", b =>
                {
                    b.Property<int>("StandingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StandingID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Draws")
                        .HasColumnType("int");

                    b.Property<int>("GoalDifference")
                        .HasColumnType("int");

                    b.Property<int?>("LeagueEntityLeagueID")
                        .HasColumnType("int");

                    b.Property<int>("LeagueID")
                        .HasColumnType("int");

                    b.Property<int>("Losses")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("TeamID")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("StandingID");

                    b.HasIndex("LeagueEntityLeagueID");

                    b.HasIndex("LeagueID");

                    b.HasIndex("TeamID");

                    b.ToTable("Standings");
                });

            modelBuilder.Entity("Infrastructure.Persistence.TeamLeagues.Entities.TeamLeagueEntity", b =>
                {
                    b.Property<int>("TeamID")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("LeagueID")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LeagueEntityLeagueID")
                        .HasColumnType("int");

                    b.HasKey("TeamID", "LeagueID");

                    b.HasIndex("LeagueEntityLeagueID");

                    b.HasIndex("LeagueID");

                    b.ToTable("TeamLeagues");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Teams.Entities.TeamEntity", b =>
                {
                    b.Property<int>("TeamID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamID"));

                    b.Property<int>("CoachID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TeamID");

                    b.HasIndex("CoachID");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Users.Entities.UserEntity", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Infrastructure.Persistence.MatchEvents.Entities.MatchEventEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Matches.Entities.MatchEntity", null)
                        .WithMany("MatchEvents")
                        .HasForeignKey("MatchEntityMatchID");

                    b.HasOne("Infrastructure.Persistence.Matches.Entities.MatchEntity", "Match")
                        .WithMany()
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Persistence.Players.Entities.PlayerEntity", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Infrastructure.Persistence.MatchReferees.Entities.MatchRefereeEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Matches.Entities.MatchEntity", "Match")
                        .WithMany("MatchReferees")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Persistence.Referees.Entities.RefereeEntity", "Referee")
                        .WithMany("MatchReferees")
                        .HasForeignKey("RefereeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Referee");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Matches.Entities.MatchEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Teams.Entities.TeamEntity", "Team1")
                        .WithMany()
                        .HasForeignKey("Team1ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Infrastructure.Persistence.Teams.Entities.TeamEntity", "Team2")
                        .WithMany()
                        .HasForeignKey("Team2ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Team1");

                    b.Navigation("Team2");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Notifications.Entities.NotificationEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Users.Entities.UserEntity", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Infrastructure.Persistence.PlayerStatistics.Entities.PlayerStatisticEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Matches.Entities.MatchEntity", null)
                        .WithMany("PlayerStatistics")
                        .HasForeignKey("MatchEntityMatchID");

                    b.HasOne("Infrastructure.Persistence.Matches.Entities.MatchEntity", "Match")
                        .WithMany()
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Persistence.Players.Entities.PlayerEntity", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Players.Entities.PlayerEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Teams.Entities.TeamEntity", "Team")
                        .WithMany()
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Persistence.Users.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Standings.Entities.StandingEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Leagues.Entities.LeagueEntity", null)
                        .WithMany("Standings")
                        .HasForeignKey("LeagueEntityLeagueID");

                    b.HasOne("Infrastructure.Persistence.Leagues.Entities.LeagueEntity", "League")
                        .WithMany()
                        .HasForeignKey("LeagueID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Persistence.Teams.Entities.TeamEntity", "Team")
                        .WithMany()
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Infrastructure.Persistence.TeamLeagues.Entities.TeamLeagueEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Leagues.Entities.LeagueEntity", null)
                        .WithMany("TeamLeagues")
                        .HasForeignKey("LeagueEntityLeagueID");

                    b.HasOne("Infrastructure.Persistence.Leagues.Entities.LeagueEntity", "League")
                        .WithMany()
                        .HasForeignKey("LeagueID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Persistence.Teams.Entities.TeamEntity", "Team")
                        .WithMany()
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Teams.Entities.TeamEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Users.Entities.UserEntity", "Coach")
                        .WithMany()
                        .HasForeignKey("CoachID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Coach");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Leagues.Entities.LeagueEntity", b =>
                {
                    b.Navigation("Standings");

                    b.Navigation("TeamLeagues");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Matches.Entities.MatchEntity", b =>
                {
                    b.Navigation("MatchEvents");

                    b.Navigation("MatchReferees");

                    b.Navigation("PlayerStatistics");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Referees.Entities.RefereeEntity", b =>
                {
                    b.Navigation("MatchReferees");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Users.Entities.UserEntity", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
