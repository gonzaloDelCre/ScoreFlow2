using Domain.Shared;
using Application.Leagues.DTOs;
using Application.Leagues.UseCases.Create;
using Application.Leagues.UseCases.Delete;
using Application.Leagues.UseCases.Update;
using Application.Leagues.UseCases.Get;

namespace Application.Leagues.UseCases
{
    public class LeagueUseCaseHandler
    {
        private readonly CreateLeagueUseCase _create;
        private readonly GetAllLeaguesUseCase _getAll;
        private readonly GetLeagueByIdUseCase _getById;
        private readonly UpdateLeagueUseCase _update;
        private readonly DeleteLeagueUseCase _delete;

        public LeagueUseCaseHandler(
            CreateLeagueUseCase create,
            GetAllLeaguesUseCase getAll,
            GetLeagueByIdUseCase getById,
            UpdateLeagueUseCase update,
            DeleteLeagueUseCase delete)
        {
            _create = create;
            _getAll = getAll;
            _getById = getById;
            _update = update;
            _delete = delete;
        }

        public Task<LeagueResponseDTO> CreateAsync(LeagueRequestDTO dto) =>
            _create.ExecuteAsync(dto);

        public Task<List<LeagueResponseDTO>> GetAllAsync() =>
            _getAll.ExecuteAsync();

        public Task<LeagueResponseDTO?> GetByIdAsync(int id) =>
            _getById.ExecuteAsync(id);

        public Task<LeagueResponseDTO> UpdateAsync(LeagueRequestDTO dto) =>
            _update.ExecuteAsync(dto);

        public Task DeleteAsync(int id) =>
            _delete.ExecuteAsync(id);
    }
}
