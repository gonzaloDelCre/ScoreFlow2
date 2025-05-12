using Domain.Entities.Leagues;
using Domain.Entities.Standings;
using Domain.Entities.Teams;
using Infrastructure.Persistence.Standings.Entities;
using Domain.Shared;

namespace Infrastructure.Persistence.Standings.Mapper
{
    public class StandingMapper : IStandingMapper
    {
        public StandingEntity MapToEntity(Standing domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new StandingEntity
            {
                ID = domain.StandingID?.Value ?? 0,
                LeagueID = domain.LeagueID.Value,
                TeamID = domain.TeamID.Value,
                Points = domain.Points.Value,
                Wins = domain.Wins.Value,
                Draws = domain.Draws.Value,
                Losses = domain.Losses.Value,
                GoalDifference = domain.GoalDifference.Value,
                CreatedAt = domain.CreatedAt
            };
        }

        public Standing MapToDomain(
            StandingEntity entity,
            League league,
            Team team)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (league == null) throw new ArgumentNullException(nameof(league));
            if (team == null) throw new ArgumentNullException(nameof(team));

            var matchesPlayed = entity.Wins + entity.Draws + entity.Losses;

            return new Standing(
                new StandingID(entity.ID),
                new LeagueID(entity.LeagueID),
                new TeamID(entity.TeamID),
                new Points(entity.Points),
                new MatchesPlayed(matchesPlayed),
                new Wins(entity.Wins),
                new Draws(entity.Draws),
                new Losses(entity.Losses),
                new GoalDifference(entity.GoalDifference),
                league,
                team,
                entity.CreatedAt
            );
        }
    }
}
