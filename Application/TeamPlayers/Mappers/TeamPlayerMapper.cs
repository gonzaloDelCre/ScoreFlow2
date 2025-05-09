using Application.TeamPlayers.DTOs;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Application.TeamPlayers.Mappers
{
    public static class TeamPlayerMapper
    {
        public static TeamPlayerResponseDTO ToDTO(this TeamPlayer tp)
            => new TeamPlayerResponseDTO
            {
                ID = tp.ID.Value,
                TeamID = tp.TeamID.Value,
                TeamName = tp.Team.Name.Value,
                PlayerID = tp.PlayerID.Value,
                PlayerName = tp.Player.Name.Value,
                JoinedAt = tp.JoinedAt.Value,
                RoleInTeam = tp.RoleInTeam
            };

        public static TeamPlayer ToDomain(this TeamPlayerRequestDTO dto)
            => new TeamPlayer(
                id: new TeamPlayerID(dto.ID ?? 0),
                teamID: new TeamID(dto.TeamID),
                playerID: new PlayerID(dto.PlayerID),
                joinedAt: new JoinedAt(dto.JoinedAt),
                roleInTeam: dto.RoleInTeam,
                team: null!,
                player: null!
            );
    }
}