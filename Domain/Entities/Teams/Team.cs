using Domain.Entities.Players;
using Domain.Entities.Standings;
using Domain.Entities.TeamLeagues;
using Domain.Entities.Users;

namespace Domain.Entities.Teams
{
    public class Team
    {
        public int TeamID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? CoachID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? Coach { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<TeamLeague> TeamLeagues { get; set; } = new List<TeamLeague>();
        public ICollection<Standing> Standings { get; set; } = new List<Standing>();
    }

}
