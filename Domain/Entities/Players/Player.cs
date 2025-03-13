using Domain.Entities.MatchEvents;
using Domain.Entities.PlayerStatistics;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.Players
{
    public class Player
    {
        public PlayerID PlayerID { get; private set; }
        public PlayerName Name { get; private set; }
        public TeamID TeamID { get; private set; }
        public PlayerPosition Position { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Team Team { get; private set; }
        public ICollection<MatchEvent> MatchEvents { get; private set; }
        public ICollection<PlayerStatistic> PlayerStatistics { get; private set; }

        public Player(PlayerID playerID, PlayerName name, TeamID teamID, PlayerPosition position, Team team, DateTime createdAt)
        {
            PlayerID = playerID ?? throw new ArgumentNullException(nameof(playerID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            Position = position;
            Team = team ?? throw new ArgumentNullException(nameof(team));
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
            MatchEvents = new List<MatchEvent>();
            PlayerStatistics = new List<PlayerStatistic>();
        }

        public Player()
        {
            MatchEvents = new List<MatchEvent>();
            PlayerStatistics = new List<PlayerStatistic>();
        }


    }
}
