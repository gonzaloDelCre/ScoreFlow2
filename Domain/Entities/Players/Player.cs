using Domain.Entities.MatchEvents;
using Domain.Entities.PlayerStatistics;
using Domain.Entities.Teams;
using Domain.Enum;

namespace Domain.Entities.Players
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeamID { get; set; }
        public PlayerPosition Position { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Team Team { get; set; }
        public ICollection<MatchEvent> MatchEvents { get; set; } = new List<MatchEvent>();
        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();
    }


}
