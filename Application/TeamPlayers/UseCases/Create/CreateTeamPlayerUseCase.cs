using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Ports.Teams;
using Domain.Shared;
using Domain.Ports.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Create
{
    public class CreateTeamPlayerUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public CreateTeamPlayerUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<TeamPlayerResponseDTO> ExecuteAsync(TeamPlayerRequestDTO dto)
        {
            var tp = dto.ToDomain();
            var created = await _repo.AddAsync(tp);
            return created.ToDTO();
        }
    }
}