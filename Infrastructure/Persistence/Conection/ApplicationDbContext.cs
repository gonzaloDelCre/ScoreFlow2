using Infrastructure.Persistence.Leagues.Entities;
using Infrastructure.Persistence.Matches.Entities;
using Infrastructure.Persistence.MatchEvents.Entities;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.PlayerStatistics.Entities;
using Infrastructure.Persistence.Standings.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Conection
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Tablas
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<MatchEventEntity> MatchEvents { get; set; }
        public DbSet<PlayerStatisticEntity> PlayerStatistics { get; set; }
        public DbSet<LeagueEntity> Leagues { get; set; }
        public DbSet<StandingEntity> Standings { get; set; }
        public DbSet<TeamPlayerEntity> TeamPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- Users ---
            modelBuilder.Entity<UserEntity>(b =>
            {
                b.HasKey(u => u.UserID);
                b.HasIndex(u => u.Email).IsUnique();
                b.Property(u => u.FullName).HasMaxLength(100);
                b.Property(u => u.Email).HasMaxLength(255);
                b.Property(u => u.PasswordHash).HasMaxLength(255);
                b.Property(u => u.Role).HasMaxLength(50);
            });

            // --- Leagues ---
            modelBuilder.Entity<LeagueEntity>(b =>
            {
                b.HasKey(l => l.ID);
                b.Property(l => l.Name).HasMaxLength(100).IsRequired();
                b.Property(l => l.Description).HasMaxLength(500);
                b.Property(l => l.CreatedAt).IsRequired();
                b.HasMany(l => l.Standings)
                 .WithOne(s => s.League)
                 .HasForeignKey(s => s.LeagueID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Teams ---
            modelBuilder.Entity<TeamEntity>(b =>
            {
                b.HasKey(t => t.TeamID);
                b.Property(t => t.ExternalID).HasMaxLength(100);
                b.Property(t => t.Name).HasMaxLength(100).IsRequired();
                b.Property(t => t.Category).HasMaxLength(100);
                b.Property(t => t.Club).HasMaxLength(100);
                b.Property(t => t.Stadium).HasMaxLength(255);
                b.Property(t => t.Logo).HasMaxLength(500);
                b.Property(t => t.CreatedAt).IsRequired();

                b.HasOne(t => t.Coach)
                 .WithMany()
                 .HasForeignKey(t => t.CoachPlayerID)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasMany(t => t.TeamPlayers)
                 .WithOne(tp => tp.Team)
                 .HasForeignKey(tp => tp.TeamID)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(t => t.Standings)
                 .WithOne(s => s.Team)
                 .HasForeignKey(s => s.TeamID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Players ---
            modelBuilder.Entity<PlayerEntity>(b =>
            {
                b.HasKey(p => p.PlayerID);
                b.Property(p => p.Name).HasMaxLength(100).IsRequired();
                b.Property(p => p.Position).HasMaxLength(50).IsRequired();
                b.Property(p => p.Age).IsRequired();
                b.Property(p => p.Goals).IsRequired();
                b.Property(p => p.Photo).HasMaxLength(500);
                b.Property(p => p.CreatedAt).IsRequired();

                b.HasMany(p => p.TeamPlayers)
                 .WithOne(tp => tp.Player)
                 .HasForeignKey(tp => tp.PlayerID)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany<PlayerStatisticEntity>()
                 .WithOne(ps => ps.Player)
                 .HasForeignKey(ps => ps.PlayerID)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany<MatchEventEntity>()
                 .WithOne(me => me.Player)
                 .HasForeignKey(me => me.PlayerID)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // --- Matches ---
            modelBuilder.Entity<MatchEntity>(b =>
            {
                b.HasKey(m => m.ID);
                b.Property(m => m.DateTime).IsRequired();
                b.Property(m => m.ScoreTeam1).IsRequired();
                b.Property(m => m.ScoreTeam2).IsRequired();
                b.Property(m => m.Status).IsRequired();
                b.Property(m => m.Location).HasMaxLength(255);
                b.Property(m => m.CreatedAt).IsRequired();

                b.HasOne(m => m.Team1)
                 .WithMany()
                 .HasForeignKey(m => m.Team1ID)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(m => m.Team2)
                 .WithMany()
                 .HasForeignKey(m => m.Team2ID)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(m => m.MatchEvents)
                 .WithOne(me => me.Match)
                 .HasForeignKey(me => me.MatchID)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(m => m.PlayerStatistics)
                 .WithOne(ps => ps.Match)
                 .HasForeignKey(ps => ps.MatchID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // --- MatchEvents ---
            modelBuilder.Entity<MatchEventEntity>(b =>
            {
                b.HasKey(me => me.ID);
                b.Property(me => me.EventType).IsRequired();
                b.Property(me => me.Minute).IsRequired();
                b.Property(me => me.CreatedAt).IsRequired();

                b.HasOne(me => me.Match)
                 .WithMany(m => m.MatchEvents)
                 .HasForeignKey(me => me.MatchID)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(me => me.Player)
                 .WithMany()
                 .HasForeignKey(me => me.PlayerID)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // --- PlayerStatistics ---
            modelBuilder.Entity<PlayerStatisticEntity>(b =>
            {
                b.HasKey(ps => ps.ID);
                b.Property(ps => ps.Goals).IsRequired();
                b.Property(ps => ps.Assists).IsRequired();
                b.Property(ps => ps.YellowCards).IsRequired();
                b.Property(ps => ps.RedCards).IsRequired();
                b.Property(ps => ps.MinutesPlayed);
                b.Property(ps => ps.CreatedAt).IsRequired();

                b.HasOne(ps => ps.Match)
                 .WithMany(m => m.PlayerStatistics)
                 .HasForeignKey(ps => ps.MatchID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // --- TeamPlayers ---
            modelBuilder.Entity<TeamPlayerEntity>(b =>
            {
                b.HasKey(tp => tp.ID);
                b.Property(tp => tp.RoleInTeam).IsRequired();
                b.Property(tp => tp.JoinedAt).IsRequired();

                b.HasOne(tp => tp.Team)
                 .WithMany(t => t.TeamPlayers)
                 .HasForeignKey(tp => tp.TeamID)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(tp => tp.Player)
                 .WithMany(p => p.TeamPlayers)
                 .HasForeignKey(tp => tp.PlayerID)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
