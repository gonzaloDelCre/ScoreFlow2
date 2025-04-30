using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;

namespace Application.Leagues.UseCases.Create
{
    public class CreateLeagueUseCase
    {
        private readonly LeagueService _service;
        private readonly LeagueMapper _mapper;

        public CreateLeagueUseCase(LeagueService service, LeagueMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<LeagueResponseDTO> ExecuteAsync(LeagueRequestDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Validaciones
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("El nombre de la liga es obligatorio.");

            var domainLeague = _mapper.MapToDomain(dto);
            var created = await _service.CreateLeagueAsync(
                                    domainLeague.Name,
                                    domainLeague.Description,
                                    domainLeague.CreatedAt
                                );

            return _mapper.MapToDTO(created);
        }
    }
}
