using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Get
{
    public class GetAllLeaguesUseCase
    {
        private readonly LeagueService _service;
        private readonly LeagueMapper _mapper;

        public GetAllLeaguesUseCase(LeagueService service, LeagueMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<List<LeagueResponseDTO>> ExecuteAsync()
        {
            var all = await _service.GetAllLeaguesAsync();
            return all.Select(l => _mapper.MapToDTO(l)).ToList();
        }
    }
}
