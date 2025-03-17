using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.Mappers;
using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Entities.PlayerStatistics;
using Domain.Services.Players;
using Domain.Services.PlayerStatistics;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Get
{
    public class GetPlayerStatisticById
    {
        private readonly PlayerStatisticService _playerStatisticService;
        private readonly PlayerStatisticMapper _mapper;

        public GetPlayerStatisticById(PlayerStatisticService playerStatisticService, PlayerStatisticMapper mapper)
        {
            _playerStatisticService = playerStatisticService;
            _mapper = mapper;
        }

        public async Task<PlayerStatisticResponseDTO?> ExecuteAsync(int playerStatisticId)
        {
            var playerStatistic = await _playerStatisticService.GetPlayerStatisticByIdAsync(new PlayerStatisticID(playerStatisticId));
            return playerStatistic != null ? _mapper.MapToDTO(playerStatistic) : null;
        }
    }
}
