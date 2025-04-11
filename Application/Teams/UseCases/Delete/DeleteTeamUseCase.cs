using Domain.Services.Teams;
using Domain.Shared;

namespace Application.Teams.UseCases.Delete
{
    public class DeleteTeamUseCase
    {
        private readonly TeamService _teamService;

        public DeleteTeamUseCase(TeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task ExecuteAsync(int id)
        {
            try
            {
                var success = await _teamService.DeleteTeamAsync(new TeamID(id));

                if (!success)
                    throw new ArgumentException("No se pudo eliminar el equipo.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al eliminar el equipo.", ex);
            }
        }
    }
}
