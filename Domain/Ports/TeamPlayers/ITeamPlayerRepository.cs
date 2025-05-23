﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.TeamPlayers;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Ports.TeamPlayers
{
    public interface ITeamPlayerRepository
    {
        Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId);
        Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId);
        Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId);
        Task AddAsync(TeamID teamId, PlayerID playerId, DateTime? joinedAt = null, RoleInTeam? role = null);
        Task UpdateAsync(TeamPlayer teamPlayer);
        Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId);
        Task<TeamPlayer?> AddAsync(TeamPlayer teamPlayer);
    }
}
