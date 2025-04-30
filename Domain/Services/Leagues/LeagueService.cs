using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Leagues
{
    public class LeagueService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILogger<LeagueService> _logger;

        public LeagueService(ILeagueRepository leagueRepository, ILogger<LeagueService> logger)
        {
            _leagueRepository = leagueRepository;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva liga y devuelve la entidad con el ID asignado.
        /// </summary>
        public async Task<League> CreateLeagueAsync(LeagueName name, string description, DateTime createdAt)
        {
            if (name == null || string.IsNullOrWhiteSpace(name.Value))
                throw new ArgumentException("El nombre de la liga es obligatorio.", nameof(name));

            if (createdAt == DateTime.MinValue)
                throw new ArgumentException("La fecha de creación es obligatoria.", nameof(createdAt));

            // Construyo un objeto temporal con ID 0; el repositorio devolverá el ID real
            var tempLeague = new League(
                new LeagueID(0),
                name,
                description,
                createdAt
            );

            // El AddAsync del repositorio debe retornar la instancia con el ID asignado
            var createdLeague = await _leagueRepository.AddAsync(tempLeague);
            return createdLeague;
        }

        public async Task<League?> GetLeagueByIdAsync(LeagueID leagueId)
        {
            if (leagueId == null)
                throw new ArgumentNullException(nameof(leagueId));

            try
            {
                return await _leagueRepository.GetByIdAsync(leagueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la liga con ID {LeagueID}", leagueId.Value);
                throw new InvalidOperationException("Hubo un error al obtener la liga.", ex);
            }
        }

        public async Task<IEnumerable<League>> GetAllLeaguesAsync()
        {
            try
            {
                return await _leagueRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de ligas.");
                throw;
            }
        }

        public async Task UpdateLeagueAsync(League league)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league), "La liga no puede ser nula.");

            try
            {
                await _leagueRepository.UpdateAsync(league);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la liga con ID {LeagueID}", league.LeagueID.Value);
                throw;
            }
        }

        public async Task<bool> DeleteLeagueAsync(LeagueID leagueId)
        {
            if (leagueId == null)
                throw new ArgumentNullException(nameof(leagueId));

            try
            {
                return await _leagueRepository.DeleteAsync(leagueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la liga con ID {LeagueID}", leagueId.Value);
                throw;
            }
        }
    }
}
