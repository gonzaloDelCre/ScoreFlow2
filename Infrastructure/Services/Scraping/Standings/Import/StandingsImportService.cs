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
        private readonly LeagueID _leagueId;
        private readonly int _competitionId;

        public StandingsImportService(
            StandingsScraperService scraper,
            ITeamRepository teamRepo,
            IStandingRepository standingRepo,
            IOptions<StandingsImportOptions> options)
        {
            _scraper = scraper;
            _teamRepo = teamRepo;
            _standingRepo = standingRepo;
            _competitionId = options.Value.CompetitionId;
            _leagueId = new LeagueID(options.Value.LeagueId);
        }

        public async Task ImportAsync()
        {
            Console.WriteLine("🚀 Iniciando import de clasificación...");
            var scraped = await _scraper.GetStandingsAsync(_competitionId, _leagueId.Value);
            Console.WriteLine($"📦 Filas obtenidas: {scraped.Count}");

            // Resto del código igual...
            foreach (var (extId, pts, played, won, drawn, lost, gf, ga, gd) in scraped)
            {
                var ext = extId.ToString();
                Console.WriteLine($"– Procesando equipo ExternalID={ext}...");

                var team = await _teamRepo.GetByExternalIdAsync(ext);
                if (team == null)
                {
                    Console.WriteLine($"   ⚠️ Equipo {ext} no encontrado, saltando.");
                    continue;
                }

                // 2) Buscar standing existente
                var existing = await _standingRepo
                    .GetByTeamIdAndLeagueIdAsync(team.TeamID, _leagueId);

                if (existing == null)
                {
                    Console.WriteLine("   → Nuevo standing, añadiendo...");
                    var newStanding = new Standing(
                        new StandingID(0),
                        _leagueId,
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
                    Console.WriteLine("   → Ya existía, comprobando diferencias...");
                    bool dirty = false;

                    if (existing.Points.Value != pts) { existing.UpdatePoints(new Points(pts)); dirty = true; }
                    if (existing.MatchesPlayed.Value != played) { existing.UpdateMatchesPlayed(new MatchesPlayed(played)); dirty = true; }
                    if (existing.Wins.Value != won) { existing.UpdateWins(new Wins(won)); dirty = true; }
                    if (existing.Draws.Value != drawn) { existing.UpdateDraws(new Draws(drawn)); dirty = true; }
                    if (existing.Losses.Value != lost) { existing.UpdateLosses(new Losses(lost)); dirty = true; }
                    if (existing.GoalDifference.Value != gd) { existing.UpdateGoalDifference(new GoalDifference(gd)); dirty = true; }

                    if (dirty)
                    {
                        Console.WriteLine("   → Actualizando standing...");
                        await _standingRepo.UpdateAsync(existing);
                    }
                    else
                    {
                        Console.WriteLine("   → Sin cambios.");
                    }
                }
            }

            Console.WriteLine("✅ Import de clasificación completado.");
        }
    }
}
