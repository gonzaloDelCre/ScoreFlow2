using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Delete
{
    public class DeleteLeague
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly LeagueService _leagueService;

        public DeleteLeague(ILeagueRepository leagueRepository, LeagueService leagueService)
        {
            _leagueRepository = leagueRepository;
            _leagueService = leagueService;
        }

        public async Task<bool> Execute(LeagueID leagueId)
        {
            // Validar el ID de la liga antes de eliminarla
            if (leagueId == null)
                throw new ArgumentException("El ID de la liga no puede ser nulo.");

            return await _leagueRepository.DeleteAsync(leagueId);
        }
    }
}
