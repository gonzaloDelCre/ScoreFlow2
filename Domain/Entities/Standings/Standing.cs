using System;
using Domain.Entities.Teams;
using Domain.Entities.Leagues;
using Domain.Shared;

namespace Domain.Entities.Standings
{
    public class Standing
    {
        private StandingID standingID;
        private LeagueID leagueId;
        private TeamID teamID;
        private Points points;
        private MatchesPlayed matchesPlayed;
        private Wins wins;
        private Draws draws;
        private Losses losses;
        private GoalDifference goalDifference;
        private DateTime utcNow;

        public StandingID StandingID { get; private set; }
        public LeagueID LeagueID { get; private set; }
        public TeamID TeamID { get; private set; }
        public Points Points { get; private set; }
        public MatchesPlayed MatchesPlayed { get; private set; }
        public Wins Wins { get; private set; }
        public Draws Draws { get; private set; }
        public Losses Losses { get; private set; }
        public GoalDifference GoalDifference { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public League League { get; private set; }
        public Team Team { get; private set; }

        public Standing(
            StandingID standingID,
            LeagueID leagueID,
            TeamID teamID,
            Points points,
            MatchesPlayed matchesPlayed,
            Wins wins,
            Draws draws,
            Losses losses,
            GoalDifference goalDifference,
            League league,
            Team team,
            DateTime? createdAt = null)
        {
            StandingID = standingID ?? throw new ArgumentNullException(nameof(standingID));
            LeagueID = leagueID ?? throw new ArgumentNullException(nameof(leagueID));
            TeamID = teamID ?? throw new ArgumentNullException(nameof(teamID));
            Points = points ?? throw new ArgumentNullException(nameof(points));
            MatchesPlayed = matchesPlayed ?? throw new ArgumentNullException(nameof(matchesPlayed));
            Wins = wins ?? throw new ArgumentNullException(nameof(wins));
            Draws = draws ?? throw new ArgumentNullException(nameof(draws));
            Losses = losses ?? throw new ArgumentNullException(nameof(losses));
            GoalDifference = goalDifference ?? throw new ArgumentNullException(nameof(goalDifference));
            League = league ?? throw new ArgumentNullException(nameof(league));
            Team = team ?? throw new ArgumentNullException(nameof(team));
            CreatedAt = createdAt == null || createdAt == default
                             ? DateTime.UtcNow
                             : createdAt.Value;
        }
        protected Standing() { }

        public Standing(StandingID standingID, LeagueID leagueId, TeamID teamID, Points points, MatchesPlayed matchesPlayed, Wins wins, Draws draws, Losses losses, GoalDifference goalDifference, DateTime utcNow)
        {
            this.standingID = standingID;
            this.leagueId = leagueId;
            this.teamID = teamID;
            this.points = points;
            this.matchesPlayed = matchesPlayed;
            this.wins = wins;
            this.draws = draws;
            this.losses = losses;
            this.goalDifference = goalDifference;
            this.utcNow = utcNow;
        }

        public void Update(
            Points points,
            MatchesPlayed matchesPlayed,
            Wins wins,
            Draws draws,
            Losses losses,
            GoalDifference goalDifference)
        {
            Points = points ?? Points;
            MatchesPlayed = matchesPlayed ?? MatchesPlayed;
            Wins = wins ?? Wins;
            Draws = draws ?? Draws;
            Losses = losses ?? Losses;
            GoalDifference = goalDifference ?? GoalDifference;
        }


        public void ApplyResult(int scored, int conceded)
        {
            MatchesPlayed = new MatchesPlayed(MatchesPlayed.Value + 1);
            Points = new Points(Points.Value + (scored > conceded ? 3 : (scored == conceded ? 1 : 0)));
            Wins = new Wins(Wins.Value + (scored > conceded ? 1 : 0));
            Draws = new Draws(Draws.Value + (scored == conceded ? 1 : 0));
            Losses = new Losses(Losses.Value + (scored < conceded ? 1 : 0));
            GoalDifference = new GoalDifference(GoalDifference.Value + (scored - conceded));
        }

        public void UpdatePoints(Points points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            Points = points;
        }

        public void UpdateMatchesPlayed(MatchesPlayed matchesPlayed)
        {
            if (matchesPlayed == null)
                throw new ArgumentNullException(nameof(matchesPlayed));
            MatchesPlayed = matchesPlayed;
        }

        public void UpdateWins(Wins wins)
        {
            if (wins == null)
                throw new ArgumentNullException(nameof(wins));
            Wins = wins;
        }

        public void UpdateDraws(Draws draws)
        {
            if (draws == null)
                throw new ArgumentNullException(nameof(draws));
            Draws = draws;
        }

        public void UpdateLosses(Losses losses)
        {
            if (losses == null)
                throw new ArgumentNullException(nameof(losses));
            Losses = losses;
        }

        public void UpdateGoalDifference(GoalDifference goalDifference)
        {
            if (goalDifference == null)
                throw new ArgumentNullException(nameof(goalDifference));
            GoalDifference = goalDifference;
        }

    }
}
