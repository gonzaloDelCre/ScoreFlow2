using Domain.Ports.Teams;
using Domain.Services.Teams;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Delete
{
    public class DeleteTeam
    {
        private readonly TeamService _teamService;

        public DeleteTeam(TeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<bool> Execute(TeamID teamID)
        {
            if (teamID == null)
                throw new ArgumentNullException(nameof(teamID), "El ID del equipo no puede ser nulo.");

            var existingTeam = await _teamService.GetTeamByIdAsync(teamID);
            if (existingTeam == null)
                throw new InvalidOperationException("El equipo no existe. No se puede eliminar.");

            return await _teamService.DeleteTeamAsync(teamID);
        }
    }
}
