using Domain.Entities.TeamPlayers;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.Players
{
    public class Player
    {
        public PlayerID PlayerID { get; private set; }
        public PlayerName Name { get; private set; }
        public PlayerPosition Position { get; private set; }
        public PlayerAge Age { get; private set; }
        public int Goals { get; private set; }
        public string? Photo { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private readonly List<TeamPlayer> teamPlayers = new();
        public IReadOnlyCollection<TeamPlayer> TeamPlayers => teamPlayers.AsReadOnly();

        public Player(
            PlayerID playerID,
            PlayerName name,
            PlayerPosition position,
            PlayerAge age,
            int goals,
            string? photo,
            DateTime? createdAt,
            IEnumerable<TeamPlayer>? teamPlayers = null)
        {
            PlayerID = playerID ?? throw new ArgumentNullException(nameof(playerID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Position = position;
            Age = age ?? throw new ArgumentNullException(nameof(age));
            Goals = goals >= 0
                        ? goals
                        : throw new ArgumentOutOfRangeException(nameof(goals));
            Photo = photo;
            CreatedAt = createdAt == null || createdAt == default
                        ? DateTime.UtcNow
                        : createdAt.Value;

            if (teamPlayers != null)
                foreach (var tp in teamPlayers)
                    AddTeamPlayer(tp);
        }

        protected Player() { }

        public void Update(
            PlayerName name,
            PlayerPosition position,
            PlayerAge age,
            int goals,
            string? photo,
            DateTime createdAt)
        {
            Name = name;
            Position = position;
            Age = age;
            Goals = goals;
            Photo = photo;
            CreatedAt = createdAt;
        }

        public void AddTeamPlayer(TeamPlayer teamPlayer)
        {
            if (teamPlayer == null)
                throw new ArgumentNullException(nameof(teamPlayer));
            if (!teamPlayers.Contains(teamPlayer))
                teamPlayers.Add(teamPlayer);
        }

        public void SetTeamPlayers(IEnumerable<TeamPlayer> list)
        {
            teamPlayers.Clear();
            foreach (var tp in list) AddTeamPlayer(tp);
        }
    }
}