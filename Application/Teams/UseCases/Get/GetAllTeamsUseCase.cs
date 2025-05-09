using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Ports.Teams;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class GetAllTeamsUseCase
    {
        private readonly ITeamRepository _repo;
        public GetAllTeamsUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<List<TeamResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(t => t.ToDTO()).ToList();
        }
    }
}
