using System;
using Domain.Entities.Teams;
using Domain.Entities.Leagues;
using Domain.Shared;

namespace Domain.Entities.Standings
{
    public class Standing
    {
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

        // Constructor
        public Standing(
            StandingID standingID = null,
            LeagueID leagueID = null,
            TeamID teamID = null,
            Points points = null,
            MatchesPlayed matchesPlayed = null,
            Wins wins = null,
            Draws draws = null,
            Losses losses = null,
            GoalDifference goalDifference = null,
            League league = null,
            Team team = null,
            DateTime? createdAt = null)
        {
            StandingID = standingID ?? new StandingID(0);  
            LeagueID = leagueID ?? new LeagueID(0);     
            TeamID = teamID ?? new TeamID(0);            
            Points = points ?? new Points(0);           
            MatchesPlayed = matchesPlayed ?? new MatchesPlayed(0);  
            Wins = wins ?? new Wins(0);  
            Draws = draws ?? new Draws(0);  
            Losses = losses ?? new Losses(0);  
            GoalDifference = goalDifference ?? new GoalDifference(0);  
            League = league ?? new League(); 
            Team = team ?? new Team();  
            CreatedAt = createdAt ?? DateTime.UtcNow; 
        }

        // Update Method
        public void Update(Points points, MatchesPlayed matchesPlayed, Wins wins, Draws draws, Losses losses, GoalDifference goalDifference)
        {
            Points = points ?? Points;
            MatchesPlayed = matchesPlayed ?? MatchesPlayed;
            Wins = wins ?? Wins;
            Draws = draws ?? Draws;
            Losses = losses ?? Losses;
            GoalDifference = goalDifference ?? GoalDifference;
        }

        public void UpdatePoints(Points points)
        {
            Points = points ?? Points;
        }

        public void UpdateMatchesPlayed(MatchesPlayed matchesPlayed)
        {
            MatchesPlayed = matchesPlayed ?? MatchesPlayed;
        }

        public void UpdateWins(Wins wins)
        {
            Wins = wins ?? Wins;
        }

        public void UpdateDraws(Draws draws)
        {
            Draws = draws ?? Draws;
        }

        public void UpdateLosses(Losses losses)
        {
            Losses = losses ?? Losses;
        }

        public void UpdateGoalDifference(GoalDifference goalDifference)
        {
            GoalDifference = goalDifference ?? GoalDifference;
        }
    }
}
