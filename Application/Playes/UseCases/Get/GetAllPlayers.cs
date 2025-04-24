using Domain.Services.Players;
using System.Linq;
using Application.Playes.DTOs;
using Application.Playes.Mappers;

namespace Application.Playes.UseCases.Get
{
    public class GetAllPlayer
    {
        private readonly PlayerService _playerService;

        public GetAllPlayer(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<IEnumerable<PlayerResponseDTO>> ExecuteAsync()
        {
            var players = await _playerService.GetAllPlayersAsync();
            return players.Select(player => PlayerMapper.ToResponseDTO(player));
        }
    }
}
