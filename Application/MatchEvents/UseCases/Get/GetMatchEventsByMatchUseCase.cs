using Application.MatchEvents.DTOs;
using Application.MatchEvents.Mappers;
using Domain.Ports.MatchEvents;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases.Get
{
    public class GetMatchEventsByMatchUseCase
    {
        private readonly IMatchEventRepository _repo;
        public GetMatchEventsByMatchUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<List<MatchEventResponseDTO>> ExecuteAsync(int matchId)
        {
            var list = await _repo.GetByMatchIdAsync(new MatchID(matchId));
            return list.Select(e => e.ToDTO()).ToList();
        }
    }
}
