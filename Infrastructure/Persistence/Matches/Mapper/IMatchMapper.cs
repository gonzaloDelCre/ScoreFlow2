using Domain.Entities.Matches;
using Infrastructure.Persistence.Matches.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Matches.Mapper
{
    public interface IMatchMapper
    {
        MatchEntity ToEntity(Match domain);
        Match ToDomain(MatchEntity entity);
    }
}
