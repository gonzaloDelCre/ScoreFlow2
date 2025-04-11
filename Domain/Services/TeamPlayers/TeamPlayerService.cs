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

        public async Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId)
        {
            return await _teamPlayerRepository.GetByIdsAsync(teamId, playerId);
        }

        public async Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId)
        {
            return await _teamPlayerRepository.GetByTeamIdAsync(teamId);
        }

        public async Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId)
        {
            return await _teamPlayerRepository.GetByPlayerIdAsync(playerId);
        }

        public async Task<TeamPlayer?> AddAsync(TeamPlayer teamPlayer)
        {
            if (teamPlayer == null)
                throw new ArgumentNullException(nameof(teamPlayer));

            return await _teamPlayerRepository.AddAsync(teamPlayer);
        }

        public async Task UpdateAsync(TeamPlayer teamPlayer)
        {
            if (teamPlayer == null)
                throw new ArgumentNullException(nameof(teamPlayer));

            await _teamPlayerRepository.UpdateAsync(teamPlayer);
        }

        public async Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId)
        {
            if (teamId == null || playerId == null)
                throw new ArgumentNullException("teamId or playerId cannot be null");

            return await _teamPlayerRepository.DeleteAsync(teamId, playerId);
        }
    }
}
