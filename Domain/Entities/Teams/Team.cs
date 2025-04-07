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
        public User? Coach { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<Player> Players { get; private set; } = new List<Player>();
        public ICollection<TeamLeague> TeamLeagues { get; private set; } = new List<TeamLeague>();
        public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

        public string Logo { get; private set; } = string.Empty;

        // Constructor
        public Team(TeamID teamID, TeamName name, User? coach, DateTime createdAt, string logo)
        {
            TeamID = teamID;
            Name = name;
            Coach = coach;
            CreatedAt = createdAt;
            Logo = logo;
        }

        // Default constructor for ORM/EF Core
        public Team()
        {
        }

        public Team(TeamID teamID, TeamName name, User? coach, DateTime createdAt, string logo, List<Player> players) : this(teamID, name, coach, createdAt, logo)
        {
        }

        // Update method to modify team details
        public void Update(TeamName name, User coach, string logo)
        {
            if (name != null)
                Name = name;

            if (!string.IsNullOrWhiteSpace(logo))
                Logo = logo;
        }
    }   
}
