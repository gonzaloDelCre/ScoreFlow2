using Domain.Entities.Matches;
using Domain.Entities.Players;
using Domain.Shared;

namespace Domain.Entities.MatchEvents
{
    public class MatchEvent
    {
        public MatchEventID MatchEventID { get; private set; }
        public MatchID MatchID { get; private set; }
        public PlayerID? PlayerID { get; private set; }
        public EventType EventType { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Match Match { get; private set; }
        public Player? Player { get; private set; }

        public MatchEvent(MatchEventID matchEventID, MatchID matchID, PlayerID? playerID, EventType eventType, Match match, Player? player, DateTime createdAt)
        {
            MatchEventID = matchEventID ?? throw new ArgumentNullException(nameof(matchEventID));
            MatchID = matchID ?? throw new ArgumentNullException(nameof(matchID));
            PlayerID = playerID;
            EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
            Match = match ?? throw new ArgumentNullException(nameof(match));
            Player = player;
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        }
    }


}
