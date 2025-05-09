using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Domain.Ports.Standings;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetClassificationByLeagueIdUseCase
    {
        private readonly IStandingRepository _repo;
        public GetClassificationByLeagueIdUseCase(IStandingRepository repo) => _repo = repo;

        public async Task<List<StandingResponseDTO>> ExecuteAsync(int leagueId)
        {
            var list = await _repo.GetClassificationByLeagueIdAsync(new LeagueID(leagueId));
            return list.Select(s => s.ToDTO()).ToList();
        }
    }
}
