using Application.Standings.DTOs;
using Application.Standings.Mappers;
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
        public CreateStandingUseCase(IStandingRepository repo) => _repo = repo;

        public async Task<StandingResponseDTO> ExecuteAsync(StandingRequestDTO dto)
        {
            var standing = dto.ToDomain();
            var created = await _repo.AddAsync(standing);
            return created.ToDTO();
        }
    }
}
