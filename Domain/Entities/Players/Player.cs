using Domain.Entities.MatchEvents;
using Domain.Entities.PlayerStatistics;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.Players
{
    public class Player
    {
        private PlayerID playerID;
        private PlayerName playerName;
        private PlayerPosition playerPosition;
        private PlayerAge playerAge;
        private List<TeamPlayer> teamPlayers; // Relación con equipos a través de TeamPlayer
        private PlayerID playerID1;

        public PlayerID PlayerID { get; private set; }
        public PlayerName Name { get; private set; }
        public PlayerPosition Position { get; private set; }
        public PlayerAge Age { get; private set; }
        public int Goals { get; private set; }
        public string? Photo { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Relación con los equipos a través de TeamPlayer
        public ICollection<TeamPlayer> TeamPlayers => teamPlayers.AsReadOnly();
        public ICollection<MatchEvent> MatchEvents { get; private set; }
        public ICollection<PlayerStatistic> PlayerStatistics { get; private set; }

        // Constructor actualizado
        public Player(
            PlayerID playerID,
            PlayerName name,
            PlayerPosition position,
            PlayerAge age,
            int goals,
            string? photo,
            DateTime createdAt,
            List<TeamPlayer> teamPlayers)  // Relacionado a través de TeamPlayer
        {
            PlayerID = playerID ?? throw new ArgumentNullException(nameof(playerID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Position = position;
            Age = age ?? throw new ArgumentNullException(nameof(age));
            Goals = goals;
            Photo = photo;
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
            this.teamPlayers = teamPlayers ?? throw new ArgumentNullException(nameof(teamPlayers)); // Lista de relaciones
        }

        public Player(PlayerID playerID1)
        {
            this.playerID1 = playerID1;
        }

        // Método para actualizar los detalles del jugador
        public void Update(PlayerName name, PlayerPosition position, PlayerAge age, int goals, string? photo, DateTime createdAt)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "El nombre del jugador no puede ser nulo.");

            Name = name;
            Position = position;
            Age = age;
            Goals = goals;
            Photo = photo;
            CreatedAt = createdAt;
        }

        // Métodos para actualizar propiedades individuales
        public void UpdateName(PlayerName name) => Name = name ?? throw new ArgumentNullException(nameof(name));
        public void UpdatePosition(PlayerPosition position) => Position = position;
        public void UpdateAge(PlayerAge age) => Age = age ?? throw new ArgumentNullException(nameof(age));
        public void UpdateGoals(int goals) => Goals = goals;
        public void UpdatePhoto(string? photo) => Photo = photo;
        public void AddTeamPlayer(TeamPlayer teamPlayer)
        {
            if (teamPlayer == null)
                throw new ArgumentNullException(nameof(teamPlayer));

            teamPlayers.Add(teamPlayer); // Agregar la relación TeamPlayer
        }

    }
}
