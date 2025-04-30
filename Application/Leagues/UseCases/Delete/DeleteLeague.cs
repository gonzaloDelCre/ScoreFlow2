using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Delete
{
    public class DeleteLeagueUseCase
    {
        private readonly LeagueService _service;

        public DeleteLeagueUseCase(LeagueService service)
        {
            _service = service;
        }

        public async Task ExecuteAsync(int id)
        {
            var league = await _service.GetLeagueByIdAsync(new LeagueID(id));
            if (league == null)
                throw new InvalidOperationException("La liga no existe.");

            var ok = await _service.DeleteLeagueAsync(new LeagueID(id));
            if (!ok)
                throw new ApplicationException("No se pudo eliminar la liga.");
        }
    }
}
