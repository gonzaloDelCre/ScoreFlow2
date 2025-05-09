using Application.Leagues.DTOs;
using Domain.Entities.Leagues;
using Domain.Shared;
using System;

namespace Application.Leagues.Mapper
{
    public static class LeagueMapper
    {
        public static LeagueResponseDTO ToDTO(this League l)
            => new LeagueResponseDTO
            {
                ID = l.LeagueID.Value,
                Name = l.Name.Value,
                Description = l.Description,
                CreatedAt = l.CreatedAt
            };

        public static League ToDomain(this LeagueRequestDTO dto)
            => new League(
                leagueID: new LeagueID(1),  
                name: new LeagueName(dto.Name),
                description: dto.Description,
                createdAt: DateTime.UtcNow
            );
    }
}
