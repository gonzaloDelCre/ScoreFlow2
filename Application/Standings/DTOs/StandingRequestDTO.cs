using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.DTOs
{
    public class StandingRequestDTO
    {
        public int? ID { get; set; }
        public int LeagueID { get; set; }
        public int TeamID { get; set; }
        public int Points { get; set; }
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalDifference { get; set; }
    }
}
