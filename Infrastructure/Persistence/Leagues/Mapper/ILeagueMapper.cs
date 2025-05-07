using Domain.Entities.Leagues;
using Infrastructure.Persistence.Leagues.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Leagues.Mapper
{
    public interface ILeagueMapper
    {
        League MapToDomain(LeagueEntity entity);
        LeagueEntity MapToEntity(League domain);
    }
}
