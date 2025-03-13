
using Domain.Entities.Matches;
using Domain.Entities.Players;

namespace Domain.Entities.MatchEvents
{
    public class MatchEvent
    {
        public int MatchEventID { get; set; }
        public int MatchID { get; set; }
        public int? PlayerID { get; set; }
        public string EventType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Match Match { get; set; }
        public Player? Player { get; set; }
    }


}
