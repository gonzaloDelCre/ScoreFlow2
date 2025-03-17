using Application.Playes.DTOs;
using Domain.Entities.Players;
using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.Mappers
{
    public class PlayerMapper
    {
        public PlayerResponseDTO MapToDTO(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "La entidad de dominio Player no puede ser nula.");

            return new PlayerResponseDTO
            {
                PlayerID = player.PlayerID,
                Name = player.Name.Value,
                TeamID = player.TeamID,
                Position = player.Position,
                CreatedAt = player.CreatedAt
            };
        }

        public Player MapToDomain(PlayerRequestDTO playerDTO)
        {
            if (playerDTO == null)
                throw new ArgumentNullException(nameof(playerDTO), "El DTO PlayerRequestDTO no puede ser nulo.");

            var playerID = new PlayerID(0); 

            return new Player(
                playerID,
                new PlayerName(playerDTO.Name),
                playerDTO.TeamID,
                (PlayerPosition)Enum.Parse(typeof(PlayerPosition),playerDTO.Position.ToString()),
                null, 
                playerDTO.CreatedAt
            );
        }
    }
}
