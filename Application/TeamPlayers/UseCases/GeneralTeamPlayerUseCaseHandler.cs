using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.UseCases.Create;
using Application.TeamPlayers.UseCases.Delete;
using Application.TeamPlayers.UseCases.Get;
using Application.TeamPlayers.UseCases.Update;
using Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases
{
    public class GeneralTeamPlayerUseCaseHandler
    {
        private readonly CreateTeamPlayer _createTeamPlayer;
        private readonly GetTeamPlayerByIds _getTeamPlayerByIds;
        private readonly GetTeamPlayersByTeamId _getTeamPlayersByTeamId;
        private readonly GetTeamPlayersByPlayerId _getTeamPlayersByPlayerId;
        private readonly DeleteTeamPlayer _deleteTeamPlayer;
        private readonly UpdateTeamPlayer _updateTeamPlayer;

        public GeneralTeamPlayerUseCaseHandler(
            CreateTeamPlayer createTeamPlayer,
            GetTeamPlayerByIds getTeamPlayerByIds,
            GetTeamPlayersByTeamId getTeamPlayersByTeamId,
            GetTeamPlayersByPlayerId getTeamPlayersByPlayerId,
            DeleteTeamPlayer deleteTeamPlayer,
            UpdateTeamPlayer updateTeamPlayer)
        {
            _createTeamPlayer = createTeamPlayer;
            _getTeamPlayerByIds = getTeamPlayerByIds;
            _getTeamPlayersByTeamId = getTeamPlayersByTeamId;
            _getTeamPlayersByPlayerId = getTeamPlayersByPlayerId;
            _deleteTeamPlayer = deleteTeamPlayer;
            _updateTeamPlayer = updateTeamPlayer;
        }

        public async Task<TeamPlayerResponseDTO?> AddAsync(TeamPlayerRequestDTO dto)
        {
            return await _createTeamPlayer.ExecuteAsync(dto);
        }

        public async Task<TeamPlayerResponseDTO?> GetByIdsAsync(int teamId, int playerId)
        {
            return await _getTeamPlayerByIds.ExecuteAsync(teamId, playerId);
        }

        public async Task<IEnumerable<TeamPlayerResponseDTO>> GetByTeamIdAsync(int teamId)
        {
            return await _getTeamPlayersByTeamId.ExecuteAsync(teamId);
        }

        public async Task<IEnumerable<TeamPlayerResponseDTO>> GetByPlayerIdAsync(int playerId)
        {
            return await _getTeamPlayersByPlayerId.ExecuteAsync(playerId);
        }

        public async Task<bool> DeleteAsync(int teamId, int playerId)
        {
            return await _deleteTeamPlayer.ExecuteAsync(teamId, playerId);
        }

        public async Task UpdateAsync(TeamPlayerRequestDTO dto, int teamId, int playerId)
        {
            await _updateTeamPlayer.ExecuteAsync(dto, teamId, playerId);
        }

    }
}
