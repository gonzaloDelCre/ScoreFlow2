using Domain.Entities.Referees;
using Infrastructure.Persistence.Referees.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Referees.Mapper
{
    public class RefereeMapper
    {
        public RefereeEntity MapToEntity(Referee referee)
        {
            return new RefereeEntity
            {
                RefereeID = referee.RefereeID,
                Name = referee.Name
            };
        }

        public Referee MapToDomain(RefereeEntity entity)
        {
            return new Referee(
                entity.RefereeID,
                entity.Name
            );
        }
    }
}
