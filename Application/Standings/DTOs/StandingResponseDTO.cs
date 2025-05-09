using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.DTOs
{
    public class StandingResponseDTO
    {
        public int ID { get; set; }
        public int LeagueID { get; set; }
        public string LeagueName { get; set; } = null!;
        public int TeamID { get; set; }
        public string TeamName { get; set; } = null!;
        public int Points { get; set; }
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalDifference { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
