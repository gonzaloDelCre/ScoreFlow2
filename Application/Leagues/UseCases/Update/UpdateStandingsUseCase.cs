using Domain.Entities.Standings;
using Domain.Ports.Leagues;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Update
{
    public class UpdateStandingsUseCase
    {
        private readonly ILeagueRepository _leagueRepository;

        public UpdateStandingsUseCase(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public async Task ExecuteAsync(LeagueID leagueId, IEnumerable<Standing> standings)
        {
            await _leagueRepository.UpdateStandingsAsync(leagueId, standings);
        }
    }

}
