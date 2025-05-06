using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Players
{
    public interface IPlayerImporter
    {
        Task ImportByTeamExternalIdAsync(int teamExternalId);
    }
}
