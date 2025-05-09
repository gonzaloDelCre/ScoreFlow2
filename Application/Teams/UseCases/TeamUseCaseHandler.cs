using Application.Playes.DTOs;
using Application.Standings.DTOs;
using Application.Teams.DTOs;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Scraping;
using Application.Teams.UseCases.Update;

namespace Application.Teams.UseCases
{
    public class TeamUseCaseHandler
    {
        private readonly CreateTeamUseCase _create;
        private readonly UpdateTeamUseCase _update;
        private readonly GetAllTeamsUseCase _getAll;
        private readonly GetTeamByIdUseCase _getById;
        private readonly GetTeamByExternalIdUseCase _getByExternal;
        private readonly GetTeamsByCategoryUseCase _getByCategory;
        private readonly SearchTeamsByNameUseCase _searchByName;
        private readonly GetTeamPlayersUseCase _getPlayers;
        private readonly GetTeamStandingsUseCase _getStandings;
        private readonly DeleteTeamUseCase _delete;
        private readonly ImportTeamsUseCase _import;

        public TeamUseCaseHandler(
            CreateTeamUseCase create,
            UpdateTeamUseCase update,
            GetAllTeamsUseCase getAll,
            GetTeamByIdUseCase getById,
            GetTeamByExternalIdUseCase getByExternal,
            GetTeamsByCategoryUseCase getByCategory,
            SearchTeamsByNameUseCase searchByName,
            GetTeamPlayersUseCase getPlayers,
            GetTeamStandingsUseCase getStandings,
            DeleteTeamUseCase delete,
            ImportTeamsUseCase importTeamsUse)
        {
            _create = create;
            _update = update;
            _getAll = getAll;
            _getById = getById;
            _getByExternal = getByExternal;
            _getByCategory = getByCategory;
            _searchByName = searchByName;
            _getPlayers = getPlayers;
            _getStandings = getStandings;
            _delete = delete;
            _import = importTeamsUse;
        }

        public Task<TeamResponseDTO> CreateTeamAsync(TeamRequestDTO dto) => _create.ExecuteAsync(dto);
        public Task<TeamResponseDTO?> UpdateTeamAsync(TeamRequestDTO dto) => _update.ExecuteAsync(dto);
        public Task<List<TeamResponseDTO>> GetAllTeamsAsync() => _getAll.ExecuteAsync();
        public Task<TeamResponseDTO?> GetTeamByIdAsync(int id) => _getById.ExecuteAsync(id);
        public Task<TeamResponseDTO?> GetTeamByExternalIdAsync(string externalId) => _getByExternal.ExecuteAsync(externalId);
        public Task<List<TeamResponseDTO>> GetTeamsByCategoryAsync(string category) => _getByCategory.ExecuteAsync(category);
        public Task<List<TeamResponseDTO>> SearchTeamsByNameAsync(string partialName) => _searchByName.ExecuteAsync(partialName);
        public Task<List<PlayerResponseDTO>> GetTeamPlayersAsync(int teamId) => _getPlayers.ExecuteAsync(teamId);
        public Task<List<StandingResponseDTO>> GetTeamStandingsAsync(int teamId) => _getStandings.ExecuteAsync(teamId);
        public Task<bool> DeleteTeamAsync(int id) => _delete.ExecuteAsync(id);
        public async Task ImportTeamsAsync()
        {
            await _import.ExecuteAsync();
        }
    }
}