using System;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.TeamPlayers
{
    public class TeamPlayer
    {
        public TeamPlayerID ID { get; private set; }
        public TeamID TeamID { get; private set; }
        public PlayerID PlayerID { get; private set; }
        public JoinedAt JoinedAt { get; private set; }
        public RoleInTeam? RoleInTeam { get; private set; }

        public Team Team { get; private set; }
        public Player Player { get; private set; }

        public TeamPlayer(
            TeamPlayerID id,
            TeamID teamID,
            PlayerID playerID,
            JoinedAt joinedAt,
            RoleInTeam? roleInTeam = null,
            Team team = null,
            Player player = null)
        {
            ID = id ?? throw new ArgumentNullException(nameof(id));
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            PlayerID = playerID ?? throw new ArgumentNullException(nameof(playerID));
            JoinedAt = joinedAt ?? throw new ArgumentNullException(nameof(joinedAt));
            RoleInTeam = roleInTeam;
            Team = team;
            Player = player;
        }

        public TeamPlayer(TeamID teamID, PlayerID playerID, JoinedAt? joinedAt = null, RoleInTeam? roleInTeam = null)
            : this(new TeamPlayerID(0), teamID, playerID, joinedAt ?? new JoinedAt(DateTime.UtcNow), roleInTeam)
        { }

        protected TeamPlayer() { }

        public void UpdateRole(RoleInTeam? newRole)
            => RoleInTeam = newRole;

        public void UpdateJoinedAt(JoinedAt joinedAt)
            => JoinedAt = joinedAt ?? JoinedAt;
    }
}
