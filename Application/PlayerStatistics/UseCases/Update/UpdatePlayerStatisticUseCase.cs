using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.Mappers;
using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Update
{
    public class UpdatePlayerStatisticUseCase
    {
        private readonly IPlayerStatisticRepository _repo;
        public UpdatePlayerStatisticUseCase(IPlayerStatisticRepository repo) => _repo = repo;

        public async Task<PlayerStatisticResponseDTO?> ExecuteAsync(PlayerStatisticRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar estadística de jugador");

            var existing = await _repo.GetByIdAsync(new PlayerStatisticID(dto.ID.Value));
            if (existing == null) return null;

            existing.Update(
                goals: new Goals(dto.Goals),
                assists: new Assists(dto.Assists),
                yellowCards: new YellowCards(dto.YellowCards),
                redCards: new RedCards(dto.RedCards),
                minutesPlayed: dto.MinutesPlayed
            );

            await _repo.UpdateAsync(existing);
            return existing.ToDTO();
        }
    }
}
