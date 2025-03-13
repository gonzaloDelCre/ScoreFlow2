
using Domain.Entities.Standings;
using Domain.Entities.TeamLeagues;

namespace Domain.Entities.Leagues
{
    public class League
    {
        public int LeagueID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TeamLeague> TeamLeagues { get; set; } = new List<TeamLeague>();
        public ICollection<Standing> Standings { get; set; } = new List<Standing>();
    }


}
