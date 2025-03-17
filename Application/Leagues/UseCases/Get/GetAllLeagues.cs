using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Get
{
    public class GetAllLeagues
    {
        private readonly LeagueService _leagueService;
        private readonly LeagueMapper _mapper;

        public GetAllLeagues(LeagueService leagueService, LeagueMapper mapper)
        {
            _leagueService = leagueService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LeagueResponseDTO>> ExecuteAsync()
        {
            var leagues = await _leagueService.GetAllLeaguesAsync();
            return leagues.Select(league => _mapper.MapToDTO(league));
        }
    }
}
