using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Ports.Leagues;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Matches.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Matches.Import
{
    public class MatchImportService
    {
        private readonly MatchScraperService _scraper;
        private readonly IMatchRepository _matchRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly ILeagueRepository _leagueRepo;
        private const int LeagueId = 42; // Ajusta este ID a tu competición

        public MatchImportService(
            MatchScraperService scraper,
            IMatchRepository matchRepo,
            ITeamRepository teamRepo,
            ILeagueRepository leagueRepo)
        {
            _scraper = scraper;
            _matchRepo = matchRepo;
            _teamRepo = teamRepo;
            _leagueRepo = leagueRepo;
        }

        public async Task ImportAsync()
        {
            Console.WriteLine("🚀 Iniciando import de todos los partidos...");

            // 1) Cargar dominio de la liga
            var leagueDomain = await _leagueRepo.GetByIdAsync(new LeagueID(LeagueId));
            if (leagueDomain == null)
            {
                Console.WriteLine($"❌ Liga con ID {LeagueId} no encontrada. Abortando import.");
                return;
            }

            // 2) Scrapeo de todas las jornadas
            var scraped = await _scraper.GetAllMatchesAsync();
            Console.WriteLine($"📦 Scrapeados {scraped.Count} partidos en todas las jornadas.");

            // 3) Partidos ya existentes en BD
            var existing = (await _matchRepo.GetByLeagueIdAsync(LeagueId)).ToList();

            // 4) Procesar cada partido scrapeado
            foreach (var m in scraped)
            {
                Console.WriteLine($"– {m.LocalName} vs {m.VisitorName} (J{m.Jornada}) {m.Date:dd/MM/yyyy HH:mm}");

                // Asociar equipos por external ID
                var team1 = await _teamRepo.GetByExternalIdAsync(m.LocalId.ToString());
                var team2 = await _teamRepo.GetByExternalIdAsync(m.VisitorId.ToString());
                if (team1 == null || team2 == null)
                {
                    Console.WriteLine("   ⚠️ Uno o ambos equipos no existen en BD, omitiendo...");
                    continue;
                }

                // Buscar partido existente por equipos, fecha y jornada
                var found = existing.FirstOrDefault(x =>
                    x.Team1.TeamID.Value == team1.TeamID.Value &&
                    x.Team2.TeamID.Value == team2.TeamID.Value &&
                    x.MatchDate == m.Date &&
                    x.Jornada == m.Jornada);

                if (found == null)
                {
                    // Crear nuevo partido
                    Console.WriteLine("   → Nuevo partido, creando...");
                    var match = new Match(
                        new MatchID(1),
                        team1,
                        team2,
                        m.Date,
                        m.Status,
                        m.Location,
                        leagueDomain,
                        m.Jornada);

                    match.UpdateScore(m.Score1, m.Score2);
                    await _matchRepo.AddAsync(match);
                    Console.WriteLine("   → Añadido.");
                }
                else
                {
                    // Actualizar partido existente si hay cambios
                    var dirty = false;

                    if (found.ScoreTeam1 != m.Score1 || found.ScoreTeam2 != m.Score2)
                    {
                        found.UpdateScore(m.Score1, m.Score2);
                        dirty = true;
                    }

                    if (found.Status != m.Status)
                    {
                        found.Update(
                            found.Team1,
                            found.Team2,
                            found.MatchDate,
                            m.Status,
                            found.Location,
                            leagueDomain,
                            m.Jornada);
                        dirty = true;
                    }

                    if (dirty)
                    {
                        Console.WriteLine("   → Actualizando partido...");
                        await _matchRepo.UpdateAsync(found);
                        Console.WriteLine("   → Actualizado.");
                    }
                    else
                    {
                        Console.WriteLine("   → Sin cambios.");
                    }
                }
            }

            Console.WriteLine("✅ Import completado.");
        }
    }
}
