using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Entities;
using System.Linq;

namespace Infrastructure.Persistence.Players.Mapper
{
    public class PlayerMapper : IPlayerMapper
    {
        public PlayerEntity ToEntity(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            return new PlayerEntity
            {
                PlayerID = player.PlayerID.Value,
                Name = player.Name.Value,
                Position = player.Position.ToString(),
                Age = player.Age.Value,
                Goals = player.Goals,
                Photo = player.Photo,
                CreatedAt = player.CreatedAt
            };
        }

        public Player ToDomain(
            PlayerEntity entity,
            IEnumerable<TeamPlayerEntity> teamPlayers)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var domainTPs = teamPlayers
                .Where(tp => tp.PlayerID == entity.PlayerID)
                .Select(tp => new TeamPlayer(
                    new TeamID(tp.TeamID),
                    new PlayerID(tp.PlayerID),
                    tp.JoinedAt,
                    tp.RoleInTeam))
                .ToList();

            return new Player(
                new PlayerID(entity.PlayerID),
                new PlayerName(entity.Name),
                Enum.Parse<PlayerPosition>(entity.Position),
                new PlayerAge(entity.Age),
                entity.Goals,
                entity.Photo,
                entity.CreatedAt,
                domainTPs);
        }
    }
}
