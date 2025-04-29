using Infrastructure.Services.Scraping.Players.Import;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Create
{
    public class CreatePlayersFromScraperUseCase
    {
        private readonly PlayerImportService _importer;

        public CreatePlayersFromScraperUseCase(PlayerImportService importer)
        {
            _importer = importer;
        }

        /// <summary>
        /// Ejecuta el scraping e importación de jugadores para un equipo,
        /// usando su ExternalId en lugar del ID interno.
        /// </summary>
        public async Task ExecuteAsync(int teamExternalId)
        {
            await _importer.ImportByTeamExternalIdAsync(teamExternalId);
        }
    }
}
