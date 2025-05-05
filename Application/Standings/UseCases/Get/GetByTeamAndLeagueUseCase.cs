using Application.Standings.DTOs;
using Application.Standings.Mapper;
using Domain.Ports.Standings;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetByTeamAndLeagueUseCase
    {
        private readonly IStandingRepository _repo;
        private readonly StandingMapper _mapper;

        public GetByTeamAndLeagueUseCase(IStandingRepository repo, StandingMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<StandingResponseDTO> ExecuteAsync(int teamId, int leagueId)
        {
            var s = await _repo.GetByTeamIdAndLeagueIdAsync(new TeamID(teamId), new LeagueID(leagueId))
                ?? throw new KeyNotFoundException($"No standing for team {teamId} in league {leagueId}");
            return _mapper.MapToDTO(s);
        }
    }
}
