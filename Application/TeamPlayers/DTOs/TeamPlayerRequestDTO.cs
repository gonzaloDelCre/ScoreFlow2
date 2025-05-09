using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.DTOs
{
    public class TeamPlayerRequestDTO
    {
        public int? ID { get; set; }
        public int TeamID { get; set; }
        public int PlayerID { get; set; }
        public DateTime JoinedAt { get; set; }
        public RoleInTeam? RoleInTeam { get; set; }
    }
}
