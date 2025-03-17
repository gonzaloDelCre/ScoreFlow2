using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Services.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Get
{
    public class GetAllPlayer
    {
        private readonly PlayerService _playerService;
        private readonly PlayerMapper _mapper;

        public GetAllPlayer(PlayerService playerService, PlayerMapper mapper)
        {
            _playerService = playerService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlayerResponseDTO>> ExecuteAsync()
        {
            var players = await _playerService.GetAllPlayersAsync();
            return players.Select(player => _mapper.MapToDTO(player));
        }
    }
}
