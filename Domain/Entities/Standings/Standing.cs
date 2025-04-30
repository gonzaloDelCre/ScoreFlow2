using System;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Entities.Standings
{
    public class Standing
    {
        public StandingID StandingID { get; private set; }

        public LeagueID LeagueID { get; private set; }

        public TeamID TeamID { get; private set; }

        public Wins Wins { get; private set; }
        public Draws Draws { get; private set; }
        public Losses Losses { get; private set; }
        public GoalsFor GoalsFor { get; private set; }
        public GoalsAgainst GoalsAgainst { get; private set; }
        public Points Points { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Team Team { get; private set; }

        // 🛠️ Constructor principal
        public Standing(
            StandingID standingID = null,
            LeagueID leagueID = null, // ← NUEVO
            TeamID teamID = null,
            Wins wins = null,
            Draws draws = null,
            Losses losses = null,
            GoalsFor goalsFor = null,
            GoalsAgainst goalsAgainst = null,
            Points points = null,
            Team team = null,
            DateTime? createdAt = null)
        {
            StandingID = standingID ?? new StandingID(0);
            LeagueID = leagueID ?? throw new ArgumentNullException(nameof(leagueID)); // ← requerido
            TeamID = teamID ?? new TeamID(0);
            Wins = wins ?? new Wins(0);
            Draws = draws ?? new Draws(0);
            Losses = losses ?? new Losses(0);
            GoalsFor = goalsFor ?? new GoalsFor(0);
            GoalsAgainst = goalsAgainst ?? new GoalsAgainst(0);
            Points = points ?? new Points(0);
            Team = team ?? new Team();
            CreatedAt = createdAt ?? DateTime.UtcNow;
        }

        // Método para actualizar todos los stats de golpe
        public void Update(
            Wins wins,
            Draws draws,
            Losses losses,
            GoalsFor goalsFor,
            GoalsAgainst goalsAgainst,
            Points points)
        {
            Wins = wins ?? Wins;
            Draws = draws ?? Draws;
            Losses = losses ?? Losses;
            GoalsFor = goalsFor ?? GoalsFor;
            GoalsAgainst = goalsAgainst ?? GoalsAgainst;
            Points = points ?? Points;
        }

        // Métodos individuales
        public void UpdateWins(Wins wins) => Wins = wins ?? Wins;
        public void UpdateDraws(Draws draws) => Draws = draws ?? Draws;
        public void UpdateLosses(Losses losses) => Losses = losses ?? Losses;
        public void UpdateGoalsFor(GoalsFor goalsFor) => GoalsFor = goalsFor ?? GoalsFor;
        public void UpdateGoalsAgainst(GoalsAgainst goalsAgainst) => GoalsAgainst = goalsAgainst ?? GoalsAgainst;
        public void UpdatePoints(Points points) => Points = points ?? Points;
    }
}
