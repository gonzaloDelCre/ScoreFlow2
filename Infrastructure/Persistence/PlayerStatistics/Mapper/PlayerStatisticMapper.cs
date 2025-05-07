using Domain.Entities.Matches;
using Domain.Entities.Players;
using Domain.Entities.PlayerStatistics;
using Domain.Shared;
using Infrastructure.Persistence.PlayerStatistics.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.PlayerStatistics.Mapper
{
    public class PlayerStatisticMapper : IPlayerStatisticMapper
    {
        public PlayerStatisticEntity MapToEntity(PlayerStatistic domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new PlayerStatisticEntity
            {
                ID = domain.PlayerStatisticID.Value,
                PlayerID = domain.PlayerID.Value,
                MatchID = domain.MatchID.Value,
                Goals = domain.Goals.Value,
                Assists = domain.Assists.Value,
                YellowCards = domain.YellowCards.Value,
                RedCards = domain.RedCards.Value,
                MinutesPlayed = domain.MinutesPlayed,
                CreatedAt = domain.CreatedAt
            };
        }

        public PlayerStatistic MapToDomain(
            PlayerStatisticEntity entity,
            Player player,
            Match match)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (match == null) throw new ArgumentNullException(nameof(match));

            return new PlayerStatistic(
                new PlayerStatisticID(entity.ID),
                new MatchID(entity.MatchID),
                new PlayerID(entity.PlayerID),
                new Goals(entity.Goals),
                new Assists(entity.Assists),
                new YellowCards(entity.YellowCards),
                new RedCards(entity.RedCards),
                match,
                player,
                entity.CreatedAt,
                entity.MinutesPlayed
            );
        }
    }
}
