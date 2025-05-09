using Application.Matches.DTOs;
using Domain.Shared;
using Application.Matches.UseCases;
using Application.Matches.UseCases.Get;
using Application.Matches.UseCases.Create;
using Application.Matches.UseCases.Delete;
using Application.Matches.UseCases.Update;

namespace Application.Matches.UseCases
{
    public class MatchUseCaseHandler
    {
        private readonly CreateMatchUseCase _create;
        private readonly UpdateMatchUseCase _update;
        private readonly GetAllMatchesUseCase _getAll;
        private readonly GetMatchByIdUseCase _getById;
        private readonly GetMatchesByTeamUseCase _getByTeam;
        private readonly GetMatchesByLeagueUseCase _getByLeague;
        private readonly DeleteMatchUseCase _delete;

        public MatchUseCaseHandler(
            CreateMatchUseCase create,
            UpdateMatchUseCase update,
            GetAllMatchesUseCase getAll,
            GetMatchByIdUseCase getById,
            GetMatchesByTeamUseCase getByTeam,
            GetMatchesByLeagueUseCase getByLeague,
            DeleteMatchUseCase delete)
        {
            _create = create;
            _update = update;
            _getAll = getAll;
            _getById = getById;
            _getByTeam = getByTeam;
            _getByLeague = getByLeague;
            _delete = delete;
        }

        public Task<MatchResponseDTO> CreateMatchAsync(MatchRequestDTO dto) =>
            _create.ExecuteAsync(dto);

        public Task<MatchResponseDTO?> UpdateMatchAsync(MatchRequestDTO dto) =>
            _update.ExecuteAsync(dto);

        public Task<List<MatchResponseDTO>> GetAllMatchesAsync() =>
            _getAll.ExecuteAsync();

        public Task<MatchResponseDTO?> GetMatchByIdAsync(int id) =>
            _getById.ExecuteAsync(id);

        public Task<List<MatchResponseDTO>> GetMatchesByTeamAsync(int teamId) =>
            _getByTeam.ExecuteAsync(teamId);

        public Task<List<MatchResponseDTO>> GetMatchesByLeagueAsync(int leagueId) =>
            _getByLeague.ExecuteAsync(leagueId);

        public Task<bool> DeleteMatchAsync(int id) =>
            _delete.ExecuteAsync(id);
    }
}
