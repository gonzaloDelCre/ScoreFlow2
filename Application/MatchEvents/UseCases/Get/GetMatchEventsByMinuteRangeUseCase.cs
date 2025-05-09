using Application.MatchEvents.DTOs;
using Application.MatchEvents.Mappers;
using Domain.Ports.MatchEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases.Get
{
    public class GetMatchEventsByMinuteRangeUseCase
    {
        private readonly IMatchEventRepository _repo;
        public GetMatchEventsByMinuteRangeUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<List<MatchEventResponseDTO>> ExecuteAsync(int from, int to)
        {
            var list = await _repo.GetByMinuteRangeAsync(from, to);
            return list.Select(e => e.ToDTO()).ToList();
        }
    }
}
