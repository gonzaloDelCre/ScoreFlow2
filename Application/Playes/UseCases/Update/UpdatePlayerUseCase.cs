using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Entities.Players;
using Domain.Ports.Players;
using Domain.Shared;

namespace Application.Playes.UseCases.Update
{
    public class UpdatePlayerUseCase
    {
        private readonly IPlayerRepository _repo;
        public UpdatePlayerUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<PlayerResponseDTO?> ExecuteAsync(PlayerRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar un jugador");

            var existing = await _repo.GetByIdAsync(new PlayerID(dto.ID.Value));
            if (existing == null) return null;

            existing.Update(
                name: new PlayerName(dto.Name),
                position: dto.Position,
                age: new PlayerAge(dto.Age),
                goals: dto.Goals,
                photo: dto.Photo,
                createdAt: existing.CreatedAt
            );

            await _repo.UpdateAsync(existing);
            return existing.ToDTO();
        }
    }
}
