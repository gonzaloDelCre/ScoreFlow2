using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Entities.Players;
using Domain.Ports.Players;
using Domain.Shared;

namespace Application.Playes.UseCases.Create
{
    public class CreatePlayerUseCase
    {
        private readonly IPlayerRepository _repo;
        public CreatePlayerUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<PlayerResponseDTO> ExecuteAsync(PlayerRequestDTO dto)
        {
            var player = dto.ToDomain();
            var created = await _repo.AddAsync(player);
            return created.ToDTO();
        }
    }
}
