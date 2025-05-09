using Application.MatchEvents.DTOs;
using Application.MatchEvents.Mappers;
using Domain.Enum;
using Domain.Ports.MatchEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases.Get
{
    public class GetMatchEventsByTypeUseCase
    {
        private readonly IMatchEventRepository _repo;
        public GetMatchEventsByTypeUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<List<MatchEventResponseDTO>> ExecuteAsync(EventType type)
        {
            var list = await _repo.GetByTypeAsync(type);
            return list.Select(e => e.ToDTO()).ToList();
        }
    }
}
