using Domain.Entities.Standings;
using Domain.Ports.Standings;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Standings
{
    public class StandingService
    {
        private readonly IStandingRepository _standingRepository;
        private readonly ILogger<StandingService> _logger;

        public StandingService(
            IStandingRepository standingRepository,
            ILogger<StandingService> logger)
        {
            _standingRepository = standingRepository;
            _logger = logger;
        }

        public async Task<Standing> CreateStandingAsync(
            LeagueID leagueId,
            TeamID teamId,
            int wins,
            int draws,
            int losses,
            int goalsFor,
            int goalsAgainst)
        {
            if (leagueId == null) throw new ArgumentNullException(nameof(leagueId));
            if (teamId == null) throw new ArgumentNullException(nameof(teamId));
            if (wins < 0 || draws < 0 || losses < 0)
                throw new ArgumentException("Wins, draws and losses must be non-negative.");
            if (goalsFor < 0 || goalsAgainst < 0)
                throw new ArgumentException("Goals for and against must be non-negative.");

            var points = new Points(wins * 2 + draws); // or your league’s point rule

            var standing = new Standing(
                standingID: new StandingID(0),
                leagueID: leagueId,
                teamID: teamId,
                wins: new Wins(wins),
                draws: new Draws(draws),
                losses: new Losses(losses),
                goalsFor: new GoalsFor(goalsFor),
                goalsAgainst: new GoalsAgainst(goalsAgainst),
                points: points,
                league: null,
                team: null,
                createdAt: DateTime.UtcNow
            );

            return await _standingRepository.AddAsync(standing);
        }

        public async Task<Standing?> GetStandingByIdAsync(StandingID standingId)
        {
            try
            {
                return await _standingRepository.GetByIdAsync(standingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching standing {StandingID}", standingId.Value);
                throw;
            }
        }

        public async Task<IEnumerable<Standing>> GetAllStandingsAsync()
        {
            try
            {
                return await _standingRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all standings");
                throw;
            }
        }

        public async Task<IEnumerable<Standing>> GetByLeagueIdAsync(LeagueID leagueId)
        {
            try
            {
                return await _standingRepository.GetByLeagueIdAsync(leagueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching standings for league {LeagueID}", leagueId.Value);
                throw;
            }
        }

        public async Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(LeagueID leagueId)
        {
            try
            {
                return await _standingRepository.GetClassificationByLeagueIdAsync(leagueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching classification for league {LeagueID}", leagueId.Value);
                throw;
            }
        }

        public async Task<Standing?> GetByTeamAndLeagueAsync(TeamID teamId, LeagueID leagueId)
        {
            try
            {
                return await _standingRepository.GetByTeamIdAndLeagueIdAsync(teamId, leagueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching standing for team {TeamID} in league {LeagueID}", teamId.Value, leagueId.Value);
                throw;
            }
        }

        public async Task UpdateStandingAsync(Standing standing)
        {
            if (standing == null) throw new ArgumentNullException(nameof(standing));

            try
            {
                await _standingRepository.UpdateAsync(standing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating standing {StandingID}", standing.StandingID.Value);
                throw;
            }
        }

        public async Task<bool> DeleteStandingAsync(StandingID standingId)
        {
            try
            {
                return await _standingRepository.DeleteAsync(standingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting standing {StandingID}", standingId.Value);
                throw;
            }
        }
    }
}
