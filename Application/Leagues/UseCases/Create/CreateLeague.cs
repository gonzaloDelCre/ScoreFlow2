using Application.Leagues.DTOs;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;

namespace Application.Leagues.UseCases.Create
{
    public class CreateLeague
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly LeagueService _leagueService;

        public CreateLeague(ILeagueRepository leagueRepository, LeagueService leagueService)
        {
            _leagueRepository = leagueRepository;
            _leagueService = leagueService;
        }

        public async Task<LeagueResponseDTO> Execute(LeagueRequestDTO leagueDTO)
        {
            if (leagueDTO == null)
                throw new ArgumentNullException(nameof(leagueDTO), "Los detalles de la liga no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(leagueDTO.Name))
                throw new ArgumentException("El nombre de la liga es obligatorio.");

            var existingLeague = await _leagueRepository.GetByNameAsync(leagueDTO.Name);
            if (existingLeague != null)
                throw new ArgumentException("Ya existe una liga con el mismo nombre.");

            var league = await _leagueService.CreateLeagueAsync(
                new LeagueName(leagueDTO.Name),
                leagueDTO.Description,
                leagueDTO.CreatedAt
            );

            return new LeagueResponseDTO
            {
                LeagueID = league.LeagueID.Value,
                Name = league.Name.Value,
                Description = league.Description,
                CreatedAt = league.CreatedAt
            };
        }
    }
}
