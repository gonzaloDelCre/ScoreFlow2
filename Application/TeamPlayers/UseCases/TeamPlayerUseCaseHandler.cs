using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.UseCases.Create;
using Application.TeamPlayers.UseCases.Delete;
using Application.TeamPlayers.UseCases.Get;
using Application.TeamPlayers.UseCases.Scraping;
using Application.TeamPlayers.UseCases.Update;
using Domain.Enum;
using Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases
{
    public class TeamPlayerUseCaseHandler
    {
        private readonly CreateTeamPlayerUseCase _create;
        private readonly UpdateTeamPlayerUseCase _update;
        private readonly DeleteTeamPlayerUseCase _delete;
        private readonly GetAllTeamPlayersUseCase _getAll;
        private readonly GetTeamPlayerByIdsUseCase _getByIds;
        private readonly GetTeamPlayersByTeamUseCase _getByTeam;
        private readonly GetTeamPlayersByPlayerUseCase _getByPlayer;
        private readonly GetTeamPlayersByRoleUseCase _getByRole;
        private readonly GetTeamPlayersByJoinDateRangeUseCase _getByDateRange;
        private readonly LinkPlayersToTeamUseCase _linkUseCase;


        public TeamPlayerUseCaseHandler(
            CreateTeamPlayerUseCase create,
            UpdateTeamPlayerUseCase update,
            DeleteTeamPlayerUseCase delete,
            GetAllTeamPlayersUseCase getAll,
            GetTeamPlayerByIdsUseCase getByIds,
            GetTeamPlayersByTeamUseCase getByTeam,
            GetTeamPlayersByPlayerUseCase getByPlayer,
            GetTeamPlayersByRoleUseCase getByRole,
            GetTeamPlayersByJoinDateRangeUseCase getByDateRange,
            LinkPlayersToTeamUseCase linkUseCase)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _getAll = getAll;
            _getByIds = getByIds;
            _getByTeam = getByTeam;
            _getByPlayer = getByPlayer;
            _getByRole = getByRole;
            _getByDateRange = getByDateRange;
            _linkUseCase = linkUseCase;

        }

        public Task<TeamPlayerResponseDTO> CreateAsync(TeamPlayerRequestDTO dto) => _create.ExecuteAsync(dto);
        public Task<TeamPlayerResponseDTO?> UpdateAsync(TeamPlayerRequestDTO dto) => _update.ExecuteAsync(dto);
        public Task<bool> DeleteAsync(int teamId, int playerId) => _delete.ExecuteAsync(teamId, playerId);
        public Task<List<TeamPlayerResponseDTO>> GetAllAsync() => _getAll.ExecuteAsync();
        public Task<TeamPlayerResponseDTO?> GetByIdsAsync(int teamId, int playerId) => _getByIds.ExecuteAsync(teamId, playerId);
        public Task<List<TeamPlayerResponseDTO>> GetByTeamAsync(int teamId) => _getByTeam.ExecuteAsync(teamId);
        public Task<List<TeamPlayerResponseDTO>> GetByPlayerAsync(int playerId) => _getByPlayer.ExecuteAsync(playerId);
        public Task<List<TeamPlayerResponseDTO>> GetByRoleAsync(RoleInTeam role) => _getByRole.ExecuteAsync(role);
        public Task<List<TeamPlayerResponseDTO>> GetByJoinDateRangeAsync(DateTime from, DateTime to) => _getByDateRange.ExecuteAsync(from, to);
        public Task<int> LinkPlayersToTeamAsync(int teamId)
        {
            return _linkUseCase.ExecuteAsync(teamId);
        }
    }
}
