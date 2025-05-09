using Domain.Entities.Standings;
using Domain.Ports.Leagues;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Get
{
    public class GetStandingsUseCase
    {
        private readonly ILeagueRepository _leagueRepository;

        public GetStandingsUseCase(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public async Task<IEnumerable<Standing>> ExecuteAsync(LeagueID leagueId)
        {
            return await _leagueRepository.GetStandingsAsync(leagueId);
        }
    }

}
