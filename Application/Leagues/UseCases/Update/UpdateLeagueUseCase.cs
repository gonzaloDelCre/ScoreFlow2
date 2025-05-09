using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Update
{
    public class UpdateLeagueUseCase
    {
        private readonly ILeagueRepository _repo;

        public UpdateLeagueUseCase(ILeagueRepository repo) => _repo = repo;

        public async Task<LeagueResponseDTO?> ExecuteAsync(LeagueRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar una liga");

            var existing = await _repo.GetByIdAsync(new LeagueID(dto.ID.Value));
            if (existing == null) return null;

            existing.Update(
                name: new LeagueName(dto.Name),
                description: dto.Description!,
                createdAt: dto.CreatedAt
            );

            await _repo.UpdateAsync(existing);
            return existing.ToDTO();
        }
    }
}
