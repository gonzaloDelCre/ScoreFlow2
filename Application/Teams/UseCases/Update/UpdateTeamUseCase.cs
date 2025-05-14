using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Update
{
    public class UpdateTeamUseCase
    {
        private readonly ITeamRepository _repo;
        public UpdateTeamUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<TeamResponseDTO?> ExecuteAsync(TeamRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar un equipo");

            var existing = await _repo.GetByIdAsync(new TeamID(dto.ID.Value));
            if (existing == null) return null;

            existing.UpdateInfo(
                name: string.IsNullOrWhiteSpace(dto.Name) ? null : new TeamName(dto.Name),
                logo: string.IsNullOrWhiteSpace(dto.LogoUrl) ? null : new LogoUrl(dto.LogoUrl),
                category: dto.Category,
                club: dto.Club,
                stadium: dto.Stadium
            );
            if (dto.ExternalID != null) existing.SetExternalID(dto.ExternalID);
            if (dto.CoachPlayerID.HasValue)
                existing.AssignCoach(new Domain.Entities.Players.Player(new Domain.Shared.PlayerID(dto.CoachPlayerID.Value), new Domain.Entities.Players.PlayerName(""), Domain.Enum.PlayerPosition.JUGADOR, new Domain.Entities.Players.PlayerAge(0), 0, null, default));

            await _repo.UpdateAsync(existing);
            return existing.ToDTO();
        }
    }
}
