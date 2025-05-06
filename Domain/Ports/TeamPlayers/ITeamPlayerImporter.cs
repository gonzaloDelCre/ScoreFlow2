using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.TeamPlayers
{
    public interface ITeamPlayerImporter
    {
        Task LinkPlayersToTeamAsync(int teamExternalId);
    }
}
