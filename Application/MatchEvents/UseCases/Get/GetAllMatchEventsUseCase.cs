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
    public class GetAllMatchEventsUseCase
    {
        private readonly IMatchEventRepository _repo;
        public GetAllMatchEventsUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<List<MatchEventResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(e => e.ToDTO()).ToList();
        }
    }
}
