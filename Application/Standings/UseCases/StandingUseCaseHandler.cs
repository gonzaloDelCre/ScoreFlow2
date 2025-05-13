using Application.Standings.DTOs;
using Application.Standings.UseCases.Create;
using Application.Standings.UseCases.Delete;
using Application.Standings.UseCases.Get;
using Application.Standings.UseCases.Scraping;
using Application.Standings.UseCases.Update;
using Domain.Shared;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<StandingUseCaseHandler> _logger;

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
            ImportStandingsUseCase importStandings,
            ILogger<StandingUseCaseHandler> logger)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            _update = update ?? throw new ArgumentNullException(nameof(update));
            _delete = delete ?? throw new ArgumentNullException(nameof(delete));
            _getAll = getAll ?? throw new ArgumentNullException(nameof(getAll));
            _getById = getById ?? throw new ArgumentNullException(nameof(getById));
            _getByLeague = getByLeague ?? throw new ArgumentNullException(nameof(getByLeague));
            _getByTeamAndLeague = getByTeamAndLeague ?? throw new ArgumentNullException(nameof(getByTeamAndLeague));
            _getClassification = getClassification ?? throw new ArgumentNullException(nameof(getClassification));
            _getTop = getTop ?? throw new ArgumentNullException(nameof(getTop));
            _getByGdRange = getByGdRange ?? throw new ArgumentNullException(nameof(getByGdRange));
            _importStandings = importStandings ?? throw new ArgumentNullException(nameof(importStandings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<StandingResponseDTO> CreateAsync(StandingRequestDTO dto)
        {
            _logger.LogDebug("Ejecutando caso de uso para crear standing");
            return await _create.ExecuteAsync(dto);
        }

        public async Task<StandingResponseDTO?> UpdateAsync(StandingRequestDTO dto)
        {
            _logger.LogDebug("Ejecutando caso de uso para actualizar standing ID={ID}", dto?.ID);
            return await _update.ExecuteAsync(dto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogDebug("Ejecutando caso de uso para eliminar standing ID={ID}", id);
            return await _delete.ExecuteAsync(id);
        }

        public async Task<List<StandingResponseDTO>> GetAllAsync()
        {
            _logger.LogDebug("Ejecutando caso de uso para obtener todos los standings");
            return await _getAll.ExecuteAsync();
        }

        public async Task<StandingResponseDTO?> GetByIdAsync(int id)
        {
            _logger.LogDebug("Ejecutando caso de uso para obtener standing por ID={ID}", id);
            return await _getById.ExecuteAsync(id);
        }

        public async Task<List<StandingResponseDTO>> GetByLeagueAsync(int leagueId)
        {
            _logger.LogDebug("Ejecutando caso de uso para obtener standings por liga ID={LeagueID}", leagueId);
            return await _getByLeague.ExecuteAsync(leagueId);
        }

        public async Task<StandingResponseDTO?> GetByTeamAndLeagueAsync(int teamId, int leagueId)
        {
            _logger.LogDebug("Ejecutando caso de uso para obtener standing por equipo ID={TeamID} y liga ID={LeagueID}",
                teamId, leagueId);
            return await _getByTeamAndLeague.ExecuteAsync(teamId, leagueId);
        }

        public async Task<List<StandingResponseDTO>> GetClassificationAsync(int leagueId)
        {
            _logger.LogDebug("Ejecutando caso de uso para obtener clasificación completa de liga ID={LeagueID}", leagueId);
            return await _getClassification.ExecuteAsync(leagueId);
        }

        public async Task<List<StandingResponseDTO>> GetTopByPointsAsync(int topN)
        {
            _logger.LogDebug("Ejecutando caso de uso para obtener top {TopN} equipos por puntos", topN);
            if (topN <= 0)
                throw new ArgumentException("El número de equipos debe ser mayor que cero", nameof(topN));

            return await _getTop.ExecuteAsync(topN);
        }

        public async Task<List<StandingResponseDTO>> GetByGoalDifferenceRangeAsync(int minGD, int maxGD)
        {
            _logger.LogDebug("Ejecutando caso de uso para obtener standings por diferencia de goles entre {MinGD} y {MaxGD}",
                minGD, maxGD);
            if (maxGD < minGD)
                throw new ArgumentException("El valor máximo debe ser mayor o igual que el mínimo", nameof(maxGD));

            return await _getByGdRange.ExecuteAsync(minGD, maxGD);
        }

        public async Task<int> ImportStandingsAsync(int competitionId, int leagueId)
        {
            _logger.LogInformation("Ejecutando caso de uso para importar clasificación: competitionId={CompetitionId}, leagueId={LeagueId}",
                competitionId, leagueId);

            try
            {
                return await _importStandings.ExecuteAsync(competitionId, leagueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al importar clasificación: competitionId={CompetitionId}, leagueId={LeagueId}",
                    competitionId, leagueId);
                throw;
            }
        }
    }
}
