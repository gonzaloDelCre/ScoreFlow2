using Application.Playes.DTOs;
using Application.Playes.UseCases.Create;
using Application.Playes.UseCases.Delete;
using Application.Playes.UseCases.Get;
using Application.Playes.UseCases.Scraping;
using Application.Playes.UseCases.Update;
using Domain.Enum;
using Domain.Shared;

namespace Application.Playes.UseCases
{
    public class PlayerUseCaseHandler
    {
        private readonly CreatePlayerUseCase _create;
        private readonly UpdatePlayerUseCase _update;
        private readonly DeletePlayerUseCase _delete;
        private readonly GetAllPlayersUseCase _getAll;
        private readonly GetPlayerByIdUseCase _getById;
        private readonly GetPlayerByNameUseCase _getByName;
        private readonly GetPlayersByTeamUseCase _getByTeam;
        private readonly GetPlayersByPositionUseCase _getByPosition;
        private readonly GetPlayersByAgeRangeUseCase _getByAgeRange;
        private readonly GetTopScorersUseCase _getTopScorers;
        private readonly SearchPlayersByNameUseCase _searchByName;
        private readonly ImportPlayersByTeamExternalIdUseCase _importUseCase;

        public PlayerUseCaseHandler(
            CreatePlayerUseCase create,
            UpdatePlayerUseCase update,
            DeletePlayerUseCase delete,
            GetAllPlayersUseCase getAll,
            GetPlayerByIdUseCase getById,
            GetPlayerByNameUseCase getByName,
            GetPlayersByTeamUseCase getByTeam,
            GetPlayersByPositionUseCase getByPosition,
            GetPlayersByAgeRangeUseCase getByAgeRange,
            GetTopScorersUseCase getTopScorers,
            SearchPlayersByNameUseCase searchByName,
            ImportPlayersByTeamExternalIdUseCase importUseCase)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _getAll = getAll;
            _getById = getById;
            _getByName = getByName;
            _getByTeam = getByTeam;
            _getByPosition = getByPosition;
            _getByAgeRange = getByAgeRange;
            _getTopScorers = getTopScorers;
            _searchByName = searchByName;
            _importUseCase = importUseCase;
        }

        public Task<PlayerResponseDTO> CreateAsync(PlayerRequestDTO dto) => _create.ExecuteAsync(dto);
        public Task<PlayerResponseDTO?> UpdateAsync(PlayerRequestDTO dto) => _update.ExecuteAsync(dto);
        public Task<bool> DeleteAsync(int id) => _delete.ExecuteAsync(id);
        public Task<List<PlayerResponseDTO>> GetAllAsync() => _getAll.ExecuteAsync();
        public Task<PlayerResponseDTO?> GetByIdAsync(int id) => _getById.ExecuteAsync(id);
        public Task<PlayerResponseDTO?> GetByNameAsync(string name) => _getByName.ExecuteAsync(name);
        public Task<List<PlayerResponseDTO>> GetByTeamAsync(int teamId) => _getByTeam.ExecuteAsync(teamId);
        public Task<List<PlayerResponseDTO>> GetByPositionAsync(PlayerPosition position) => _getByPosition.ExecuteAsync(position);
        public Task<List<PlayerResponseDTO>> GetByAgeRangeAsync(int minAge, int maxAge) => _getByAgeRange.ExecuteAsync(minAge, maxAge);
        public Task<List<PlayerResponseDTO>> GetTopScorersAsync(int topN) => _getTopScorers.ExecuteAsync(topN);
        public Task<List<PlayerResponseDTO>> SearchByNameAsync(string partialName) => _searchByName.ExecuteAsync(partialName);
        public async Task ImportByTeamExternalIdAsync(int teamExternalId)
        {
            await _importUseCase.ExecuteAsync(teamExternalId);
        }
    }
}
