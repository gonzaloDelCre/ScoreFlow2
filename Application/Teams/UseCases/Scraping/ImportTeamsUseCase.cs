using Infrastructure.Services.Scraping.Teams.Import;
using System.Threading.Tasks;
using Domain.Shared;

namespace Application.Teams.UseCases.Scraping
{
    public class ImportTeamsUseCase
    {
        private readonly TeamImportService _importService;

        public ImportTeamsUseCase(TeamImportService importService)
        {
            _importService = importService;
        }

        public async Task ExecuteAsync()
        {

            await _importService.ImportAsync();

        }
    }
}
