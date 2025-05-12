using Infrastructure.Services.Scraping.Matches.Import;
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

        public ImportMatchUseCase(MatchImportService importService)
        {
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));
        }

        /// <summary>
        /// Importa partidos para una liga específica
        /// </summary>
        /// <param name="leagueId">ID de la liga en la base de datos</param>
        /// <param name="competitionId">ID externo de la competición (opcional)</param>
        public async Task ExecuteAsync(int leagueId, string competitionId = null)
        {
            await _importService.ImportAsync(leagueId, competitionId);
        }
    }
}
