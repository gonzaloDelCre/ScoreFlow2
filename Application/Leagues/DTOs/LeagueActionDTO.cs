using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.DTOs
{
    public class LeagueActionDTO
    {
        public string Action { get; set; }
        public int? LeagueID { get; set; }
        public LeagueRequestDTO? League { get; set; }
    }
}
