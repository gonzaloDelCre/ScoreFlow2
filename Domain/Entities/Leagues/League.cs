using Domain.Shared;
using Domain.Entities.Standings;
using Domain.Entities.TeamLeagues;
using Domain.Services.Leagues;

namespace Domain.Entities.Leagues
{
    public class League
    {
        public LeagueID LeagueID { get; private set; }
        public LeagueName Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<TeamLeague> TeamLeagues { get; private set; } = new List<TeamLeague>();
        public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

        public League(LeagueID leagueID, LeagueName name, string description, DateTime createdAt)
        {
            LeagueID = leagueID;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }

        public void Update(LeagueName name, string description, DateTime createdAt)
        {
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }
    }
}