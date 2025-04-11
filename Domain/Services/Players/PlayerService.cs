using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Ports.Players;
using Domain.Ports.Teams;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.Players
{
    public class PlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository; // Repositorio para interactuar con los equipos
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository, ILogger<PlayerService> logger)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _logger = logger;
        }

        // Crear un nuevo jugador y asociarlo a un equipo
        public async Task<Player> CreatePlayerAsync(PlayerName name, PlayerPosition position, PlayerAge age, int goals, string? photo, DateTime createdAt, List<TeamID> teamIds)
        {
            if (string.IsNullOrWhiteSpace(name.Value))
                throw new ArgumentException("El nombre del jugador es obligatorio.");

            if (createdAt == DateTime.MinValue)
                throw new ArgumentException("La fecha de creación es obligatoria.");

            // Crear el jugador
            var player = new Player(new PlayerID(0), name, position, age, goals, photo, createdAt, new List<TeamPlayer>());

            foreach (var teamId in teamIds)
            {
                // Obtener el equipo por su ID
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                    throw new ArgumentException($"El equipo con ID {teamId.Value} no existe.");

                // Asociar el jugador con el equipo a través de TeamPlayer
                var teamPlayer = new TeamPlayer(new TeamID(teamId.Value), new PlayerID(player.PlayerID.Value), DateTime.UtcNow, RoleInTeam.JUGADOR);
                player.AddTeamPlayer(teamPlayer);
            }

            // Guardar el jugador en la base de datos
            await _playerRepository.AddAsync(player);
            return player;
        }

        // Obtener un jugador por su ID
        public async Task<Player?> GetPlayerByIdAsync(PlayerID playerId)
        {
            try
            {
                return await _playerRepository.GetByIdAsync(playerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el jugador con ID {PlayerID}.", playerId.Value);
                throw;
            }
        }

        // Actualizar los detalles del jugador
        public async Task UpdatePlayerAsync(PlayerID playerId, PlayerName name, PlayerPosition position, PlayerAge age, int goals, string? photo, DateTime createdAt)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                throw new ArgumentException("El jugador especificado no existe.");

            player.Update(name, position, age, goals, photo, createdAt);

            await _playerRepository.UpdateAsync(player);
        }

        // Eliminar un jugador
        public async Task<bool> DeletePlayerAsync(PlayerID playerId)
        {
            try
            {
                return await _playerRepository.DeleteAsync(playerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el jugador con ID {PlayerID}.", playerId.Value);
                throw;
            }
        }

        // Obtener todos los jugadores
        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            try
            {
                return await _playerRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de jugadores.");
                throw;
            }
        }

        // Obtener jugadores por equipo
        public async Task<IEnumerable<Player>> GetPlayersByTeamAsync(TeamID teamId)
        {
            try
            {
                return await _playerRepository.GetByTeamIdAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los jugadores del equipo con ID {TeamID}.", teamId.Value);
                throw;
            }
        }
    }
}
