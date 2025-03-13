using Domain.Shared;
using Domain.Entities.Standings;
using Domain.Entities.TeamLeagues;

namespace Domain.Entities.Leagues
{
    public class League
    {
        public LeagueID LeagueID { get; private set; }
        public LeagueName Name { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<TeamLeague> TeamLeagues { get; private set; } = new List<TeamLeague>();
        public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

        public League(LeagueID leagueID, LeagueName name)
        {
            LeagueID = leagueID ?? throw new ArgumentNullException(nameof(leagueID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }


}
