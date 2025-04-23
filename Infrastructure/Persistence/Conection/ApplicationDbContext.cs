using Infrastructure.Persistence.Leagues.Entities;
using Infrastructure.Persistence.Matches.Entities;
using Infrastructure.Persistence.MatchEvents.Entities;
using Infrastructure.Persistence.MatchReferees.Entities;
using Infrastructure.Persistence.Notifications.Entities;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.PlayerStatistics.Entities;
using Infrastructure.Persistence.Referees.Entities;
using Infrastructure.Persistence.Standings.Entities;
using Infrastructure.Persistence.TeamLeagues.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Conection
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Definir las tablas
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<MatchEventEntity> MatchEvents { get; set; }
        public DbSet<PlayerStatisticEntity> PlayerStatistics { get; set; }
        public DbSet<LeagueEntity> Leagues { get; set; }
        public DbSet<TeamLeagueEntity> TeamLeagues { get; set; }
        public DbSet<StandingEntity> Standings { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<RefereeEntity> Referees { get; set; }
        public DbSet<MatchRefereeEntity> MatchReferees { get; set; }
        public DbSet<TeamPlayerEntity> TeamPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamLeagueEntity>().HasKey(tl => new { tl.TeamID, tl.LeagueID });
            modelBuilder.Entity<MatchRefereeEntity>().HasKey(mr => new { mr.MatchID, mr.RefereeID });

            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Team1)
                .WithMany()
                .HasForeignKey(m => m.Team1ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Team2)
                .WithMany()
                .HasForeignKey(m => m.Team2ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchEventEntity>()
                .HasOne(me => me.Match)
                .WithMany()
                .HasForeignKey(me => me.MatchID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchEventEntity>()
                .HasOne(me => me.Player)
                .WithMany()
                .HasForeignKey(me => me.PlayerID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlayerStatisticEntity>()
                .HasOne(ps => ps.Match)
                .WithMany()
                .HasForeignKey(ps => ps.MatchID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerStatisticEntity>()
                .HasOne(ps => ps.Player)
                .WithMany()
                .HasForeignKey(ps => ps.PlayerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamLeagueEntity>()
                .HasOne(tl => tl.Team)
                .WithMany()
                .HasForeignKey(tl => tl.TeamID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamLeagueEntity>()
                .HasOne(tl => tl.League)
                .WithMany()
                .HasForeignKey(tl => tl.LeagueID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StandingEntity>()
                .HasOne(s => s.League)
                .WithMany()
                .HasForeignKey(s => s.LeagueID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StandingEntity>()
                .HasOne(s => s.Team)
                .WithMany()
                .HasForeignKey(s => s.TeamID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotificationEntity>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchRefereeEntity>()
                .HasOne(mr => mr.Match)
                .WithMany(m => m.MatchReferees)
                .HasForeignKey(mr => mr.MatchID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchRefereeEntity>()
                .HasOne(mr => mr.Referee)
                .WithMany(r => r.MatchReferees)
                .HasForeignKey(mr => mr.RefereeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamPlayerEntity>()
                .HasOne(tp => tp.Team)
                .WithMany(t => t.TeamPlayers)
                .HasForeignKey(tp => tp.TeamID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamPlayerEntity>()
                .HasOne(tp => tp.Player)
                .WithMany(p => p.TeamPlayers)
                .HasForeignKey(tp => tp.PlayerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserEntity>().Property(u => u.FullName).HasMaxLength(100);
            modelBuilder.Entity<UserEntity>().Property(u => u.Email).HasMaxLength(255);
            modelBuilder.Entity<TeamEntity>().Property(t => t.Name).HasMaxLength(100);
            modelBuilder.Entity<LeagueEntity>().Property(l => l.Name).HasMaxLength(100);
            modelBuilder.Entity<RefereeEntity>().Property(r => r.Name).HasMaxLength(100);
        }
    }
}
