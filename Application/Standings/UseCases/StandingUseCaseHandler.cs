using Application.Standings.DTOs;
using Application.Standings.UseCases.Create;
using Application.Standings.UseCases.Delete;
using Application.Standings.UseCases.Get;
using Application.Standings.UseCases.Scraping;
using Application.Standings.UseCases.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases
{
    public class StandingUseCaseHandler
    {
        private readonly CreateStandingUseCase _create;
        private readonly UpdateStandingUseCase _update;
        private readonly DeleteStandingUseCase _delete;
        private readonly GetAllStandingsUseCase _getAll;
        private readonly GetStandingByIdUseCase _getById;
        private readonly GetStandingsByLeagueIdUseCase _getByLeague;
        private readonly GetStandingByTeamAndLeagueUseCase _getByTeamAndLeague;
        private readonly GetClassificationByLeagueIdUseCase _getClassification;
        private readonly GetTopByPointsUseCase _getTop;
        private readonly GetByGoalDifferenceRangeUseCase _getByGdRange;
        private readonly ImportStandingsUseCase _importStandings;

        public StandingUseCaseHandler(
            CreateStandingUseCase create,
            UpdateStandingUseCase update,
            DeleteStandingUseCase delete,
            GetAllStandingsUseCase getAll,
            GetStandingByIdUseCase getById,
            GetStandingsByLeagueIdUseCase getByLeague,
            GetStandingByTeamAndLeagueUseCase getByTeamAndLeague,
            GetClassificationByLeagueIdUseCase getClassification,
            GetTopByPointsUseCase getTop,
            GetByGoalDifferenceRangeUseCase getByGdRange,
            ImportStandingsUseCase importStandings)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _getAll = getAll;
            _getById = getById;
            _getByLeague = getByLeague;
            _getByTeamAndLeague = getByTeamAndLeague;
            _getClassification = getClassification;
            _getTop = getTop;
            _getByGdRange = getByGdRange;
            _importStandings = importStandings;
        }

        public Task<StandingResponseDTO> CreateAsync(StandingRequestDTO dto) => _create.ExecuteAsync(dto);
        public Task<StandingResponseDTO?> UpdateAsync(StandingRequestDTO dto) => _update.ExecuteAsync(dto);
        public Task<bool> DeleteAsync(int id) => _delete.ExecuteAsync(id);
        public Task<List<StandingResponseDTO>> GetAllAsync() => _getAll.ExecuteAsync();
        public Task<StandingResponseDTO?> GetByIdAsync(int id) => _getById.ExecuteAsync(id);
        public Task<List<StandingResponseDTO>> GetByLeagueAsync(int leagueId) => _getByLeague.ExecuteAsync(leagueId);
        public Task<StandingResponseDTO?> GetByTeamAndLeagueAsync(int teamId, int leagueId) => _getByTeamAndLeague.ExecuteAsync(teamId, leagueId);
        public Task<List<StandingResponseDTO>> GetClassificationAsync(int leagueId) => _getClassification.ExecuteAsync(leagueId);
        public Task<List<StandingResponseDTO>> GetTopByPointsAsync(int topN) => _getTop.ExecuteAsync(topN);
        public Task<List<StandingResponseDTO>> GetByGoalDifferenceRangeAsync(int minGD, int maxGD) => _getByGdRange.ExecuteAsync(minGD, maxGD);
        public async Task ImportStandingsAsync()
        {
            await _importStandings.ExecuteAsync();
        }
    }
}
