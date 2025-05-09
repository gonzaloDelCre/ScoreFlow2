using Application.MatchEvents.DTOs;
using Application.MatchEvents.UseCases.Create;
using Application.MatchEvents.UseCases.Delete;
using Application.MatchEvents.UseCases.Get;
using Application.MatchEvents.UseCases.Update;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases
{
    public class MatchEventUseCaseHandler
    {
        private readonly CreateMatchEventUseCase _create;
        private readonly UpdateMatchEventUseCase _update;
        private readonly DeleteMatchEventUseCase _delete;
        private readonly GetAllMatchEventsUseCase _getAll;
        private readonly GetMatchEventByIdUseCase _getById;
        private readonly GetMatchEventsByMatchUseCase _getByMatch;
        private readonly GetMatchEventsByPlayerUseCase _getByPlayer;
        private readonly GetMatchEventsByTypeUseCase _getByType;
        private readonly GetMatchEventsByMinuteRangeUseCase _getByMinuteRange;

        public MatchEventUseCaseHandler(
            CreateMatchEventUseCase create,
            UpdateMatchEventUseCase update,
            DeleteMatchEventUseCase delete,
            GetAllMatchEventsUseCase getAll,
            GetMatchEventByIdUseCase getById,
            GetMatchEventsByMatchUseCase getByMatch,
            GetMatchEventsByPlayerUseCase getByPlayer,
            GetMatchEventsByTypeUseCase getByType,
            GetMatchEventsByMinuteRangeUseCase getByMinuteRange)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _getAll = getAll;
            _getById = getById;
            _getByMatch = getByMatch;
            _getByPlayer = getByPlayer;
            _getByType = getByType;
            _getByMinuteRange = getByMinuteRange;
        }

        public Task<MatchEventResponseDTO> CreateAsync(MatchEventRequestDTO dto) => _create.ExecuteAsync(dto);
        public Task<MatchEventResponseDTO?> UpdateAsync(MatchEventRequestDTO dto) => _update.ExecuteAsync(dto);
        public Task<bool> DeleteAsync(int id) => _delete.ExecuteAsync(id);
        public Task<List<MatchEventResponseDTO>> GetAllAsync() => _getAll.ExecuteAsync();
        public Task<MatchEventResponseDTO?> GetByIdAsync(int id) => _getById.ExecuteAsync(id);
        public Task<List<MatchEventResponseDTO>> GetByMatchAsync(int matchId) => _getByMatch.ExecuteAsync(matchId);
        public Task<List<MatchEventResponseDTO>> GetByPlayerAsync(int playerId) => _getByPlayer.ExecuteAsync(playerId);
        public Task<List<MatchEventResponseDTO>> GetByTypeAsync(EventType type) => _getByType.ExecuteAsync(type);
        public Task<List<MatchEventResponseDTO>> GetByMinuteRangeAsync(int from, int to) => _getByMinuteRange.ExecuteAsync(from, to);
    }
}
