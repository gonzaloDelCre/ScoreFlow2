using Infrastructure.Persistence.Leagues.Entities;
using Infrastructure.Persistence.Matches.Entities;
using Infrastructure.Persistence.MatchEvents.Entities;
using Infrastructure.Persistence.MatchReferees.Entities;
using Infrastructure.Persistence.Notifications.Entities;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.PlayerStatistics.Entities;
using Infrastructure.Persistence.Referees.Entities;
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
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<RefereeEntity> Referees { get; set; }
        public DbSet<MatchRefereeEntity> MatchReferees { get; set; }
        public DbSet<TeamPlayerEntity> TeamPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Team ←→ League (1:N)
            modelBuilder.Entity<TeamEntity>()
                .HasOne(t => t.League)
                .WithMany(l => l.Teams)
                .HasForeignKey(t => t.LeagueID)
                .OnDelete(DeleteBehavior.Cascade);

            // TeamPlayers (PK compuesta)
            modelBuilder.Entity<TeamPlayerEntity>()
                .HasKey(tp => new { tp.TeamID, tp.PlayerID });
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

            // MatchReferees (PK compuesta)
            modelBuilder.Entity<MatchRefereeEntity>()
                .HasKey(mr => new { mr.MatchID, mr.RefereeID });
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

            // MatchEntity relaciones
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

            // Eventos de partido
            modelBuilder.Entity<MatchEventEntity>()
                .HasOne(me => me.Match)
                .WithMany(m => m.MatchEvents)
                .HasForeignKey(me => me.MatchID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MatchEventEntity>()
                .HasOne(me => me.Player)
                .WithMany()
                .HasForeignKey(me => me.PlayerID)
                .OnDelete(DeleteBehavior.SetNull);

            // Estadísticas de jugador
            modelBuilder.Entity<PlayerStatisticEntity>()
                .HasOne(ps => ps.Match)
                .WithMany(m => m.PlayerStatistics)
                .HasForeignKey(ps => ps.MatchID)
                .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<PlayerStatisticEntity>()
            //    .HasOne(ps => ps.Player)
            //    .WithMany(p => p.PlayerStatistics)
            //    .HasForeignKey(ps => ps.PlayerID)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Clasificaciones
            modelBuilder.Entity<StandingEntity>()
                        .HasOne(s => s.League)
                        .WithMany(l => l.Standings)
                        .HasForeignKey(s => s.LeagueID)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StandingEntity>()
                        .HasOne(s => s.Team)
                        .WithMany(t => t.Standings)
                        .HasForeignKey(s => s.TeamID)
                        .OnDelete(DeleteBehavior.Cascade);

            // Notificaciones
            modelBuilder.Entity<NotificationEntity>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Unicidad email en usuario
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Longitudes
            modelBuilder.Entity<UserEntity>().Property(u => u.FullName).HasMaxLength(100);
            modelBuilder.Entity<UserEntity>().Property(u => u.Email).HasMaxLength(255);
            modelBuilder.Entity<TeamEntity>().Property(t => t.Name).HasMaxLength(100);
            modelBuilder.Entity<LeagueEntity>().Property(l => l.Name).HasMaxLength(100);
            modelBuilder.Entity<RefereeEntity>().Property(r => r.Name).HasMaxLength(100);
        }
    }
}
