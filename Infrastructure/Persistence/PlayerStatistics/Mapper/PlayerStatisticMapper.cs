//using Domain.Entities.Matches;
//using Domain.Entities.Players;
//using Domain.Entities.PlayerStatistics;
//using Infrastructure.Persistence.PlayerStatistics.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Persistence.PlayerStatistics.Mapper
//{
//    public class PlayerStatisticMapper
//    {
//        public PlayerStatisticEntity MapToEntity(PlayerStatistic stat)
//        {
//            return new PlayerStatisticEntity
//            {
//                StatID = stat.StatID,
//                PlayerID = stat.Player.PlayerID,
//                MatchID = stat.Match.MatchID,
//                Goals = stat.Goals,
//                Assists = stat.Assists,
//                YellowCards = stat.YellowCards,
//                RedCards = stat.RedCards,
//                MinutesPlayed = stat.MinutesPlayed,
//                CreatedAt = stat.CreatedAt
//            };
//        }

//        public PlayerStatistic MapToDomain(PlayerStatisticEntity entity, Player player, Match match)
//        {
//            return new PlayerStatistic(
//                entity.StatID,
//                player,
//                match,
//                entity.Goals,
//                entity.Assists,
//                entity.YellowCards,
//                entity.RedCards,
//                entity.MinutesPlayed,
//                entity.CreatedAt
//            );
//        }
//    }
//}
