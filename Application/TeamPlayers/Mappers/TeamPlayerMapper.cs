using Application.TeamPlayers.DTOs;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.Mappers
{
    public static class TeamPlayerMapper
    {
        // Convierte un objeto de dominio TeamPlayer a DTO de respuesta
        public static TeamPlayerResponseDTO ToResponseDTO(this TeamPlayer teamPlayer)
        {
            return new TeamPlayerResponseDTO
            {
                TeamID = teamPlayer.TeamID.Value,
                PlayerID = teamPlayer.PlayerID.Value,
                TeamName = teamPlayer.Team.Name.Value,
                PlayerName = teamPlayer.Player.Name.Value,
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
