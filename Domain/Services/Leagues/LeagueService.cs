//using Domain.Entities.Leagues;
//using Domain.Entities.Standings;
//using Domain.Ports.Leagues;
//using Domain.Shared;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Services.Leagues
//{
//    public class LeagueService
//    {
//        private readonly ILeagueRepository _leagueRepository;
//        private readonly ILogger<LeagueService> _logger;

//        public LeagueService(ILeagueRepository leagueRepository, ILogger<LeagueService> logger)
//        {
//            _leagueRepository = leagueRepository;
//            _logger = logger;
//        }

//        public async Task<League> CreateLeagueAsync(LeagueName name, string description, DateTime createdAt)
//        {
//            if (string.IsNullOrWhiteSpace(name.Value))
//                throw new ArgumentException("El nombre de la liga es obligatorio.");

//            if (createdAt == DateTime.MinValue)
//                throw new ArgumentException("La fecha de creación es obligatoria.");

//            var league = new League(new LeagueID(1), name, description, createdAt);
//            await _leagueRepository.AddAsync(league);
//            return league;
//        }

//        public async Task<League?> GetLeagueByIdAsync(LeagueID leagueId)
//        {
//            try
//            {
//                return await _leagueRepository.GetByIdAsync(leagueId);
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidOperationException("Hubo un error al obtener la liga.", ex);
//            }
//        }

//        public async Task UpdateLeagueAsync(League league)
//        {
//            if (league == null)
//                throw new ArgumentNullException(nameof(league), "La liga no puede ser nula.");

//            await _leagueRepository.UpdateAsync(league);
//        }

//        public async Task<bool> DeleteLeagueAsync(LeagueID leagueId)
//        {
//            try
//            {
//                return await _leagueRepository.DeleteAsync(leagueId);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al eliminar la liga con ID {LeagueID}.", leagueId.Value);
//                throw;
//            }
//        }

//        public async Task<IEnumerable<League>> GetAllLeaguesAsync()
//        {
//            try
//            {
//                return await _leagueRepository.GetAllAsync();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener la lista de ligas.");
//                throw;
//            }
//        }

       
//    }
//}
