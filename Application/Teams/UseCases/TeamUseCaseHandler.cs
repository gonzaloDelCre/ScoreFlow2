using Application.Teams.DTOs;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;

namespace Application.Teams.UseCases
{
    public class TeamUseCaseHandler
    {
        private readonly CreateTeamUseCase _createTeamUseCase;
        private readonly UpdateTeamUseCase _updateTeamUseCase;
        private readonly GetAllTeamsUseCase _getAllTeamsUseCase;
        private readonly GetTeamByIdUseCase _getTeamByIdUseCase;
        private readonly DeleteTeamUseCase _deleteTeamUseCase;
        private readonly CreateTeamsFromScraperUseCase _scraperUseCase;

        public TeamUseCaseHandler(
            CreateTeamUseCase createTeamUseCase,
            UpdateTeamUseCase updateTeamUseCase,
            GetAllTeamsUseCase getAllTeamsUseCase,
            GetTeamByIdUseCase getTeamByIdUseCase,
            DeleteTeamUseCase deleteTeamUseCase,
            CreateTeamsFromScraperUseCase scraperUseCase)
        {
            _createTeamUseCase = createTeamUseCase;
            _updateTeamUseCase = updateTeamUseCase;
            _getAllTeamsUseCase = getAllTeamsUseCase;
            _getTeamByIdUseCase = getTeamByIdUseCase;
            _deleteTeamUseCase = deleteTeamUseCase;
            _scraperUseCase = scraperUseCase;
        }

        public async Task<TeamResponseDTO> CreateTeamAsync(TeamRequestDTO teamRequestDTO) =>
            await _createTeamUseCase.ExecuteAsync(teamRequestDTO);

        public async Task<TeamResponseDTO> UpdateTeamAsync(TeamRequestDTO teamRequestDTO) =>
            await _updateTeamUseCase.ExecuteAsync(teamRequestDTO);

        public async Task<List<TeamResponseDTO>> GetAllTeamsAsync() =>
            await _getAllTeamsUseCase.ExecuteAsync();

        public async Task<TeamResponseDTO> GetTeamByIdAsync(int id) =>
            await _getTeamByIdUseCase.ExecuteAsync(id);

        public async Task DeleteTeamAsync(int id) =>
            await _deleteTeamUseCase.ExecuteAsync(id);
        public async Task ScrapeAsync()
        {
            await _scraperUseCase.ExecuteAsync();
        }
    }
}