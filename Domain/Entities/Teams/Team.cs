using System;
using System.Collections.Generic;
using Domain.Entities.Standings;
using Domain.Entities.Players;
using Domain.Shared;

namespace Domain.Entities.Teams
{
    public class Team
    {
        public TeamID TeamID { get; private set; }
        public TeamName Name { get; private set; }
        public LogoUrl Logo { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? Category { get; private set; }
        public string? Club { get; private set; }
        public string? Stadium { get; private set; }
        public string? ExternalID { get; private set; }

        public Player? Coach { get; private set; }
        private readonly List<Player> players = new();
        private readonly List<Standing> standings = new();

        public IReadOnlyCollection<Player> Players => players.AsReadOnly();
        public IReadOnlyCollection<Standing> Standings => standings.AsReadOnly();

        public Team(
            TeamID teamID,
            TeamName name,
            LogoUrl logo,
            DateTime createdAt,
            string? category = null,
            string? club = null,
            string? stadium = null,
            string? externalID = null,
            Player? coach = null)
        {
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Logo = logo ?? throw new ArgumentNullException(nameof(logo));
            CreatedAt = createdAt;
            Category = category;
            Club = club;
            Stadium = stadium;
            ExternalID = externalID;
            Coach = coach;
        }

        public Team(TeamID teamID, TeamName name, LogoUrl logo)
            : this(teamID, name, logo, DateTime.UtcNow) { }

        protected Team() { }

        public Team(TeamID teamID)
        {
            TeamID = teamID;
           
        }

        public void UpdateInfo(
            TeamName? name = null,
            LogoUrl? logo = null,
            string? category = null,
            string? club = null,
            string? stadium = null)
        {
            if (name != null) Name = name;
            if (logo != null) Logo = logo;
            Category = category;
            Club = club;
            Stadium = stadium;
        }

        public void SetExternalID(string externalID)
            => ExternalID = externalID;

        public void AssignCoach(Player coach)
            => Coach = coach ?? throw new ArgumentNullException(nameof(coach));

        public void AddPlayer(Player p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (!players.Contains(p)) players.Add(p);
        }

        public void RemovePlayer(Player p)
            => players.Remove(p);

        public void AddStanding(Standing s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (!standings.Contains(s)) standings.Add(s);
        }

        public void UpdateName(TeamName teamName)
        {
            if (teamName == null)
                throw new ArgumentNullException(nameof(teamName));
            Name = teamName;
        }

        public void UpdateLogo(LogoUrl logoUrl)
        {
            if (logoUrl == null)
                throw new ArgumentNullException(nameof(logoUrl));
            Logo = logoUrl;
        }

        public void SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("La categoría no puede estar vacía.", nameof(category));
            Category = category.Trim();
        }

        public void SetClub(string club)
        {
            if (string.IsNullOrWhiteSpace(club))
                throw new ArgumentException("El club no puede estar vacío.", nameof(club));
            Club = club.Trim();
        }

        public void SetStadium(string stadium)
        {
            if (string.IsNullOrWhiteSpace(stadium))
                throw new ArgumentException("El estadio no puede estar vacío.", nameof(stadium));
            Stadium = stadium.Trim();
        }

    }
}
