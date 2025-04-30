using Application.Standings.DTOs;
using Application.Standings.Mapper;
using Domain.Entities.Standings;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Update
{
    public class UpdateStandingUseCase
    {
        private readonly IStandingRepository _repo;
        private readonly StandingMapper _mapper;

        public UpdateStandingUseCase(IStandingRepository repo, StandingMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<StandingResponseDTO> ExecuteAsync(StandingRequestDTO req)
        {
            var existing = await _repo.GetByIdAsync(new StandingID(req.StandingID))
                           ?? throw new KeyNotFoundException($"Standing {req.StandingID} no existe");
            var domain = _mapper.MapToDomain(req, existing);
            await _repo.UpdateAsync(domain);
            return _mapper.MapToDTO(domain);
        }
    }
}
 