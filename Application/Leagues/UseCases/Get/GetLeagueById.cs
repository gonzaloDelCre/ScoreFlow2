using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using Domain.Shared;

namespace Application.Leagues.UseCases.Get
{
    public class GetLeagueById
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly LeagueMapper _mapper;

        public GetLeagueById(ILeagueRepository leagueRepository, LeagueMapper mapper)
        {
            _leagueRepository = leagueRepository;
            _mapper = mapper;
        }

        public async Task<LeagueResponseDTO?> Execute(LeagueID leagueId)
        {
            if (leagueId == null)
                throw new ArgumentException("El ID de la liga no puede ser nulo.");

            var league = await _leagueRepository.GetByIdAsync(leagueId);
            return league != null ? _mapper.MapToDTO(league) : null;
        }
    }
}
