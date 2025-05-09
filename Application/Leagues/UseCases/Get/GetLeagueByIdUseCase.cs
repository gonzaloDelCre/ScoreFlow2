using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using Domain.Shared;

namespace Application.Leagues.UseCases.Get
{
    public class GetLeagueByIdUseCase
    {
        private readonly ILeagueRepository _repo;

        public GetLeagueByIdUseCase(ILeagueRepository repo) => _repo = repo;

        public async Task<LeagueResponseDTO?> ExecuteAsync(int id)
        {
            var l = await _repo.GetByIdAsync(new LeagueID(id));
            return l is null ? null : l.ToDTO();
        }
    }
}
