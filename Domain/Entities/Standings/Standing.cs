using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Leagues;
using Domain.Entities.Teams;

namespace Domain.Entities.Standings
{
    public class Standing
    {
        public int StandingID { get; set; }
        public int LeagueID { get; set; }
        public int TeamID { get; set; }
        public int Points { get; set; }
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public League League { get; set; }
        public Team Team { get; set; }
    }

}
