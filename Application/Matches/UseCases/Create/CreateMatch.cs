using Application.Matches.DTOs;
using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Services.Matches;
using Domain.Shared;

namespace Application.Matches.UseCases.Create
{
    public class CreateMatch
    {
        private readonly MatchService _matchService;
        private readonly ITeamRepository _teamRepository;  // Repositorio para obtener los equipos

        public CreateMatch(MatchService matchService, ITeamRepository teamRepository)
        {
            _matchService = matchService;
            _teamRepository = teamRepository;
        }

        // Ejecuta la creación de un nuevo partido
        public async Task<Match> Execute(MatchRequestDTO matchDTO)
        {
            if (matchDTO == null)
                throw new ArgumentNullException(nameof(matchDTO), "Los detalles del partido no pueden ser nulos.");

            // Obtener los equipos desde el repositorio
            var team1 = await _teamRepository.GetByIdAsync(new TeamID(matchDTO.Team1ID));
            var team2 = await _teamRepository.GetByIdAsync(new TeamID(matchDTO.Team2ID));

            if (team1 == null || team2 == null)
                throw new ArgumentException("Uno o ambos equipos no existen.");

            var matchDate = matchDTO.MatchDate;
            var location = matchDTO.Location ?? string.Empty; // Asume un valor por defecto si Location es null

            // Aquí creamos el partido utilizando los equipos obtenidos y otros datos
            return await _matchService.CreateMatchAsync(team1, team2, matchDate, location);
        }
    }
}
