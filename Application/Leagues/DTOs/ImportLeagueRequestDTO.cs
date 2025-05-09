using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.DTOs
{
    public class ImportLeagueRequestDTO
    {
        public int CompetitionId { get; set; }
        public int LeagueScopeId { get; set; }
        public int SeasonId { get; set; }
        public int LeagueId { get; set; }  
    }
}
