using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Infrastructure.Persistence.Matches.Entities;
using Infrastructure.Persistence.Teams.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Matches.Mapper
{
    public class MatchMapper
    {
        public static Match ToDomain(MatchEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var team1 = TeamMapper.ToDomain(entity.Team1); // Convertimos Team1 de Entity a Domain
            var team2 = TeamMapper.ToDomain(entity.Team2); // Convertimos Team2 de Entity a Domain

            return new Match(
                new MatchID(entity.MatchID),   // Convertimos MatchID a MatchID
                team1,                         // Asignamos Team1
                team2,                         // Asignamos Team2
                entity.DateTime,               // Fecha del partido
                entity.Status,                  // Asignamos el estado del partido sin conversión
                entity.Location                // Ubicación (si la necesitas en la entidad)
            );
        }



        public static MatchEntity ToEntity(Match domain)
        {
            if (domain == null)
            {
                return null;
            }

            return new MatchEntity
            {
                MatchID = domain.MatchID.Value,   // Convertimos MatchID a int
                Team1ID = domain.Team1.TeamID.Value,  // Convertimos el TeamID de Team1
                Team2ID = domain.Team2.TeamID.Value,  // Convertimos el TeamID de Team2
                DateTime = domain.MatchDate,      // Fecha del partido
                Status = domain.Status,           // Estado del partido
                Location = domain.Location        // Ubicación si la necesitas
            };
        }
    }
}


