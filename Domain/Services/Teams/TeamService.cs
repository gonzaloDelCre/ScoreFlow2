using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Teams
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger<TeamService> _logger;

        public TeamService(ITeamRepository teamRepository, ILogger<TeamService> logger)
        {
            _teamRepository = teamRepository;
            _logger = logger;
        }

        public async Task<Team> CreateTeamAsync(string teamName, string logoUrl)
        {
            if (string.IsNullOrWhiteSpace(teamName))
                throw new ArgumentException("El nombre del equipo es obligatorio.");

            if (string.IsNullOrWhiteSpace(logoUrl))
                throw new ArgumentException("La URL del logo es obligatoria.");

            // Crear el equipo con ID 0 (se asignará posteriormente)
            var team = new Team(
                new TeamID(0),
                new TeamName(teamName),
                DateTime.UtcNow,
                logoUrl
            );

            // Añadir el equipo al repositorio
            await _teamRepository.AddAsync(team);
            return team;
        }

        public async Task<Team?> GetTeamByIdAsync(TeamID teamId)
        {
            try
            {
                return await _teamRepository.GetByIdAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el equipo con ID {TeamID}.", teamId.Value);
                throw;
            }
        }

        public async Task UpdateTeamAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser nulo.");

            await _teamRepository.UpdateAsync(team);
        }

        public async Task<bool> DeleteTeamAsync(TeamID teamId)
        {
            try
            {
                return await _teamRepository.DeleteAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el equipo con ID {TeamID}.", teamId.Value);
                throw;
            }
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            try
            {
                return await _teamRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de equipos.");
                throw;
            }
        }
    }
}
