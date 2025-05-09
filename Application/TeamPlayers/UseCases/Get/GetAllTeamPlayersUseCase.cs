using Application.TeamPlayers.DTOs;
using Domain.Shared;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Ports.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetAllTeamPlayersUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public GetAllTeamPlayersUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<List<TeamPlayerResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(tp => tp.ToDTO()).ToList();
        }
    }
}
