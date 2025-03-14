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
        public Team Team1 { get; private set; }
        public Team Team2 { get; private set; }
        public DateTime MatchDate { get; private set; }
        public MatchStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string Location { get; private set; }

        public ICollection<MatchEvent> MatchEvents { get; private set; } = new List<MatchEvent>();
        public ICollection<PlayerStatistic> PlayerStatistics { get; private set; } = new List<PlayerStatistic>();
        public ICollection<MatchReferee> MatchReferees { get; private set; } = new List<MatchReferee>();

        // Constructor
        public Match(MatchID matchID, Team team1, Team team2, DateTime matchDate, MatchStatus status, string location)
        {
            MatchID = matchID ?? throw new ArgumentNullException(nameof(matchID));  
            Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));       
            Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));
            MatchDate = matchDate == default ? throw new ArgumentException("La fecha del partido es obligatoria.") : matchDate;  
            Status = status;
            Location = location ?? throw new ArgumentNullException(nameof(location), "La ubicación es obligatoria."); 
        }

        public void Update(Team team1, Team team2, DateTime matchDate, MatchStatus status, string location)
        {
            Team1 = team1;
            Team2 = team2;
            MatchDate = matchDate;
            Status = status;
            Location = location;
        }
        public void AddMatchEvent(MatchEvent matchEvent)
        {
            if (matchEvent == null)
                throw new ArgumentNullException(nameof(matchEvent));
            MatchEvents.Add(matchEvent);
        }

        public void AddPlayerStatistic(PlayerStatistic playerStatistic)
        {
            if (playerStatistic == null)
                throw new ArgumentNullException(nameof(playerStatistic));
            PlayerStatistics.Add(playerStatistic);
        }

        public void AddMatchReferee(MatchReferee matchReferee)
        {
            if (matchReferee == null)
                throw new ArgumentNullException(nameof(matchReferee));
            MatchReferees.Add(matchReferee);
        }
    }

}
