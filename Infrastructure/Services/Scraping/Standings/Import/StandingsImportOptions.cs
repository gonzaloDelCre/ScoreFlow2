using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Standings.Import
{
    public class StandingsImportOptions
    {
        public int CompetitionId { get; set; }
        public int LeagueId { get; set; }
    }
}
