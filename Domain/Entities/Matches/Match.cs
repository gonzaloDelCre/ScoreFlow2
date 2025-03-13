using Domain.Enum;
using Domain.Entities.MatchEvents;
using Domain.Entities.MatchReferees;
using Domain.Entities.PlayerStatistics;
using Domain.Entities.Teams;

namespace Domain.Entities.Matches
{
    public class Match
    {
        public int MatchID { get; set; }
        public int Team1ID { get; set; }
        public int Team2ID { get; set; }
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public ICollection<MatchEvent> MatchEvents { get; set; } = new List<MatchEvent>();
        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();
        public ICollection<MatchReferee> MatchReferees { get; set; } = new List<MatchReferee>();
    }


}
