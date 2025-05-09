using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Ports.Matches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Get
{
    public class GetMatchesByLeagueUseCase
    {
        private readonly IMatchRepository _repo;

        public GetMatchesByLeagueUseCase(IMatchRepository repo) => _repo = repo;

        public async Task<List<MatchResponseDTO>> ExecuteAsync(int leagueId)
        {
            var list = await _repo.GetByLeagueIdAsync(leagueId);
            return list.Select(m => m.ToDTO()).ToList();
        }
    }
}
