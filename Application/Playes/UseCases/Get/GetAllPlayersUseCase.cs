using System.Linq;
using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Ports.Players;
using Domain.Shared;

namespace Application.Playes.UseCases.Get
{
    public class GetAllPlayersUseCase
    {
        private readonly IPlayerRepository _repo;
        public GetAllPlayersUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<List<PlayerResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(p => p.ToDTO()).ToList();
        }
    }
}
