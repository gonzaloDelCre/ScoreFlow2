using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Shared;

namespace Application.Leagues.UseCases.Create
{
    public class CreateLeagueUseCase
    {
        private readonly ILeagueRepository _repo;

        public CreateLeagueUseCase(ILeagueRepository repo) => _repo = repo;

        public async Task<LeagueResponseDTO> ExecuteAsync(LeagueRequestDTO dto)
        {
            var domain = dto.ToDomain();
            var created = await _repo.AddAsync(domain);
            return created.ToDTO();
        }
    }
}
