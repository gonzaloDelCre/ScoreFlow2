using Domain.Entities.Leagues;
using Domain.Entities.Standings;
using Domain.Entities.Teams;
using Infrastructure.Persistence.Standings.Entities;
using Domain.Shared;

namespace Infrastructure.Persistence.Standings.Mapper
{
    public class StandingMapper
    {
        // Convierte la entidad de dominio Standing en la entidad de base de datos StandingEntity.
        public StandingEntity MapToEntity(Standing standing)
        {
            if (standing == null)
            {
                throw new ArgumentNullException(nameof(standing), "Standing cannot be null.");
            }

            return new StandingEntity
            {
                LeagueID = standing.LeagueID.Value,
                TeamID = standing.TeamID.Value,
                Points = standing.Points.Value,
                Wins = standing.Wins.Value,
                Losses = standing.Losses.Value,
                Draws = standing.Draws.Value,
                GoalDifference = standing.GoalDifference.Value,
                CreatedAt = standing.CreatedAt
            };
        }

        // Convierte la entidad de base de datos StandingEntity en la entidad de dominio Standing.
        // Se reciben además las entidades de dominio League y Team correspondientes.
        public Standing MapToDomain(StandingEntity entity, League league, Team team)
        {
            return new Standing(
                new StandingID(entity.StandingID),
                new LeagueID(entity.LeagueID),
                new TeamID(entity.TeamID),
                new Points(entity.Points),
                new MatchesPlayed(entity.Wins + entity.Losses + entity.Draws), // Se calcula a partir de wins, losses y draws
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
