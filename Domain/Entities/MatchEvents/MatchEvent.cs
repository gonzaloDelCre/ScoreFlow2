using Domain.Entities.Matches;
using Domain.Entities.Players;
using Domain.Shared;
using Domain.Enum;

namespace Domain.Entities.MatchEvents
{
    public class MatchEvent
    {
        public MatchEventID MatchEventID { get; private set; }
        public MatchID MatchID { get; private set; }
        public PlayerID? PlayerID { get; private set; }
        public EventType EventType { get; private set; }
        public int Minute { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Match Match { get; private set; }
        public Player? Player { get; private set; }

        public MatchEvent(
            MatchEventID matchEventID,
            MatchID matchID,
            PlayerID? playerID,
            EventType eventType,
            int minute,
            Match match,
            Player? player,
            DateTime? createdAt = null)
        {
            MatchEventID = matchEventID
                ?? throw new ArgumentNullException(nameof(matchEventID));
            MatchID = matchID
                ?? throw new ArgumentNullException(nameof(matchID));
            PlayerID = playerID;
            EventType = eventType;
            Minute = minute >= 0
                ? minute
                : throw new ArgumentOutOfRangeException(nameof(minute));
            Match = match
                ?? throw new ArgumentNullException(nameof(match));
            Player = player;
            CreatedAt = createdAt == null || createdAt == default
                ? DateTime.UtcNow
                : createdAt.Value;
        }

        protected MatchEvent() { }

        public void Update(
            EventType eventType,
            int minute,
            Player? player)
        {
            EventType = eventType;
            Minute = minute;
            Player = player;
            PlayerID = player?.PlayerID;
        }
    }
}
