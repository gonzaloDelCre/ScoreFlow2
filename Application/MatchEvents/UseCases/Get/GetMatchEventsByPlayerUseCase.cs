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
    public class GetMatchEventsByPlayerUseCase
    {
        private readonly IMatchEventRepository _repo;
        public GetMatchEventsByPlayerUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<List<MatchEventResponseDTO>> ExecuteAsync(int playerId)
        {
            var list = await _repo.GetByPlayerIdAsync(new PlayerID(playerId));
            return list.Select(e => e.ToDTO()).ToList();
        }
    }
}
