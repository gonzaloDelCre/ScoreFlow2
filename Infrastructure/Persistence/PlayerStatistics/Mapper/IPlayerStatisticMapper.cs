using Infrastructure.Persistence.PlayerStatistics.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.PlayerStatistics.Mapper
{
    public interface IPlayerStatisticMapper
    {
        PlayerStatisticEntity MapToEntity(Domain.Entities.PlayerStatistics.PlayerStatistic stat);
        Domain.Entities.PlayerStatistics.PlayerStatistic MapToDomain(
            PlayerStatisticEntity entity,
            Domain.Entities.Players.Player player,
            Domain.Entities.Matches.Match match
        );
    }
}
