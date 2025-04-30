using Domain.Entities.Leagues;
using Domain.Entities.Standings;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Shared;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Standings.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.Teams.Mapper;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Standings.Mapper
{
    public static class StandingMapper
    {
        public static Standing ToDomain(this StandingEntity entity, League league, ICollection teamPlayers, ICollection playerEntities)
        {
            return new Standing(
            standingID: new StandingID(entity.StandingID),
            leagueID: new LeagueID(entity.LeagueID),
            teamID: new TeamID(entity.TeamID),
            wins: new Wins(entity.Wins),
            draws: new Draws(entity.Draws),
            losses: new Losses(entity.Losses),
            goalsFor: new GoalsFor(entity.GoalsFor),
            goalsAgainst: new GoalsAgainst(entity.GoalsAgainst),
            points: new Points(entity.Points),
            team: entity.Team?.MapToDomain(league, teamPlayers, playerEntities),
            createdAt: entity.CreatedAt
            );
        }

        public static StandingEntity ToEntity(this Standing domain)
        {
            return new StandingEntity
            {
                StandingID = domain.StandingID.Value,
                LeagueID = domain.LeagueID.Value,
                TeamID = domain.TeamID.Value,
                Wins = domain.Wins.Value,
                Draws = domain.Draws.Value,
                Losses = domain.Losses.Value,
                GoalsFor = domain.GoalsFor.Value,
                GoalsAgainst = domain.GoalsAgainst.Value,
                Points = domain.Points.Value,
                CreatedAt = domain.CreatedAt
            };
        }

    }
}
