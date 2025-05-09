using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Ports.Matches;
using Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Get
{
    public class GetMatchesByTeamUseCase
    {
        private readonly IMatchRepository _repo;

        public GetMatchesByTeamUseCase(IMatchRepository repo) => _repo = repo;

        public async Task<List<MatchResponseDTO>> ExecuteAsync(int teamId)
        {
            var list = await _repo.GetByTeamIdAsync(teamId);
            return list.Select(m => m.ToDTO()).ToList();
        }
    }
}
