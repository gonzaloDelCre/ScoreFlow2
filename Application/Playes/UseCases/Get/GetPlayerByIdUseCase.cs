using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Ports.Players;
using Domain.Shared;

namespace Application.Playes.UseCases.Get
{
    public class GetPlayerByIdUseCase
    {
        private readonly IPlayerRepository _repo;
        public GetPlayerByIdUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<PlayerResponseDTO?> ExecuteAsync(int id)
        {
            var p = await _repo.GetByIdAsync(new PlayerID(id));
            return p is null ? null : p.ToDTO();
        }
    }
}
