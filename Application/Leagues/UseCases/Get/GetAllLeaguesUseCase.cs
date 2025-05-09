using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using Domain.Shared;

namespace Application.Leagues.UseCases.Get
{
    public class GetAllLeaguesUseCase
    {
        private readonly ILeagueRepository _repo;

        public GetAllLeaguesUseCase(ILeagueRepository repo) => _repo = repo;

        public async Task<List<LeagueResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(l => l.ToDTO()).ToList();
        }
    }
}
