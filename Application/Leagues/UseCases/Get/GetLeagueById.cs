using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Get
{
    public class GetLeagueById
    {
        private readonly LeagueService _leagueService;
        private readonly LeagueMapper _mapper;

        public GetLeagueById(LeagueService leagueService, LeagueMapper mapper)
        {
            _leagueService = leagueService;
            _mapper = mapper;
        }

        public async Task<LeagueResponseDTO?> ExecuteAsync(LeagueID leagueId)
        {
            var league = await _leagueService.GetLeagueByIdAsync(leagueId);
            return league != null ? _mapper.MapToDTO(league) : null;
        }
    }
}
