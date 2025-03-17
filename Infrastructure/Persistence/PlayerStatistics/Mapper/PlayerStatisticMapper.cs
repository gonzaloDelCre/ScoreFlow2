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
    public class PlayerStatisticMapper
    {
        public PlayerStatisticEntity MapToEntity(PlayerStatistic stat)
        {
            return new PlayerStatisticEntity
            {
                StatID = stat.PlayerStatisticID.Value,
                PlayerID = stat.Player.PlayerID.Value,
                MatchID = stat.Match.MatchID.Value,
                Goals = stat.Goals.Value,
                Assists = stat.Assists.Value,
                YellowCards = stat.YellowCards.Value,
                RedCards = stat.RedCards.Value,
                MinutesPlayed = stat.MinutesPlayed,
                CreatedAt = stat.CreatedAt
            };
        }

        public PlayerStatistic MapToDomain(PlayerStatisticEntity entity, Player player, Match match)
        {
            return new PlayerStatistic(
                new PlayerStatisticID(entity.StatID),
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
