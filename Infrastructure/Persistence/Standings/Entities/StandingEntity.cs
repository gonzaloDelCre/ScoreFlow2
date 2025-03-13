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
    public class StandingEntity
    {
        [Key]
        public int StandingID { get; set; }

        [Required]
        [ForeignKey("League")]
        public int LeagueID { get; set; }
        public LeagueEntity League { get; set; }

        [Required]
        [ForeignKey("Team")]
        public int TeamID { get; set; }
        public TeamEntity Team { get; set; }

        public int Points { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int GoalDifference { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
