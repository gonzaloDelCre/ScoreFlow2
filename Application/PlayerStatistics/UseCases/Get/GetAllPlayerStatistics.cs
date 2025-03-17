using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.Mappers;
using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Services.Players;
using Domain.Services.PlayerStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Get
{
    public class GetAllPlayerStatistics
    {
        private readonly PlayerStatisticService _playerStatisticService;
        private readonly PlayerStatisticMapper _mapper;

        public GetAllPlayerStatistics(PlayerStatisticService playerStatisticService, PlayerStatisticMapper mapper)
        {
            _playerStatisticService = playerStatisticService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlayerStatisticResponseDTO>> ExecuteAsync()
        {
            var playerStatistics = await _playerStatisticService.GetAllPlayerStatisticsAsync();
            return playerStatistics.Select(stat => _mapper.MapToDTO(stat));
        }
    }
}
