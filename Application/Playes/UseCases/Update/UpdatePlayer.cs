using Application.Playes.DTOs;
using Domain.Entities.Players;
using Domain.Services.Players;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Update
{
    public class UpdatePlayer
    {
        private readonly PlayerService _playerService;

        public UpdatePlayer(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<PlayerResponseDTO> Execute(PlayerRequestDTO playerDTO, int playerId)
        {
            if (playerDTO == null)
                throw new ArgumentNullException(nameof(playerDTO), "El jugador no puede ser nulo.");

            var existingPlayer = await _playerService.GetPlayerByIdAsync(new PlayerID(playerId));
            if (existingPlayer == null)
                throw new InvalidOperationException("El jugador no existe. No se puede actualizar.");

            existingPlayer.Update(
                new PlayerName(playerDTO.Name),
                playerDTO.TeamID,
                playerDTO.Position,
                playerDTO.CreatedAt
            );

            await _playerService.UpdatePlayerAsync(existingPlayer);
            return new PlayerResponseDTO
            {
                PlayerID = existingPlayer.PlayerID,
                Name = existingPlayer.Name.Value,
                TeamID = existingPlayer.TeamID,
                Position = existingPlayer.Position,
                CreatedAt = existingPlayer.CreatedAt
            };
        }
    }
}
