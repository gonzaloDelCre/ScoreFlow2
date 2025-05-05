using System;
using System.Collections.Generic;
using Domain.Entities.Players;
using Domain.Entities.Standings;
using Domain.Entities.Leagues;
using Domain.Shared;

namespace Domain.Entities.Teams
{
    public class Team
    {
        private TeamID teamID;
        private TeamName teamName;
        private DateTime utcNow;
        private string logoUrl;
        private TeamID teamID1;
        private string value;

        public TeamID TeamID { get; private set; }
        public TeamName Name { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string Logo { get; private set; }
        public string? Category { get; private set; }
        public string? Club { get; private set; }
        public string? Stadium { get; private set; }
        public string? ExternalID { get; private set; }

        public Player? Coach { get; private set; }

        // ← RELACIÓN 1:N: un equipo pertenece a una liga
        public League League { get; private set; }
        public LeagueID LeagueID => League.LeagueID;

        public ICollection<Player> Players { get; private set; } = new List<Player>();
        public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

        public Team(TeamID teamID,
                    TeamName name,
                    League league,
                    string logo,
                    DateTime? createdAt = null,
                    string? externalID = null)
        {
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            League = league ?? throw new ArgumentNullException(nameof(league));
            Logo = !string.IsNullOrWhiteSpace(logo)
                          ? logo
                          : throw new ArgumentException("El logo no puede estar vacío.", nameof(logo));
            CreatedAt = createdAt ?? DateTime.UtcNow;
            ExternalID = externalID;
        }

        // Sobrecarga vacía para ORMs
        public Team() { }

        public Team(TeamID teamID, TeamName teamName, DateTime createdAt, string? logo, string? externalID)
        {
            this.teamID = teamID;
            this.teamName = teamName;
            CreatedAt = createdAt;
            Logo = logo;
            ExternalID = externalID;
        }

        public Team(TeamID teamID, TeamName teamName, DateTime utcNow, string logoUrl)
        {
            this.teamID = teamID;
            this.teamName = teamName;
            this.utcNow = utcNow;
            this.logoUrl = logoUrl;
        }

        public Team(TeamID teamID1, string value, DateTime createdAt, string logo, string? category, string? club, string? stadium)
        {
            this.teamID1 = teamID1;
            this.value = value;
            CreatedAt = createdAt;
            Logo = logo;
            Category = category;
            Club = club;
            Stadium = stadium;
        }

        public void Update(TeamName? name = null,
                           string? logo = null,
                           string? category = null,
                           string? club = null,
                           string? stadium = null)
        {
            if (name != null) Name = name;
            if (!string.IsNullOrWhiteSpace(logo)) Logo = logo;
            Category = category;
            Club = club;
            Stadium = stadium;
        }

        public void AssignCoach(Player coach)
        {
            Coach = coach ?? throw new ArgumentNullException(nameof(coach));
        }

        public void AddPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (!Players.Contains(player)) Players.Add(player);
        }

        public void AddStanding(Standing standing)
        {
            if (standing == null) throw new ArgumentNullException(nameof(standing));
            if (!Standings.Contains(standing)) Standings.Add(standing);
        }

        public void Update(string? category, string? club, string? stadium)
        {
            Category = category;
            Club = club;
            Stadium = stadium;
        }

        public void SetCategory(string? category)
        {
            Category = category;
        }

        public void SetClub(string? club)
        {
            Club = club;
        }

        public void SetStadium(string? stadium)
        {
            Stadium = stadium;
        }

        public void SetExternalID(string extId)
        {
            ExternalID = extId;
        }
    }
}
