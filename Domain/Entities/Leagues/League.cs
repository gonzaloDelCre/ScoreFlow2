using Domain.Shared;
using Domain.Entities.Standings;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Leagues
{
    public class League
    {
        public LeagueID LeagueID { get; private set; }
        public LeagueName Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

        public League(LeagueID leagueID, LeagueName name, string description, DateTime createdAt)
        {
            LeagueID = leagueID;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }

        public League()
        {
        }

        public League(LeagueID leagueID, LeagueName name)
        {
            LeagueID = leagueID;
            Name = name;
        }

        public void Update(LeagueName name, string description, DateTime createdAt)
        {
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }

        public void UpdateName(LeagueName leagueName)
        {
            Name = leagueName;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}
