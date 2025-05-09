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
            _importService = importService;
        }

        public async Task ExecuteAsync()
        {
            await _importService.ImportAsync();
        }
    }
}
