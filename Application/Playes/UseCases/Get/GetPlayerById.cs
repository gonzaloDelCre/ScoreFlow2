using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Services.Players;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Get
{
    public class GetPlayerById
    {
        private readonly PlayerService _playerService;
        private readonly PlayerMapper _mapper;

        public GetPlayerById(PlayerService playerService, PlayerMapper mapper)
        {
            _playerService = playerService;
            _mapper = mapper;
        }

        public async Task<PlayerResponseDTO?> ExecuteAsync(PlayerID playerId)
        {
            var player = await _playerService.GetPlayerByIdAsync(playerId);
            return player != null ? _mapper.MapToDTO(player) : null;
        }
    }
}
