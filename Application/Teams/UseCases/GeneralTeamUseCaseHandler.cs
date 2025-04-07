using Application.Teams.DTOs;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;

namespace Application.Teams.UseCases
{
    public class GeneralTeamUseCaseHandler
    {
        private readonly GetTeamByIdUseCase _getTeamById;
        private readonly CreateTeamUseCase _createTeam;
        private readonly UpdateTeamUseCase _updateTeam;
        private readonly DeleteTeamUseCase _deleteTeam;
        private readonly GetAllTeamsUseCase _getAllTeams;

        public GeneralTeamUseCaseHandler(
            GetTeamByIdUseCase getTeamById,
            CreateTeamUseCase createTeam,
            UpdateTeamUseCase updateTeam,
            DeleteTeamUseCase deleteTeam,
            GetAllTeamsUseCase getAllTeams)
        {
            _getTeamById = getTeamById;
            _createTeam = createTeam;
            _updateTeam = updateTeam;
            _deleteTeam = deleteTeam;
            _getAllTeams = getAllTeams;
        }

        public async Task<object> GetAllTeamsAsync()
        {
            return await _getAllTeams.ExecuteAsync();
        }

        public async Task<TeamResponseDTO> GetTeamByIdAsync(int id)
        {
            return await _getTeamById.ExecuteAsync(id);
        }

        public async Task<TeamResponseDTO> CreateTeamAsync(TeamRequestDTO teamDTO)
        {
            return await _createTeam.ExecuteAsync(teamDTO);
        }

        public async Task<TeamResponseDTO> UpdateTeamAsync(int id, TeamRequestDTO teamDTO)
        {
            teamDTO.TeamID = id;
            return await _updateTeam.ExecuteAsync(teamDTO);
        }

        public async Task DeleteTeamAsync(int id)
        {
            await _deleteTeam.ExecuteAsync(id);
        }
    }
}
