using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Domain.Entities.Standings;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Update
{
    public class UpdateStandingUseCase
    {
        private readonly IStandingRepository _repo;
        public UpdateStandingUseCase(IStandingRepository repo) => _repo = repo;

        public async Task<StandingResponseDTO?> ExecuteAsync(StandingRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar una clasificación");

            var existing = await _repo.GetByIdAsync(new StandingID(dto.ID.Value));
            if (existing == null) return null;

            existing.Update(
                points: new Points(dto.Points),
                matchesPlayed: new MatchesPlayed(dto.MatchesPlayed),
                wins: new Wins(dto.Wins),
                draws: new Draws(dto.Draws),
                losses: new Losses(dto.Losses),
                goalDifference: new GoalDifference(dto.GoalDifference)
            );

            await _repo.UpdateAsync(existing);
            return existing.ToDTO();
        }
    }
}
