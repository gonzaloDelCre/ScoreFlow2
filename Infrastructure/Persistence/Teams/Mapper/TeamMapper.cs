using System.Linq;
using Domain.Entities.Teams;
using Domain.Entities.Players;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Domain.Shared;
using Infrastructure.Persistence.Players.Entities;
using Domain.Entities.Standings;
using Infrastructure.Persistence.Players.Mapper;

namespace Infrastructure.Persistence.Teams.Mapper
{
    public class TeamMapper : ITeamMapper
    {
        public TeamEntity ToEntity(Team domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new TeamEntity
            {
                TeamID = domain.TeamID.Value,
                ExternalID = domain.ExternalID,
                Name = domain.Name.Value,
                Category = domain.Category,
                Club = domain.Club,
                Stadium = domain.Stadium,
                Logo = domain.Logo.Value,
                CoachPlayerID = domain.Coach?.PlayerID.Value,
                CreatedAt = domain.CreatedAt
            };
        }

        public Team ToDomain(
            TeamEntity entity,
            IEnumerable<Player> players,
            IEnumerable<Standing> standings)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var team = new Team(
                new TeamID(entity.TeamID),
                new TeamName(entity.Name),
                new LogoUrl(entity.Logo),
                entity.CreatedAt,
                entity.Category,
                entity.Club,
                entity.Stadium,
                entity.ExternalID);

            if (entity.Coach != null)
            {
                var coach = new PlayerMapper().ToDomain(entity.Coach, Enumerable.Empty<TeamPlayerEntity>());
                team.AssignCoach(coach);
            }

            foreach (var p in players) team.AddPlayer(p);
            foreach (var s in standings) team.AddStanding(s);

            return team;
        }
    }
}
