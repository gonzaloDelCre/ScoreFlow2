using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Services.Players;
using Domain.Shared;

namespace Application.Playes.UseCases.Get
{
    public class GetPlayerById
    {
        private readonly PlayerService _playerService;

        public GetPlayerById(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<PlayerResponseDTO?> ExecuteAsync(PlayerID playerId)
        {
            var player = await _playerService.GetPlayerByIdAsync(playerId);
            return player != null ? PlayerMapper.ToResponseDTO(player) : null;
        }
    }
}
