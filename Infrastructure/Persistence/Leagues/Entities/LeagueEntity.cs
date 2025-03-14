using Infrastructure.Persistence.Standings.Entities;
using Infrastructure.Persistence.TeamLeagues.Entities;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Leagues.Entities
{
    public class LeagueEntity
    {
        [Key]
        public int LeagueID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Relations
        public ICollection<TeamLeagueEntity> TeamLeagues { get; set; } = new List<TeamLeagueEntity>();
        public ICollection<StandingEntity> Standings { get; set; } = new List<StandingEntity>();
    }
}

