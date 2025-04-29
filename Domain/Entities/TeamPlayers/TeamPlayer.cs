using System;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.TeamPlayers
{
    public class TeamPlayer
    {
        private TeamID teamID;

        public TeamID TeamID { get; private set; }
        public PlayerID PlayerID { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public RoleInTeam? RoleInTeam { get; private set; }

        // Relaciones
        public Team Team { get; private set; }
        public Player Player { get; private set; }

        // Constructor
        public TeamPlayer(TeamID teamID, PlayerID playerID, DateTime joinedAt, RoleInTeam? roleInTeam = null, Team team = null, Player player = null)
        {
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            PlayerID = playerID ?? throw new ArgumentNullException(nameof(playerID));
            JoinedAt = joinedAt;
            RoleInTeam = roleInTeam;
            Team = team;  
            Player = player; 
        }

        public TeamPlayer(TeamID teamID)
        {
            this.teamID = teamID;
        }

        // Método de actualización para la relación (ejemplo: actualizar rol del jugador)
        public void UpdateRole(RoleInTeam? newRoleInTeam)
        {
            RoleInTeam = newRoleInTeam;
        }
    }
}
