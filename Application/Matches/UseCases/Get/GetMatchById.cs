//using Application.Matches.DTOs;
//using Domain.Ports.Matches;
//using Domain.Services.Matches;
//using Domain.Shared;
//using System.Threading.Tasks;

//namespace Application.Matches.UseCases.Get
//{
//    public class GetMatchById
//    {
//        private readonly MatchService _matchService;

//        public GetMatchById(MatchService matchService)
//        {
//            _matchService = matchService;
//        }

//        public async Task<MatchResponseDTO?> ExecuteAsync(MatchID matchID)
//        {
//            if (matchID == null)
//                throw new ArgumentNullException(nameof(matchID), "El ID del partido no puede ser nulo.");

//            var match = await _matchService.GetMatchByIdAsync(matchID);
//            return match != null ? new MatchResponseDTO
//            {
//                MatchID = match.MatchID,
//                Team1 = match.Team1,
//                Team2 = match.Team2,
//                MatchDate = match.MatchDate,
//                Status = match.Status.ToString(),
//                Location = match.Location,
//                CreatedAt = match.CreatedAt
//            } : null;
//        }
//    }
//}
