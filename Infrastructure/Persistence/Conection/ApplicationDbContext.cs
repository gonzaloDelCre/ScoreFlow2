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
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchEvent> MatchEvents { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<TeamLeague> TeamLeagues { get; set; }
        public DbSet<Standing> Standings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<MatchReferee> MatchReferees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamLeague>().HasKey(tl => new { tl.TeamID, tl.LeagueID });
            modelBuilder.Entity<MatchReferee>().HasKey(mr => new { mr.MatchID, mr.RefereeID });

            // Configurar relaciones y restricciones
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Coach)
                .WithMany()
                .HasForeignKey(t => t.CoachID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany()
                .HasForeignKey(p => p.TeamID)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Team1)
                .WithMany()
                .HasForeignKey(m => m.Team1ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Team2)
                .WithMany()
                .HasForeignKey(m => m.Team2ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchEvent>()
                .HasOne(me => me.Match)
                .WithMany()
                .HasForeignKey(me => me.MatchID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchEvent>()
                .HasOne(me => me.Player)
                .WithMany()
                .HasForeignKey(me => me.PlayerID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(ps => ps.Match)
                .WithMany()
                .HasForeignKey(ps => ps.MatchID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(ps => ps.Player)
                .WithMany()
                .HasForeignKey(ps => ps.PlayerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamLeague>()
                .HasOne(tl => tl.Team)
                .WithMany()
                .HasForeignKey(tl => tl.TeamID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamLeague>()
                .HasOne(tl => tl.League)
                .WithMany()
                .HasForeignKey(tl => tl.LeagueID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Standing>()
                .HasOne(s => s.League)
                .WithMany()
                .HasForeignKey(s => s.LeagueID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Standing>()
                .HasOne(s => s.Team)
                .WithMany()
                .HasForeignKey(s => s.TeamID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchReferee>()
                .HasKey(mr => new { mr.MatchID, mr.RefereeID });

            modelBuilder.Entity<MatchReferee>()
                .HasOne(mr => mr.Match)
                .WithMany(m => m.MatchReferees) 
                .HasForeignKey(mr => mr.MatchID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchReferee>()
                .HasOne(mr => mr.Referee)
                .WithMany(r => r.MatchReferees) 
                .HasForeignKey(mr => mr.RefereeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configurar restricciones de longitud en `VARCHAR`
            modelBuilder.Entity<UserEntity>().Property(u => u.FullName).HasMaxLength(100);
            modelBuilder.Entity<UserEntity>().Property(u => u.Email).HasMaxLength(255);
            modelBuilder.Entity<Team>().Property(t => t.Name).HasMaxLength(100);
            modelBuilder.Entity<League>().Property(l => l.Name).HasMaxLength(100);
            modelBuilder.Entity<Referee>().Property(r => r.Name).HasMaxLength(100);

        }

    }
}
