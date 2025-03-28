﻿using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Ports.Users; 
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
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TeamService> _logger;

        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository, ILogger<TeamService> logger)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Team> CreateTeamAsync(string teamName, int coachID, string logoUrl)
        {
            if (string.IsNullOrWhiteSpace(teamName))
                throw new ArgumentException("El nombre del equipo es obligatorio.");

            if (string.IsNullOrWhiteSpace(logoUrl))
                throw new ArgumentException("La URL del logo es obligatoria.");

            var userId = new UserID(coachID);

            var coach = await _userRepository.GetByIdAsync(userId);

            if (coach == null)
                throw new ArgumentException("El entrenador con el ID proporcionado no existe.");

            var team = new Team(
                new TeamID(0), 
                new TeamName(teamName),
                coach,
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
