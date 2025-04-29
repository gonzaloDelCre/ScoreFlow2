using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.DTOs
{
    public class TeamPlayerSimpleDto
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = null!;
        public string Position { get; set; } = null!;
        public int Age { get; set; }
        public int Goals { get; set; }
        public string? Photo { get; set; }
    }
}
