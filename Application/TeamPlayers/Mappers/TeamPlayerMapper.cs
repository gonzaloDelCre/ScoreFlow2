using Application.TeamPlayers.DTOs;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Application.TeamPlayers.Mappers
{
    public static class TeamPlayerMapper
    {
        public static TeamPlayerResponseDTO ToResponseDTO(this TeamPlayer teamPlayer)
        {
            return new TeamPlayerResponseDTO
            {
                TeamID = teamPlayer.TeamID.Value,
                PlayerID = teamPlayer.PlayerID.Value,
                TeamName = teamPlayer.Team?.Name.Value ?? "Sin nombre",
                PlayerName = teamPlayer.Player?.Name.Value ?? "Sin nombre",
                JoinedAt = teamPlayer.JoinedAt,
                RoleInTeam = teamPlayer.RoleInTeam
            };
        }

        public static TeamPlayer ToDomain(this TeamPlayerRequestDTO dto, Team team, Player player)
        {
            return new TeamPlayer(
                new TeamID(dto.TeamID),
                new PlayerID(dto.PlayerID),
                dto.JoinedAt,
                dto.RoleInTeam,
                team,
                player
            );
        }
    }
}