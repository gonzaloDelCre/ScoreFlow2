using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Ports.Users; // Asegúrate de que este namespace esté presente
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.Teams
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository; // Repositorio de Usuarios
        private readonly ILogger<TeamService> _logger;

        // Constructor actualizado para recibir IUserRepository
        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository, ILogger<TeamService> logger)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository; // Inyección del repositorio de usuarios
            _logger = logger;
        }

        public async Task<Team> CreateTeamAsync(string teamName, int coachID, string logoUrl)
        {
            if (string.IsNullOrWhiteSpace(teamName))
                throw new ArgumentException("El nombre del equipo es obligatorio.");

            if (string.IsNullOrWhiteSpace(logoUrl))
                throw new ArgumentException("La URL del logo es obligatoria.");

            // Convertir coachID en un UserID
            var userId = new UserID(coachID);  // Asumimos que UserID es un tipo de valor que recibe un int

            // Buscar el coach usando el repositorio de usuarios
            var coach = await _userRepository.GetByIdAsync(userId);  // Ahora usamos UserID en lugar de coachID

            if (coach == null)
                throw new ArgumentException("El entrenador con el ID proporcionado no existe.");

            var team = new Team(
                new TeamID(1),
                new TeamName(teamName),
                coach,  // Asignamos el entrenador
                DateTime.UtcNow,
                logoUrl
            );

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
