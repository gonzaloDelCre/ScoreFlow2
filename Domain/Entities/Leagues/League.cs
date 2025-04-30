using System;
using System.Collections.Generic;
using Domain.Entities.Standings;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Entities.Leagues
{
    public class League
    {
        public LeagueID LeagueID { get; private set; }
        public LeagueName Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

        // ← colección directa de equipos
        public ICollection<Team> Teams { get; private set; } = new List<Team>();

        public League(LeagueID leagueID,
                      LeagueName name,
                      string description,
                      DateTime? createdAt = null)
        {
            LeagueID = leagueID ?? throw new ArgumentNullException(nameof(leagueID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            CreatedAt = createdAt ?? DateTime.UtcNow;
        }

        // Sobrecarga vacía para ORMs
        private League() { }

        public void Update(LeagueName name, string description, DateTime createdAt)
        {
            Name = name;
            Description = description;
        }

        public void AddTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            if (!Teams.Contains(team)) Teams.Add(team);
        }
    }
}
