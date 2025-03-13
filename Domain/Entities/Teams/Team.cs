using Domain.Entities.Players;
using Domain.Entities.Standings;
using Domain.Entities.TeamLeagues;
using Domain.Entities.Users;
using Domain.Shared;

namespace Domain.Entities.Teams
{
    public class Team
    {
        public TeamID TeamID { get; private set; }
        public TeamName Name { get; private set; }
        public User? Coach { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<TeamLeague> TeamLeagues { get; set; } = new List<TeamLeague>();
        public ICollection<Standing> Standings { get; set; } = new List<Standing>();

        public Team(TeamID teamID, TeamName name, User? coach, DateTime createdAt)
        {
            TeamID = teamID;
            Name = name;
            Coach = coach;
            CreatedAt = createdAt;
        }

    }

}
