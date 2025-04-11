using System;
using System.Collections.Generic;
using Domain.Entities.TeamLeagues;
using Domain.Entities.Standings;
using Domain.Entities.Players;
using Domain.Shared;

namespace Domain.Entities.Teams
{
    public class Team
    {
        public TeamID TeamID { get; private set; }
        public TeamName Name { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string Logo { get; private set; } = string.Empty;
        public string? Category { get; private set; }
        public string? Club { get; private set; }
        public string? Stadium { get; private set; }

        public ICollection<Player> Players { get; private set; } = new List<Player>();
        public ICollection<TeamLeague> TeamLeagues { get; private set; } = new List<TeamLeague>();
        public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

        public Player Coach { get; private set; }

        public Team(TeamID teamID, TeamName name, DateTime createdAt, string logo)
        {
            if (string.IsNullOrWhiteSpace(logo))
                throw new ArgumentException("El logo no puede estar vacío.", nameof(logo));
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Logo = logo;
            CreatedAt = createdAt;
        }

        public Team() { }

        public void Update(TeamName name = null, string logo = null, string? category = null, string? club = null, string? stadium = null)
        {
            if (name != null) Name = name;
            if (!string.IsNullOrWhiteSpace(logo)) Logo = logo;
            Category = category;
            Club = club;
            Stadium = stadium;
        }

        public void AddPlayer(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!Players.Contains(player))
                Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            Players.Remove(player);
        }

        public void AssignCoach(Player coach)
        {
            if (coach == null)
                throw new ArgumentNullException(nameof(coach));
            Coach = coach;
        }

        public void AddTeamLeague(TeamLeague teamLeague)
        {
            if (teamLeague == null)
                throw new ArgumentNullException(nameof(teamLeague));
            if (!TeamLeagues.Contains(teamLeague))
                TeamLeagues.Add(teamLeague);
        }

        public void AddStanding(Standing standing)
        {
            if (standing == null)
                throw new ArgumentNullException(nameof(standing));
            if (!Standings.Contains(standing))
                Standings.Add(standing);
        }
    }
}
