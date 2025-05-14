using Application.MatchEvents.DTOs;
using Application.MatchEvents.Mappers;
using Domain.Entities.MatchEvents;
using Domain.Ports.MatchEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases.Update
{
    public class UpdateMatchEventUseCase
    {
        private readonly IMatchEventRepository _repo;
        public UpdateMatchEventUseCase(IMatchEventRepository repo) => _repo = repo;

        public async Task<MatchEventResponseDTO?> ExecuteAsync(MatchEventRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar un evento");

            var existing = await _repo.GetByIdAsync(new MatchEventID(dto.ID.Value));
            if (existing == null) return null;

            existing.Update(
                eventType: dto.EventType,
                minute: dto.Minute,
                player: null 
            );

            await _repo.UpdateAsync(existing);
            return existing.ToDTO();
        }
    }
}
