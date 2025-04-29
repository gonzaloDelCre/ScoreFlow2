using Domain.Entities.TeamPlayers;
using Domain.Ports.TeamPlayers;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.TeamPlayers
{
    public class TeamPlayerService
    {
        private readonly ITeamPlayerRepository _teamPlayerRepository;

        public TeamPlayerService(ITeamPlayerRepository teamPlayerRepository)
        {
            _teamPlayerRepository = teamPlayerRepository ?? throw new ArgumentNullException(nameof(teamPlayerRepository));
        }

        /// <summary>
        /// Obtiene una relación entre un equipo y un jugador por sus IDs.
        /// </summary>
        public async Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId)
        {
            return await _teamPlayerRepository.GetByIdsAsync(teamId, playerId);
        }

        /// <summary>
        /// Obtiene todas las relaciones de un equipo por el ID del equipo.
        /// </summary>
        public async Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId)
        {
            return await _teamPlayerRepository.GetByTeamIdAsync(teamId);
        }

        /// <summary>
        /// Obtiene todas las relaciones de un jugador por el ID del jugador.
        /// </summary>
        public async Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId)
        {
            return await _teamPlayerRepository.GetByPlayerIdAsync(playerId);
        }

        /// <summary>
        /// Agrega una nueva relación entre un equipo y un jugador.
        /// </summary>
        public async Task<TeamPlayer?> AddAsync(TeamPlayer teamPlayer)
        {
            if (teamPlayer == null)
                throw new ArgumentNullException(nameof(teamPlayer));

            return await _teamPlayerRepository.AddAsync(teamPlayer);
        }

        /// <summary>
        /// Actualiza una relación entre un equipo y un jugador existente.
        /// </summary>
        public async Task UpdateAsync(TeamPlayer teamPlayer)
        {
            if (teamPlayer == null)
                throw new ArgumentNullException(nameof(teamPlayer));

            await _teamPlayerRepository.UpdateAsync(teamPlayer);
        }

        /// <summary>
        /// Elimina la relación entre un equipo y un jugador.
        /// </summary>
        public async Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId)
        {
            if (teamId == null || playerId == null)
                throw new ArgumentNullException("teamId or playerId cannot be null");

            return await _teamPlayerRepository.DeleteAsync(teamId, playerId);
        }
    }
}
