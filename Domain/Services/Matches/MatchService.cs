//using Domain.Entities.Matches;
//using Domain.Entities.Teams;
//using Domain.Enum;
//using Domain.Ports.Matches;
//using Domain.Shared;
//using Microsoft.Extensions.Logging;

//namespace Domain.Services.Matches
//{
//    public class MatchService
//    {
//        private readonly IMatchRepository _matchRepository;
//        private readonly ILogger<MatchService> _logger;

//        public MatchService(IMatchRepository matchRepository, ILogger<MatchService> logger)
//        {
//            _matchRepository = matchRepository;
//            _logger = logger;
//        }

//        public async Task<Match> CreateMatchAsync(Team team1, Team team2, DateTime matchDate, string location)
//        {
//            if (team1 == null || team2 == null)
//                throw new ArgumentException("Ambos equipos deben ser proporcionados.");

//            if (matchDate == DateTime.MinValue)
//                throw new ArgumentException("La fecha del partido es obligatoria.");

//            if (string.IsNullOrWhiteSpace(location))
//                throw new ArgumentException("La ubicación del partido es obligatoria.");

//            var match = new Match(
//                new MatchID(1), 
//                team1,
//                team2,
//                matchDate,
//                MatchStatus.Pendiente,
//                location
//            );

//            await _matchRepository.AddAsync(match);
//            return match;
//        }

//        public async Task<Match?> GetMatchByIdAsync(MatchID matchId)  
//        {
//            try
//            {
//                return await _matchRepository.GetByIdAsync(matchId);
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidOperationException("Hubo un error al obtener el partido.", ex);
//            }
//        }

//        public async Task UpdateMatchAsync(Match match)
//        {
//            if (match == null)
//                throw new ArgumentNullException(nameof(match), "El partido no puede ser nulo.");

//            await _matchRepository.UpdateAsync(match);
//        }

//        public async Task<bool> DeleteMatchAsync(MatchID matchId)  
//        {
//            try
//            {
//                return await _matchRepository.DeleteAsync(matchId);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al eliminar el partido con ID {MatchID}.", matchId.Value);
//                throw;
//            }
//        }

//        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
//        {
//            try
//            {
//                return await _matchRepository.GetAllAsync();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener la lista de partidos.");
//                throw;
//            }
//        }

//        public async Task<IEnumerable<Match>> GetMatchesByTeamIdAsync(MatchID teamId)  
//        {
//            try
//            {
//                return await _matchRepository.GetByTeamIdAsync(teamId);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener los partidos para el equipo con ID {TeamID}.", teamId.Value);
//                throw;
//            }
//        }

//        public async Task<IEnumerable<Match>> GetMatchesByLeagueIdAsync(MatchID leagueId)  
//        {
//            try
//            {
//                return await _matchRepository.GetByLeagueIdAsync(leagueId);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener los partidos para la liga con ID {LeagueID}.", leagueId.Value);
//                throw;
//            }
//        }
//    }
//}

