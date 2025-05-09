using Application.Teams.DTOs;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Application.Teams.Mapper
{
    public static class TeamMapper
    {
        public static TeamResponseDTO ToDTO(this Team t)
            => new TeamResponseDTO
            {
                ID = t.TeamID.Value,
                ExternalID = t.ExternalID,
                Name = t.Name.Value,
                LogoUrl = t.Logo.Value,
                Category = t.Category,
                Club = t.Club,
                Stadium = t.Stadium,
                CoachPlayerID = t.Coach?.PlayerID.Value,
                CoachName = t.Coach?.Name.Value,
                CreatedAt = t.CreatedAt
            };

        public static Team ToDomain(this TeamRequestDTO dto)
            => new Team(
                teamID: new TeamID(dto.ID ?? 0),
                name: new TeamName(dto.Name),
                logo: new LogoUrl(dto.LogoUrl),
                createdAt: default,
                category: dto.Category,
                club: dto.Club,
                stadium: dto.Stadium,
                externalID: dto.ExternalID,
                coach: null!
            );
    }
}
