using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Update
{
    public class UpdateLeagueUseCase
    {
        private readonly LeagueService _service;
        private readonly LeagueMapper _mapper;

        public UpdateLeagueUseCase(LeagueService service, LeagueMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<LeagueResponseDTO> ExecuteAsync(LeagueRequestDTO dto)
        {
            if (dto.LeagueID == null)
                throw new ArgumentException("El ID de la liga es obligatorio para actualizar.");

            var existing = await _service.GetLeagueByIdAsync(new LeagueID(dto.LeagueID.Value));
            if (existing == null)
                throw new InvalidOperationException("La liga no existe.");

            var updated = _mapper.MapToDomain(dto, existing);
            await _service.UpdateLeagueAsync(updated);

            return _mapper.MapToDTO(updated);
        }
    }
}
