using Domain.Entities.Leagues;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Entities.Standings
{
    public class Standing
    {
        public StandingID StandingID { get; private set; }
        public LeagueID LeagueID { get; private set; }
        public TeamID TeamID { get; private set; }
        public Points Points { get; private set; }
        public MatchesPlayed MatchesPlayed { get; private set; }
        public Wins Wins { get; private set; }
        public Draws Draws { get; private set; }
        public Losses Losses { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public League League { get; private set; }
        public Team Team { get; private set; }

        public Standing(StandingID standingID, LeagueID leagueID, TeamID teamID, Points points, MatchesPlayed matchesPlayed, Wins wins, Draws draws, Losses losses, League league, Team team, DateTime createdAt)
        {
            StandingID = standingID ?? throw new ArgumentNullException(nameof(standingID));
            LeagueID = leagueID ?? throw new ArgumentNullException(nameof(leagueID));
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            Points = points ?? throw new ArgumentNullException(nameof(points));
            MatchesPlayed = matchesPlayed ?? throw new ArgumentNullException(nameof(matchesPlayed));
            Wins = wins ?? throw new ArgumentNullException(nameof(wins));
            Draws = draws ?? throw new ArgumentNullException(nameof(draws));
            Losses = losses ?? throw new ArgumentNullException(nameof(losses));
            League = league ?? throw new ArgumentNullException(nameof(league));
            Team = team ?? throw new ArgumentNullException(nameof(team));
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        }
    }

}
