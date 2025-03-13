using Domain.Entities.Leagues;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Entities.TeamLeagues
{
    public class TeamLeague
    {
        public TeamID TeamID { get; private set; }
        public LeagueID LeagueID { get; private set; }

        public Team Team { get; private set; }
        public League League { get; private set; }

        public TeamLeague(TeamID teamID, LeagueID leagueID, Team team, League league)
        {
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            LeagueID = leagueID ?? throw new ArgumentNullException(nameof(leagueID));
            Team = team ?? throw new ArgumentNullException(nameof(team));
            League = league ?? throw new ArgumentNullException(nameof(league));
        }
    }

}
