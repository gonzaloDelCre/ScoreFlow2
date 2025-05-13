using Domain.Entities.Standings;
using Domain.Ports.Standings;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Standings.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Services.Scraping.Standings.Import
{
    public class StandingsImportService
    {
        private readonly StandingsScraperService _scraper;
        private readonly ITeamRepository _teamRepo;
        private readonly IStandingRepository _standingRepo;
        private readonly ILogger<StandingsImportService> _logger;

        public StandingsImportService(
            StandingsScraperService scraper,
            ITeamRepository teamRepo,
            IStandingRepository standingRepo,
            ILogger<StandingsImportService> logger)
        {
            _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
            _teamRepo = teamRepo ?? throw new ArgumentNullException(nameof(teamRepo));
            _standingRepo = standingRepo ?? throw new ArgumentNullException(nameof(standingRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Importa la clasificación y devuelve el número de filas procesadas.
        /// </summary>
        public async Task<int> ImportAsync(int competitionId, int leagueId)
        {
            if (competitionId <= 0)
                throw new ArgumentException("El ID de competición debe ser mayor que cero", nameof(competitionId));

            if (leagueId <= 0)
                throw new ArgumentException("El ID de liga debe ser mayor que cero", nameof(leagueId));

            _logger.LogInformation("Iniciando importación de clasificación: competitionId={CompetitionId}, leagueId={LeagueId}",
                competitionId, leagueId);

            List<(int, int, int, int, int, int, int, int, int)> scraped;

            try
            {
                scraped = await _scraper.GetStandingsAsync(competitionId);
                _logger.LogInformation("Obtenidas {Count} filas de la clasificación", scraped.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de la clasificación para competitionId={CompetitionId}",
                    competitionId);
                throw new ApplicationException($"Error al obtener datos de clasificación: {ex.Message}", ex);
            }

            int processed = 0;
            int updated = 0;
            int created = 0;
            int skipped = 0;

            // Using transaction to ensure consistency
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var (extId, pts, played, won, drawn, lost, gf, ga, gd) in scraped)
                    {
                        _logger.LogDebug("Procesando equipo ExternalID={ExternalId}", extId);

                        var team = await _teamRepo.GetByExternalIdAsync(extId.ToString());
                        if (team == null)
                        {
                            _logger.LogWarning("Equipo con ExternalID={ExternalId} no encontrado en el sistema, saltando", extId);
                            skipped++;
                            continue;
                        }

                        var existing = await _standingRepo
                            .GetByTeamIdAndLeagueIdAsync(team.TeamID, new LeagueID(leagueId));

                        if (existing == null)
                        {
                            _logger.LogInformation("Creando nuevo registro de clasificación para teamId={TeamId}, leagueId={LeagueId}",
                                team.TeamID.Value, leagueId);

                            // Get next available ID or use repository to generate one
                            var nextId = await GetNextStandingIdAsync();

                            var newStanding = new Standing(
                                new StandingID(nextId),
                                new LeagueID(leagueId),
                                team.TeamID,
                                new Points(pts),
                                new MatchesPlayed(played),
                                new Wins(won),
                                new Draws(drawn),
                                new Losses(lost),
                                new GoalDifference(gd),
                                DateTime.UtcNow
                            );

                            // Store goals for/against in metadata or add properties to entity if needed
                            // This would require extending the Standing model or adding metadata support

                            await _standingRepo.AddAsync(newStanding);
                            created++;
                        }
                        else
                        {
                            _logger.LogDebug("Verificando cambios para standing existente ID={StandingId}", existing.StandingID.Value);

                            bool dirty = false;
                            if (existing.Points.Value != pts) { existing.UpdatePoints(new Points(pts)); dirty = true; }
                            if (existing.MatchesPlayed.Value != played) { existing.UpdateMatchesPlayed(new MatchesPlayed(played)); dirty = true; }
                            if (existing.Wins.Value != won) { existing.UpdateWins(new Wins(won)); dirty = true; }
                            if (existing.Draws.Value != drawn) { existing.UpdateDraws(new Draws(drawn)); dirty = true; }
                            if (existing.Losses.Value != lost) { existing.UpdateLosses(new Losses(lost)); dirty = true; }
                            if (existing.GoalDifference.Value != gd) { existing.UpdateGoalDifference(new GoalDifference(gd)); dirty = true; }

                            if (dirty)
                            {
                                _logger.LogInformation("Actualizando standing ID={StandingId} con nuevos datos", existing.StandingID.Value);
                                await _standingRepo.UpdateAsync(existing);
                                updated++;
                            }
                            else
                            {
                                _logger.LogDebug("No hay cambios para standing ID={StandingId}", existing.StandingID.Value);
                            }
                        }

                        processed++;
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error durante la importación de clasificación");
                    throw;
                }
            }

            _logger.LogInformation("Importación completada: {Processed} filas procesadas, {Created} creadas, {Updated} actualizadas, {Skipped} saltadas",
                processed, created, updated, skipped);

            return processed;
        }

        private async Task<int> GetNextStandingIdAsync()
        {
            try
            {
                var allStandings = await _standingRepo.GetAllAsync();
                return allStandings.Any()
                    ? allStandings.Max(s => s.StandingID.Value) + 1
                    : 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener siguiente ID de standing");
                return new Random().Next(10000, 99999); // Fallback (not ideal but better than hardcoded 1)
            }
        }
    }
}
