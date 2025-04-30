using Application.Leagues.DTOs;
using Domain.Entities.Leagues;
using Domain.Shared;
using System;

namespace Application.Leagues.Mapper
{
    public class LeagueMapper
    {
        public LeagueResponseDTO MapToDTO(League league)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league));

            return new LeagueResponseDTO
            {
                LeagueID = league.LeagueID.Value,
                Name = league.Name.Value,
                Description = league.Description,
                CreatedAt = league.CreatedAt
            };
        }

        /// <summary>
        /// Si existingLeague != null, actualiza esa instancia; si es null, crea una nueva con ID=0.
        /// </summary>
        public League MapToDomain(LeagueRequestDTO dto, League? existingLeague = null)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (existingLeague != null)
            {
                existingLeague.Update(
                    new LeagueName(dto.Name),
                    dto.Description,
                    dto.CreatedAt
                );
                return existingLeague;
            }

            return new League(
                new LeagueID(0),
                new LeagueName(dto.Name),
                dto.Description,
                dto.CreatedAt
            );
        }
    }
}
