using Domain.Ports.TeamPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Create
{
    public class CreateTeamPlayersFromScraperUseCase
    {
        private readonly ITeamPlayerImporter _importer;

        public CreateTeamPlayersFromScraperUseCase(ITeamPlayerImporter importer)
        {
            _importer = importer;
        }

        public async Task ExecuteAsync(int teamExternalId)
        {
            await _importer.LinkPlayersToTeamAsync(teamExternalId);
        }
    }

}
