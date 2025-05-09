using Domain.Shared;
using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.UseCases.Get;
using Application.PlayerStatistics.UseCases.Update;
using Application.PlayerStatistics.UseCases.Delete;
using Application.PlayerStatistics.UseCases.Create;

namespace Application.PlayerStatistics.UseCases
{
    public class PlayerStatisticUseCaseHandler
    {
        private readonly CreatePlayerStatisticUseCase _create;
        private readonly UpdatePlayerStatisticUseCase _update;
        private readonly DeletePlayerStatisticUseCase _delete;
        private readonly GetAllPlayerStatisticsUseCase _getAll;
        private readonly GetPlayerStatisticByIdUseCase _getById;
        private readonly GetByPlayerIdUseCase _getByPlayer;
        private readonly GetByMatchIdUseCase _getByMatch;

        public PlayerStatisticUseCaseHandler(
            CreatePlayerStatisticUseCase create,
            UpdatePlayerStatisticUseCase update,
            DeletePlayerStatisticUseCase delete,
            GetAllPlayerStatisticsUseCase getAll,
            GetPlayerStatisticByIdUseCase getById,
            GetByPlayerIdUseCase getByPlayer,
            GetByMatchIdUseCase getByMatch)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _getAll = getAll;
            _getById = getById;
            _getByPlayer = getByPlayer;
            _getByMatch = getByMatch;
        }

        public Task<PlayerStatisticResponseDTO> CreateAsync(PlayerStatisticRequestDTO dto) => _create.ExecuteAsync(dto);
        public Task<PlayerStatisticResponseDTO?> UpdateAsync(PlayerStatisticRequestDTO dto) => _update.ExecuteAsync(dto);
        public Task<bool> DeleteAsync(int id) => _delete.ExecuteAsync(id);
        public Task<List<PlayerStatisticResponseDTO>> GetAllAsync() => _getAll.ExecuteAsync();
        public Task<PlayerStatisticResponseDTO?> GetByIdAsync(int id) => _getById.ExecuteAsync(id);
        public Task<List<PlayerStatisticResponseDTO>> GetByPlayerAsync(int playerId) => _getByPlayer.ExecuteAsync(playerId);
        public Task<List<PlayerStatisticResponseDTO>> GetByMatchAsync(int matchId) => _getByMatch.ExecuteAsync(matchId);
    }
}
