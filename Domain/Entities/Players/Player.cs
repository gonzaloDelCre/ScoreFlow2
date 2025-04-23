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

        private List<TeamPlayer> teamPlayers;
        public IReadOnlyCollection<TeamPlayer> TeamPlayers => teamPlayers.AsReadOnly();

        public Player(
            PlayerID playerID,
            PlayerName name,
            PlayerPosition position,
            PlayerAge age,
            int goals,
            string? photo,
            DateTime createdAt,
            List<TeamPlayer> teamPlayers)
        {
            PlayerID = playerID;
            Name = name;
            Position = position;
            Age = age;
            Goals = goals;
            Photo = photo;
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
            this.teamPlayers = teamPlayers ?? new List<TeamPlayer>();
        }

        public void Update(PlayerName name, PlayerPosition position, PlayerAge age, int goals, string? photo, DateTime createdAt)
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
            if (teamPlayer != null && !teamPlayers.Contains(teamPlayer))
                teamPlayers.Add(teamPlayer);
        }
    }
}