using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.DTOs
{
    public class TeamRosterDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public string? Logo { get; set; }
        public List<TeamPlayerSimpleDto> Players { get; set; } = new();
    }
}
