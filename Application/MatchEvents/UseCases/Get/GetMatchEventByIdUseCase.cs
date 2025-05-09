using Application.MatchEvents.DTOs;
using Application.MatchEvents.Mappers;
using Domain.Entities.MatchEvents;
using Domain.Ports.MatchEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases.Get
{
    public class GetMatchEventByIdUseCase
    {
        private readonly IMatchEventRepository _repo;
        public GetMatchEventByIdUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<MatchEventResponseDTO?> ExecuteAsync(int id)
        {
            var ev = await _repo.GetByIdAsync(new MatchEventID(id));
            return ev is null ? null : ev.ToDTO();
        }
    }
}
