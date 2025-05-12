using Domain.Shared;
using Application.Leagues.DTOs;
using Application.Leagues.UseCases.Create;
using Application.Leagues.UseCases.Delete;
using Application.Leagues.UseCases.Update;
using Application.Leagues.UseCases.Get;
using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Application.Leagues.UseCases.Scraping;

namespace Application.Leagues.UseCases
{
    public class LeagueUseCaseHandler
    {
        private readonly CreateLeagueUseCase _create;
        private readonly UpdateLeagueUseCase _update;
        private readonly GetAllLeaguesUseCase _getAll;
        private readonly GetLeagueByIdUseCase _getById;
        private readonly GetLeagueByNameUseCase _getByName;
        private readonly GetStandingsUseCase _getStandings;
        private readonly UpdateStandingsUseCase _updateStandings;
        private readonly DeleteLeagueUseCase _delete;
        private readonly ImportLeaguesUseCase _import;


        public LeagueUseCaseHandler(
            CreateLeagueUseCase create,
            UpdateLeagueUseCase update,
            GetAllLeaguesUseCase getAll,
            GetLeagueByIdUseCase getById,
            GetLeagueByNameUseCase getByName,
            GetStandingsUseCase getStandings,
            UpdateStandingsUseCase updateStandings,
            DeleteLeagueUseCase delete,
            ImportLeaguesUseCase import)

        {
            _create = create;
            _update = update;
            _getAll = getAll;
            _getById = getById;
            _getByName = getByName;
            _getStandings = getStandings;
            _updateStandings = updateStandings;
            _delete = delete;
            _import = import;
        }

        public Task<LeagueResponseDTO> CreateLeagueAsync(LeagueRequestDTO dto) =>
            _create.ExecuteAsync(dto);

        public Task<LeagueResponseDTO?> UpdateLeagueAsync(LeagueRequestDTO dto) =>
            _update.ExecuteAsync(dto);

        public Task<List<LeagueResponseDTO>> GetAllLeaguesAsync() =>
            _getAll.ExecuteAsync();

        public Task<LeagueResponseDTO?> GetLeagueByIdAsync(int id) =>
            _getById.ExecuteAsync(id);

        public Task<LeagueResponseDTO?> GetLeagueByNameAsync(string name) =>
            _getByName.ExecuteAsync(name);

        public async Task<List<StandingResponseDTO>> GetStandingsAsync(int leagueId)
        {
            var standings = await _getStandings.ExecuteAsync(new LeagueID(leagueId));
            return standings.Select(s => s.ToDTO()).ToList();
        }

        public Task UpdateStandingsAsync(int leagueId, List<StandingRequestDTO> standings) =>
            _updateStandings.ExecuteAsync(
                new LeagueID(leagueId),
                standings.Select(StandingMapper.ToDomain)
            );

        public Task<bool> DeleteLeagueAsync(int id) =>
            _delete.ExecuteAsync(id);

        public Task ImportLeaguesAsync(string competitionId, bool importMatches)
        {
            return _import.ExecuteAsync(competitionId,importMatches);
        }

    }
}
