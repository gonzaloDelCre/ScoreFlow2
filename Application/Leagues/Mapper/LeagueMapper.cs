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
                throw new ArgumentNullException(nameof(league), "La entidad de dominio League no puede ser nula.");

            return new LeagueResponseDTO
            {
                LeagueID = league.LeagueID.Value,
                Name = league.Name.Value,
                Description = league.Description,
                CreatedAt = league.CreatedAt
            };
        }

        public League MapToDomain(LeagueRequestDTO leagueDTO)
        {
            if (leagueDTO == null)
                throw new ArgumentNullException(nameof(leagueDTO), "El DTO LeagueRequestDTO no puede ser nulo.");

            var leagueID = new LeagueID(1);

            return new League(
                leagueID,
                new LeagueName(leagueDTO.Name),
                leagueDTO.Description,
                leagueDTO.CreatedAt
            );
        }
    }
}
