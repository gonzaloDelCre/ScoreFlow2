using Application.Standings.DTOs;
using Application.Standings.Mapper;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Create
{
    public class CreateStandingUseCase
    {
        private readonly IStandingRepository _repo;
        private readonly StandingMapper _mapper;

        public CreateStandingUseCase(IStandingRepository repo, StandingMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<StandingResponseDTO> ExecuteAsync(StandingRequestDTO req)
        {
            var domain = _mapper.MapToDomain(req, existing: null);
            var saved = await _repo.AddAsync(domain);
            return _mapper.MapToDTO(saved);
        }
    }
}
