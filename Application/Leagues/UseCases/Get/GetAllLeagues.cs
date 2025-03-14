using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;

namespace Application.Leagues.UseCases.Get
{
    public class GetAllLeagues
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly LeagueMapper _mapper;

        public GetAllLeagues(ILeagueRepository leagueRepository, LeagueMapper mapper)
        {
            _leagueRepository = leagueRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LeagueResponseDTO>> Execute()
        {
            var leagues = await _leagueRepository.GetAllAsync();
            return leagues.Select(league => _mapper.MapToDTO(league));
        }
    }
}
