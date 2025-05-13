using Domain.Shared;
using Infrastructure.Services.Scraping.Standings.Import;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Scraping
{
    public class ImportStandingsUseCase
    {
        private readonly StandingsImportService _importService;
        private readonly ILogger<ImportStandingsUseCase> _logger;

        public ImportStandingsUseCase(
            StandingsImportService importService,
            ILogger<ImportStandingsUseCase> logger)
        {
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> ExecuteAsync(int competitionId, int leagueId)
        {
            // Validar parámetros de entrada
            if (competitionId <= 0)
            {
                _logger.LogWarning("Intento de importar clasificación con competitionId inválido: {CompetitionId}", competitionId);
                throw new ArgumentException("El ID de competición debe ser mayor que cero", nameof(competitionId));
            }

            if (leagueId <= 0)
            {
                _logger.LogWarning("Intento de importar clasificación con leagueId inválido: {LeagueId}", leagueId);
                throw new ArgumentException("El ID de liga debe ser mayor que cero", nameof(leagueId));
            }

            _logger.LogInformation("Iniciando caso de uso de importación de clasificación: competitionId={CompetitionId}, leagueId={LeagueId}",
                competitionId, leagueId);

            try
            {
                var result = await _importService.ImportAsync(competitionId, leagueId);
                _logger.LogInformation("Importación completada exitosamente: {ProcessedRows} filas procesadas", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en caso de uso de importación para competitionId={CompetitionId}, leagueId={LeagueId}",
                    competitionId, leagueId);
                throw;
            }
        }
    }
}
