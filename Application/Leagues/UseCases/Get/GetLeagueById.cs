using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Get
{
    public class GetLeagueByIdUseCase
    {
        private readonly LeagueService _service;
        private readonly LeagueMapper _mapper;

        public GetLeagueByIdUseCase(LeagueService service, LeagueMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<LeagueResponseDTO?> ExecuteAsync(int id)
        {
            var league = await _service.GetLeagueByIdAsync(new LeagueID(id));
            return league is null
                ? null
                : _mapper.MapToDTO(league);
        }
    }
}
