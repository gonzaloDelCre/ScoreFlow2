using Application.Standings.DTOs;
using Application.Standings.Mapper;
using Domain.Entities.Standings;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetStandingByIdUseCase
    {
        private readonly IStandingRepository _repo;
        private readonly StandingMapper _mapper;

        public GetStandingByIdUseCase(IStandingRepository repo, StandingMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<StandingResponseDTO> ExecuteAsync(int id)
        {
            var maybe = await _repo.GetByIdAsync(new StandingID(id));
            if (maybe == null) throw new KeyNotFoundException($"Standing {id} no existe");
            return _mapper.MapToDTO(maybe);
        }
    }
}
