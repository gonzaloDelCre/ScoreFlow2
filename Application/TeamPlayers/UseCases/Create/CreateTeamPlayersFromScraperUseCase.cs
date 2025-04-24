using Infrastructure.Services.Scraping.TeamPlayers.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Create
{
    public class CreateTeamPlayersFromScraperUseCase
    {
        private readonly TeamPlayerImportService _importer;

        public CreateTeamPlayersFromScraperUseCase(TeamPlayerImportService importer)
        {
            _importer = importer;
        }

        public async Task ExecuteAsync(int teamId)
        {
            await _importer.LinkPlayersToTeamAsync(teamId);
        }
    }

}
