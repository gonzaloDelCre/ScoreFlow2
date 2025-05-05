using Application.Standings.DTOs;
using Application.Standings.Mapper;
using Domain.Ports.Standings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetAllStandingsUseCase
    {
        private readonly IStandingRepository _repo;
        private readonly StandingMapper _mapper;

        public GetAllStandingsUseCase(IStandingRepository repo, StandingMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<StandingResponseDTO>> ExecuteAsync()
        {
            var all = await _repo.GetAllAsync();
            return all.Select(s => _mapper.MapToDTO(s)).ToList();
        }
    }
}
