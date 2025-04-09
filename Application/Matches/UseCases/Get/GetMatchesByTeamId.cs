//using Application.Matches.DTOs;
//using Domain.Ports.Matches;
//using Domain.Services.Matches;
//using Domain.Shared;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Application.Matches.UseCases.Get
//{
//    public class GetMatchesByTeamId
//    {
//        private readonly MatchService _matchService;

//        public GetMatchesByTeamId(MatchService matchService)
//        {
//            _matchService = matchService;
//        }

//        public async Task<IEnumerable<MatchResponseDTO>> ExecuteAsync(MatchID teamId)
//        {
//            var matches = await _matchService.GetMatchesByTeamIdAsync(teamId);
//            return matches.Select(match => new MatchResponseDTO
//            {
//                MatchID = match.MatchID,
//                Team1 = match.Team1,
//                Team2 = match.Team2,
//                MatchDate = match.MatchDate,
//                Status = match.Status.ToString(),
//                Location = match.Location,
//                CreatedAt = match.CreatedAt
//            });
//        }
//    }
//}
