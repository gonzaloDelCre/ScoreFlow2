using Infrastructure.Services.Scraping.Matches.Import;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Scraping
{
    public class ImportMatchUseCase
    {
        private readonly MatchImportService _importService;
        private readonly ILogger<ImportMatchUseCase> _logger;

        public ImportMatchUseCase(
            MatchImportService importService,
            ILogger<ImportMatchUseCase> logger)
        {
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Importa partidos para una liga específica
        /// </summary>
        /// <param name="leagueId">ID de la liga en la base de datos</param>
        /// <param name="competitionId">ID externo de la competición (opcional)</param>
        /// <returns>Tarea asincrónica</returns>
        /// <exception cref="ArgumentException">Si la liga no existe</exception>
        /// <exception cref="InvalidOperationException">Si hay problemas con la extracción</exception>
        public async Task ExecuteAsync(int leagueId, string competitionId = null)
        {
            if (leagueId <= 0)
                throw new ArgumentException("El ID de la liga debe ser un número positivo", nameof(leagueId));

            _logger.LogInformation($"Ejecutando importación de partidos para liga ID {leagueId}");

            try
            {
                await _importService.ImportAsync(leagueId, competitionId);
                _logger.LogInformation($"Importación de partidos para liga ID {leagueId} completada con éxito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante la importación de partidos para liga ID {leagueId}");
                throw; // Propagar la excepción para que el controlador la maneje
            }
        }
    }
}
