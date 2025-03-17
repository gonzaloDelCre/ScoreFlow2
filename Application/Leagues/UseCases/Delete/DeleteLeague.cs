using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Delete
{
    public class DeleteLeague
    {
        private readonly LeagueService _leagueService;

        public DeleteLeague(LeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        public async Task<bool> Execute(LeagueID leagueId)
        {
            if (leagueId == null)
                throw new ArgumentNullException(nameof(leagueId), "El ID de la liga no puede ser nulo.");

            var league = await _leagueService.GetLeagueByIdAsync(leagueId);
            if (league == null)
                throw new InvalidOperationException("La liga no existe. No se puede eliminar.");

            return await _leagueService.DeleteLeagueAsync(leagueId);
        }
    }
}
