using Domain.Entities.Standings;
using Domain.Ports.Standings;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Standings.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Standings.Import
{
    public class StandingsImportService
    {
        private readonly StandingsScraperService _scraper;
        private readonly ITeamRepository _teamRepo;
        private readonly IStandingRepository _standingRepo;

        public StandingsImportService(
            StandingsScraperService scraper,
            ITeamRepository teamRepo,
            IStandingRepository standingRepo)
        {
            _scraper = scraper;
            _teamRepo = teamRepo;
            _standingRepo = standingRepo;
        }

        /// <summary>
        /// Importa la clasificación y devuelve el número de filas procesadas.
        /// </summary>
        public async Task<int> ImportAsync(int competitionId, int leagueId)
        {
            Console.WriteLine($"🚀 Iniciando import: competitionId={competitionId}, leagueId={leagueId}");
            var scraped = await _scraper.GetStandingsAsync(competitionId);
            Console.WriteLine($"📦 Filas obtenidas: {scraped.Count}");

            int processed = 0;

            foreach (var (extId, pts, played, won, drawn, lost, gf, ga, gd) in scraped)
            {
                Console.WriteLine($"– Procesando equipo ExternalID={extId}...");

                var team = await _teamRepo.GetByExternalIdAsync(extId.ToString());
                if (team == null)
                {
                    Console.WriteLine($"   ⚠️ Equipo {extId} no encontrado, saltando.");
                    continue;
                }

                var existing = await _standingRepo
                    .GetByTeamIdAndLeagueIdAsync(team.TeamID, new LeagueID(leagueId));

                if (existing == null)
                {
                    Console.WriteLine("   → Añadiendo nuevo standing...");
                    var newStanding = new Standing(
                        new StandingID(1),
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
                    await _standingRepo.AddAsync(newStanding);
                }
                else
                {
                    Console.WriteLine("   → Actualizando si hay cambios...");
                    bool dirty = false;
                    if (existing.Points.Value != pts) { existing.UpdatePoints(new Points(pts)); dirty = true; }
                    if (existing.MatchesPlayed.Value != played) { existing.UpdateMatchesPlayed(new MatchesPlayed(played)); dirty = true; }
                    if (existing.Wins.Value != won) { existing.UpdateWins(new Wins(won)); dirty = true; }
                    if (existing.Draws.Value != drawn) { existing.UpdateDraws(new Draws(drawn)); dirty = true; }
                    if (existing.Losses.Value != lost) { existing.UpdateLosses(new Losses(lost)); dirty = true; }
                    if (existing.GoalDifference.Value != gd) { existing.UpdateGoalDifference(new GoalDifference(gd)); dirty = true; }

                    if (dirty)
                    {
                        Console.WriteLine("   → Guardando cambios...");
                        await _standingRepo.UpdateAsync(existing);
                    }
                    else
                    {
                        Console.WriteLine("   → Sin cambios.");
                    }
                }

                processed++;
            }

            Console.WriteLine("✅ Import completado.");
            return processed;
        }
    }
}
