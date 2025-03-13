using Domain.Entities.Leagues;
using Domain.Entities.Standings;
using Domain.Entities.Teams;
using Infrastructure.Persistence.Standings.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Standings.Mapper
{
    public class StandingMapper
    {
        public StandingEntity MapToEntity(Standing standing)
        {
            return new StandingEntity
            {
                StandingID = standing.StandingID,
                LeagueID = standing.League.LeagueID,
                TeamID = standing.Team.TeamID,
                Points = standing.Points,
                Wins = standing.Wins,
                Losses = standing.Losses,
                Draws = standing.Draws,
                GoalDifference = standing.GoalDifference,
                CreatedAt = standing.CreatedAt
            };
        }

        public Standing MapToDomain(StandingEntity entity, League league, Team team)
        {
            return new Standing(
                entity.StandingID,
                league,
                team,
                entity.Points,
                entity.Wins,
                entity.Losses,
                entity.Draws,
                entity.GoalDifference,
                entity.CreatedAt
            );
        }
    }
}
