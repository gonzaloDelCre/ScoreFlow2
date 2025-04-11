using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Entities.Players;
using Domain.Services.Players;
using Domain.Shared;

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
                playerDTO.Position,
                new PlayerAge(playerDTO.Age),
                playerDTO.Goals,
                playerDTO.Photo,
                playerDTO.CreatedAt
            );

            await _playerService.UpdatePlayerAsync(
                existingPlayer.PlayerID,
                existingPlayer.Name,
                existingPlayer.Position,
                existingPlayer.Age,
                existingPlayer.Goals,
                existingPlayer.Photo,
                existingPlayer.CreatedAt
            );

            return new PlayerResponseDTO
            {
                PlayerID = existingPlayer.PlayerID,
                Name = existingPlayer.Name.Value,
                Position = existingPlayer.Position,
                Age = existingPlayer.Age.Value,
                Goals = existingPlayer.Goals,
                Photo = existingPlayer.Photo,
                CreatedAt = existingPlayer.CreatedAt
            };
        }
    }
}
