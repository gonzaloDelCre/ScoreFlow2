using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.Leagues.Entities;

namespace Infrastructure.Persistence.Standings.Entities
{
    [Table("Standings")]
    public class StandingEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int LeagueID { get; set; }

        [ForeignKey(nameof(LeagueID))]
        public LeagueEntity League { get; set; }

        [Required]
        public int TeamID { get; set; }

        [ForeignKey(nameof(TeamID))]
        public TeamEntity Team { get; set; }

        [Required]
        public int Points { get; set; } = 0;

        [Required]
        public int Wins { get; set; } = 0;

        [Required]
        public int Losses { get; set; } = 0;

        [Required]
        public int Draws { get; set; } = 0;

        [Required]
        public int GoalDifference { get; set; } = 0;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
