using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Leagues;
using Domain.Entities.Teams;

namespace Domain.Entities.TeamLeagues
{
    public class TeamLeague
    {
        public int TeamID { get; set; }
        public int LeagueID { get; set; }

        public Team Team { get; set; }
        public League League { get; set; }
    }

}
