using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Infrastructure.Persistence.Matches.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Matches.Mapper
{
    public class MatchMapper
    {
        public MatchEntity MapToEntity(Match match)
        {
            return new MatchEntity
            {
                MatchID = match.MatchID,
                Team1ID = match.Team1.TeamID,
                Team2ID = match.Team2.TeamID,
                DateTime = match.DateTime,
                ScoreTeam1 = match.ScoreTeam1,
                ScoreTeam2 = match.ScoreTeam2,
                Status = match.Status,
                Location = match.Location,
                CreatedAt = match.CreatedAt
            };
        }

        public Match MapToDomain(MatchEntity entity, Team team1, Team team2)
        {
            return new Match(
                entity.MatchID,
                team1,
                team2,
                entity.DateTime,
                entity.ScoreTeam1,
                entity.ScoreTeam2,
                entity.Status,
                entity.Location,
                entity.CreatedAt
            );
        }
    }
}
