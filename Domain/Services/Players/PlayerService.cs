using Domain.Entities.Players;
using Domain.Enum;
using Domain.Ports.Players;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Players
{
    public class PlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository playerRepository, ILogger<PlayerService> logger)
        {
            _playerRepository = playerRepository;
            _logger = logger;
        }

        public async Task<Player> CreatePlayerAsync(PlayerName name, TeamID teamId, PlayerPosition position, DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(name.Value))
                throw new ArgumentException("El nombre del jugador es obligatorio.");

            if (createdAt == DateTime.MinValue)
                throw new ArgumentException("La fecha de creación es obligatoria.");

            var player = new Player(new PlayerID(0), name, teamId, position, null, createdAt); 
            await _playerRepository.AddAsync(player);
            return player;
        }

        public async Task<Player?> GetPlayerByIdAsync(PlayerID playerId)
        {
            try
            {
                return await _playerRepository.GetByIdAsync(playerId.Value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Hubo un error al obtener el jugador.", ex);
            }
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "El jugador no puede ser nulo.");

            await _playerRepository.UpdateAsync(player);
        }

        public async Task<bool> DeletePlayerAsync(PlayerID playerId)
        {
            try
            {
                return await _playerRepository.DeleteAsync(playerId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el jugador con ID {PlayerID}.", playerId.Value);
                throw;
            }
        }

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

        public async Task<IEnumerable<Player>> GetPlayersByTeamAsync(TeamID teamId)
        {
            try
            {
                return await _playerRepository.GetByTeamIdAsync(teamId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los jugadores del equipo con ID {TeamID}.", teamId.Value);
                throw;
            }
        }
    }
}

