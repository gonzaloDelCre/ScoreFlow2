using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.DTOs
{
    public class TeamPlayerResponseDTO
    {
        public int ID { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; } = null!;
        public int PlayerID { get; set; }
        public string PlayerName { get; set; } = null!;
        public DateTime JoinedAt { get; set; }
        public RoleInTeam? RoleInTeam { get; set; }
    }
}
