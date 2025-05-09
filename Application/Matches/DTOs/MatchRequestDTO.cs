using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.DTOs
{
    public class MatchRequestDTO
    {
        public int? ID { get; set; }
        public int Team1ID { get; set; }
        public int Team2ID { get; set; }
        public int LeagueID { get; set; }    
        public int Jornada { get; set; }    
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }
        public string Location { get; set; } = null!;
        public int ScoreTeam1 { get; set; }
        public int ScoreTeam2 { get; set; }
    }
}
