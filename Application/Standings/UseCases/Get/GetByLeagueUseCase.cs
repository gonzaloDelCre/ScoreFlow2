using Application.Standings.DTOs;
using Application.Standings.Mapper;
using Domain.Ports.Standings;
using Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetByLeagueUseCase
    {
        private readonly IStandingRepository _repo;
        private readonly StandingMapper _mapper;

        public GetByLeagueUseCase(IStandingRepository repo, StandingMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<StandingResponseDTO>> ExecuteAsync(int leagueId)
        {
            var list = await _repo.GetByLeagueIdAsync(new LeagueID(leagueId));
            return list.Select(s => _mapper.MapToDTO(s)).ToList();
        }
    }
}
