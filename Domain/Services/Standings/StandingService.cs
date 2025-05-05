using Domain.Entities.Standings;
using Domain.Ports.Standings;
using Domain.Ports.Teams;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.Standings
{
    public class StandingService
    {
        private readonly IStandingRepository _standingRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger<StandingService> _logger;

        public StandingService(
            IStandingRepository standingRepository,
            ITeamRepository teamRepository,
            ILogger<StandingService> logger)
        {
            _standingRepository = standingRepository;
            _teamRepository = teamRepository;
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

            // 🔍 Validar que el equipo pertenece a la liga
            var team = await _teamRepository.GetByIdAsync(teamId)
                ?? throw new KeyNotFoundException($"El equipo con ID {teamId.Value} no fue encontrado.");

            if (team.LeagueID.Value != leagueId.Value)
                throw new InvalidOperationException($"El equipo con ID {teamId.Value} no pertenece a la liga con ID {leagueId.Value}.");

            var points = new Points(wins * 2 + draws);

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
                team: null,
                createdAt: DateTime.UtcNow
            );

            return await _standingRepository.AddAsync(standing);
        }

        public async Task UpdateStandingAsync(Standing standing)
        {
            if (standing == null) throw new ArgumentNullException(nameof(standing));

            // 🔍 Validar que el equipo aún pertenece a la liga indicada
            var team = await _teamRepository.GetByIdAsync(standing.TeamID)
                ?? throw new KeyNotFoundException($"El equipo con ID {standing.TeamID.Value} no fue encontrado.");

            if (team.LeagueID.Value != standing.LeagueID.Value)
                throw new InvalidOperationException($"El equipo con ID {standing.TeamID.Value} no pertenece a la liga con ID {standing.LeagueID.Value}.");

            try
            {
                await _standingRepository.UpdateAsync(standing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el Standing con ID {StandingID}", standing.StandingID.Value);
                throw;
            }
        }

        public async Task<Standing> GetStandingByIdAsync(StandingID standingId)
        {
            return await _standingRepository.GetByIdAsync(standingId);
        }

        public async Task<IEnumerable<Standing>> GetStandingsByLeagueAsync(LeagueID leagueId)
        {
            return await _standingRepository.GetByLeagueIdAsync(leagueId);
        }

        public async Task<IEnumerable<Standing>> GetClassificationAsync(LeagueID leagueId)
        {
            return await _standingRepository.GetClassificationByLeagueIdAsync(leagueId);
        }

        public async Task<bool> DeleteStandingAsync(StandingID standingId)
        {
            return await _standingRepository.DeleteAsync(standingId);
        }
    }
}
