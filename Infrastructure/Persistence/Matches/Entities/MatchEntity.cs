using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enum;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.MatchEvents.Entities;
using Infrastructure.Persistence.PlayerStatistics.Entities;

namespace Infrastructure.Persistence.Matches.Entities
{
    [Table("Matches")]
    public class MatchEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Team1ID { get; set; }

        [ForeignKey(nameof(Team1ID))]
        public TeamEntity Team1 { get; set; }

        [Required]
        public int Team2ID { get; set; }

        [ForeignKey(nameof(Team2ID))]
        public TeamEntity Team2 { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public int ScoreTeam1 { get; set; } = 0;

        public int ScoreTeam2 { get; set; } = 0;

        [Required]
        public MatchStatus Status { get; set; }

        [MaxLength(255)]
        public string Location { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MatchEventEntity> MatchEvents { get; set; } = new List<MatchEventEntity>();

        public ICollection<PlayerStatisticEntity> PlayerStatistics { get; set; } = new List<PlayerStatisticEntity>();
    }
}
