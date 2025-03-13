using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teams.DTOs
{
    public class TeamDTO
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public int? CoachID { get; set; } // CoachID puede ser null
        public DateTime CreatedAt { get; set; }

        public ICollection<int> PlayerIDs { get; set; } = new List<int>();
        public ICollection<int> TeamLeagueIDs { get; set; } = new List<int>();
        public ICollection<int> StandingIDs { get; set; } = new List<int>();
    }
}
