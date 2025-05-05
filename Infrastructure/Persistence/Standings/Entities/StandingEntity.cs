using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Leagues.Entities;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.Standings.Entities
{
    public class StandingEntity
    {
        [Key]
        public int StandingID { get; set; }

        // Foreign key property
        public int LeagueID { get; set; }

        // Navigation property with explicit FK mapping
        [ForeignKey(nameof(LeagueID))]
        public LeagueEntity League { get; set; }

        [Required]
        public int TeamID { get; set; }

        [ForeignKey(nameof(TeamID))]
        public TeamEntity Team { get; set; }

        public int Wins { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int GoalsFor { get; set; } = 0;
        public int GoalsAgainst { get; set; } = 0;
        public int Points { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
