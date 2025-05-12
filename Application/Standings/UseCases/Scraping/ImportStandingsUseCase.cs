using Domain.Shared;
using Infrastructure.Services.Scraping.Standings.Import;
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

        public ImportStandingsUseCase(StandingsImportService importService)
        {
            _importService = importService;
        }

        public Task<int> ExecuteAsync(int competitionId, int leagueId)
        {
            return _importService.ImportAsync(competitionId, leagueId);
        }

    }
}
