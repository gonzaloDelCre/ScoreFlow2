using Application.MatchEvents.DTOs;
using Application.MatchEvents.Mappers;
using Domain.Ports.MatchEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases.Create
{
    public class CreateMatchEventUseCase
    {
        private readonly IMatchEventRepository _repo;
        public CreateMatchEventUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<MatchEventResponseDTO> ExecuteAsync(MatchEventRequestDTO dto)
        {
            var ev = dto.ToDomain();
            var created = await _repo.AddAsync(ev);
            return created.ToDTO();
        }
    }
}
