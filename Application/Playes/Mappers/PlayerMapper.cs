using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Enum;
using Domain.Shared;

namespace Application.Playes.Mappers
{
    public static class PlayerMapper
    {
        // Map DTO → Dominio (sin relaciones, para crear)
        public static Player ToDomain(this PlayerRequestDTO dto)
        {
            return new Player(
                new PlayerID(0), // Se asignará en base de datos
                new PlayerName(dto.Name),
                dto.Position,
                new PlayerAge(dto.Age),
                dto.Goals,
                dto.Photo,
                dto.CreatedAt,
                new List<TeamPlayer>() // se agregarán después si hace falta
            );
        }

        // Map Dominio → DTO (con TeamPlayers)
        public static PlayerResponseDTO ToResponseDTO(this Player player)
        {
            return new PlayerResponseDTO
            {
                PlayerID = player.PlayerID,
                Name = player.Name.Value,
                Age = player.Age.Value,
                Position = player.Position,
                Goals = player.Goals,
                Photo = player.Photo,
                CreatedAt = player.CreatedAt,
                TeamPlayers = player.TeamPlayers?
                    .Select(tp => tp.ToResponseDTO())
                    .ToList() ?? new List<TeamPlayerResponseDTO>()
            };
        }

        // Lista completa
        public static List<PlayerResponseDTO> ToResponseDTOList(this IEnumerable<Player> players)
        {
            return players.Select(p => p.ToResponseDTO()).ToList();
        }
    }
}
