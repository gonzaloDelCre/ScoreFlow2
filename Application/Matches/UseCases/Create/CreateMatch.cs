using Application.Matches.DTOs;
using Domain.Entities.Matches;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Services.Matches;
using Domain.Shared;

namespace Application.Matches.UseCases.Create
{
    public class CreateMatch
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly MatchService _matchService;

        public CreateMatch(IMatchRepository matchRepository, ITeamRepository teamRepository, MatchService matchService)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _matchService = matchService;
        }

        public async Task<MatchResponseDTO> Execute(MatchRequestDTO matchDTO)
        {
            if (matchDTO == null)
                throw new ArgumentNullException(nameof(matchDTO), "Los detalles del partido no pueden ser nulos.");

            var team1 = await _teamRepository.GetByIdAsync(new TeamID(matchDTO.Team1ID));
            var team2 = await _teamRepository.GetByIdAsync(new TeamID(matchDTO.Team2ID));

            if (team1 == null || team2 == null)
                throw new ArgumentException("Uno o ambos equipos no existen.");

            var match = await _matchService.CreateMatchAsync(
                team1, team2, matchDTO.MatchDate, matchDTO.Location
            );

            return new MatchResponseDTO
            {
                MatchID = match.MatchID,  
                Team1 = team1,  
                Team2 = team2,  
                MatchDate = match.MatchDate,
                Location = match.Location
            };
        }


    }
}
