using Domain.Enum;
using Domain.Entities.MatchEvents;
using Domain.Entities.PlayerStatistics;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Entities.Matches
{
    public class Match
    {
        public MatchID MatchID { get; private set; }
        public Team Team1 { get; private set; }
        public Team Team2 { get; private set; }
        public DateTime MatchDate { get; private set; }
        public MatchStatus Status { get; private set; }
        public string Location { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<MatchEvent> MatchEvents { get; private set; }
            = new List<MatchEvent>();
        public ICollection<PlayerStatistic> PlayerStatistics { get; private set; }
            = new List<PlayerStatistic>();

        public Match(
            MatchID matchID,
            Team team1,
            Team team2,
            DateTime matchDate,
            MatchStatus status,
            string location)
        {
            MatchID = matchID ?? throw new ArgumentNullException(nameof(matchID));
            Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));
            Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));
            MatchDate = matchDate == default
                         ? throw new ArgumentException("La fecha es obligatoria.")
                         : matchDate;
            Status = status;
            Location = !string.IsNullOrWhiteSpace(location)
                         ? location
                         : throw new ArgumentNullException(nameof(location));
            CreatedAt = DateTime.UtcNow;
        }

        protected Match() { }

        public void Update(
            Team team1,
            Team team2,
            DateTime matchDate,
            MatchStatus status,
            string location)
        {
            Team1 = team1;
            Team2 = team2;
            MatchDate = matchDate;
            Status = status;
            Location = location;
        }

        public void AddMatchEvent(MatchEvent ev)
        {
            MatchEvents.Add(ev ?? throw new ArgumentNullException(nameof(ev)));
        }

        public void SetMatchEvents(IEnumerable<MatchEvent> events)
        {
            MatchEvents.Clear();
            foreach (var ev in events) MatchEvents.Add(ev);
        }

        public void AddPlayerStatistic(PlayerStatistic stat)
        {
            PlayerStatistics.Add(stat ?? throw new ArgumentNullException(nameof(stat)));
        }

        public void SetPlayerStatistics(IEnumerable<PlayerStatistic> stats)
        {
            PlayerStatistics.Clear();
            foreach (var s in stats) PlayerStatistics.Add(s);
        }
    }
}
