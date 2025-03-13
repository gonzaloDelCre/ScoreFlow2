using Domain.Enum;
using Domain.Entities.MatchEvents;
using Domain.Entities.MatchReferees;
using Domain.Entities.PlayerStatistics;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Entities.Matches
{
    public class Match
    {
        public MatchID MatchID { get; private set; }
        public TeamID Team1ID { get; private set; }
        public TeamID Team2ID { get; private set; }
        public DateTime MatchDate { get; private set; }
        public MatchStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Team Team1 { get; private set; }
        public Team Team2 { get; private set; }
        public ICollection<MatchEvent> MatchEvents { get; private set; } = new List<MatchEvent>();
        public ICollection<PlayerStatistic> PlayerStatistics { get; private set; } = new List<PlayerStatistic>();
        public ICollection<MatchReferee> MatchReferees { get; private set; } = new List<MatchReferee>();

        public Match(MatchID matchID, TeamID team1ID, TeamID team2ID, DateTime matchDate, MatchStatus status, Team team1, Team team2)
        {
            MatchID = matchID ?? throw new ArgumentNullException(nameof(matchID));
            Team1ID = team1ID ?? throw new ArgumentNullException(nameof(team1ID));
            Team2ID = team2ID ?? throw new ArgumentNullException(nameof(team2ID));
            MatchDate = matchDate == default ? throw new ArgumentException("La fecha del partido es obligatoria.") : matchDate;
            Status = status;
            Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));
            Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));
        }
    }


}
