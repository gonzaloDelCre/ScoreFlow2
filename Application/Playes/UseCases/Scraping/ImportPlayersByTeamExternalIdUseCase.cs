using Infrastructure.Services.Scraping.Players.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Scraping
{
    public class ImportPlayersByTeamExternalIdUseCase
    {
        private readonly PlayerImportService _importService;

        public ImportPlayersByTeamExternalIdUseCase(PlayerImportService importService)
        {
            _importService = importService;
        }

        public async Task ExecuteAsync(int teamExternalId)
        {
            await _importService.ImportByTeamExternalIdAsync(teamExternalId);
        }
    }
}
