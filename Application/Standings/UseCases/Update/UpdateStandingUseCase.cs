using Application.Standings.DTOs;
using Application.Standings.Mapper;
using Domain.Ports.Standings;
using Domain.Entities.Standings;
using Domain.Shared;
using System.Collections.Generic;
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

            var updatedDomain = _mapper.MapToDomain(req, existing);
            await _repo.UpdateAsync(updatedDomain);
            return _mapper.MapToDTO(updatedDomain);
        }
    }
}
