using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enum;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.MatchReferees.Entities;
using Infrastructure.Persistence.MatchEvents.Entities;
using Infrastructure.Persistence.PlayerStatistics.Entities;

namespace Infrastructure.Persistence.Matches.Entities
{
    public class MatchEntity
    {
        [Key]
        public int MatchID { get; set; }

        [Required]
        [ForeignKey("Team1")]
        public int Team1ID { get; set; }
        public TeamEntity Team1 { get; set; }

        [Required]
        [ForeignKey("Team2")]
        public int Team2ID { get; set; }
        public TeamEntity Team2 { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public int ScoreTeam1 { get; set; } = 0;
        public int ScoreTeam2 { get; set; } = 0;

        [Required]
        public MatchStatus Status { get; set; }

        public string Location { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MatchRefereeEntity> MatchReferees { get; set; }
        public ICollection<MatchEventEntity> MatchEvents { get; set; }
        public ICollection<PlayerStatisticEntity> PlayerStatistics { get; set; }
    }
}
