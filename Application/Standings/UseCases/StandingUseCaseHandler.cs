using Application.Standings.DTOs;
using Application.Standings.UseCases.Create;
using Application.Standings.UseCases.Delete;
using Application.Standings.UseCases.Get;
using Application.Standings.UseCases.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases
{
    public class StandingUseCaseHandler
    {
        private readonly CreateStandingUseCase _create;
        private readonly GetAllStandingsUseCase _getAll;
        private readonly GetStandingByIdUseCase _getById;
        private readonly UpdateStandingUseCase _update;
        private readonly DeleteStandingUseCase _delete;
        private readonly GetByLeagueUseCase _byLeague;
        private readonly GetClassificationUseCase _classification;
        private readonly GetByTeamAndLeagueUseCase _byTeamLeague;

        public StandingUseCaseHandler(
            CreateStandingUseCase create,
            GetAllStandingsUseCase getAll,
            GetStandingByIdUseCase getById,
            UpdateStandingUseCase update,
            DeleteStandingUseCase delete,
            GetByLeagueUseCase byLeague,
            GetClassificationUseCase classification,
            GetByTeamAndLeagueUseCase byTeamLeague)
        {
            _create = create;
            _getAll = getAll;
            _getById = getById;
            _update = update;
            _delete = delete;
            _byLeague = byLeague;
            _classification = classification;
            _byTeamLeague = byTeamLeague;
        }

        public Task<StandingResponseDTO> CreateAsync(StandingRequestDTO dto) =>
            _create.ExecuteAsync(dto);

        public Task<List<StandingResponseDTO>> GetAllAsync() =>
            _getAll.ExecuteAsync();

        public Task<StandingResponseDTO> GetByIdAsync(int id) =>
            _getById.ExecuteAsync(id);

        public Task<StandingResponseDTO> UpdateAsync(StandingRequestDTO dto) =>
            _update.ExecuteAsync(dto);

        public Task DeleteAsync(int id) =>
            _delete.ExecuteAsync(id);

        public Task<List<StandingResponseDTO>> GetByLeagueAsync(int leagueId) =>
            _byLeague.ExecuteAsync(leagueId);

        public Task<List<StandingResponseDTO>> GetClassificationAsync(int leagueId) =>
            _classification.ExecuteAsync(leagueId);

        public Task<StandingResponseDTO> GetByTeamAndLeagueAsync(int teamId, int leagueId) =>
            _byTeamLeague.ExecuteAsync(teamId, leagueId);
    }
}
