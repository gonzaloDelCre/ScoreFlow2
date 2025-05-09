using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Shared;
using HtmlAgilityPack;
using Infrastructure.Services.Scraping.Leagues.Services;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Leagues.Import
{
    public class LeagueImportService
    {
        private readonly LeagueScraperService _scraper;
        private readonly ILeagueRepository _repo;

        public LeagueImportService(LeagueScraperService scraper, ILeagueRepository repo)
        {
            _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Importa todas las ligas de una competición específica
        /// </summary>
        /// <param name="competitionId">ID de la competición (parámetro 'id' en la URL)</param>
        public async Task ImportAsync(string competitionId)
        {
            Console.WriteLine($"🚀 Empezando import de ligas para competición {competitionId}...");
            var summaries = await _scraper.GetAvailableLeaguesAsync(competitionId);
            Console.WriteLine($"📦 Encontradas {summaries.Count} ligas.");

            foreach (var summary in summaries)
            {
                Console.WriteLine($"→ Procesando {summary.CategoryName}...");
                var metadata = await _scraper.GetLeagueDetailsAsync(summary);
                await ImportLeagueFromMetadataAsync(metadata);
            }

            Console.WriteLine("✅ Import de ligas completado.");
        }

        private async Task ImportLeagueFromMetadataAsync(LeagueMetadata metadata)
        {
            var leagueName = $"{metadata.CategoryName} - {metadata.CompetitionName} ({metadata.SeasonName})";
            var description = $"Federación: {metadata.TerritorialName}, CompID={metadata.CompetitionId}, FaseID={metadata.PhaseId}";

            var existing = await _repo.GetByNameAsync(leagueName);
            if (existing == null)
            {
                Console.WriteLine("  → Nueva liga. Insertando...");
                var league = new League(
                    new LeagueID(1),
                    new LeagueName(leagueName),
                    description,
                    DateTime.UtcNow
                );
                await _repo.AddAsync(league);
            }
            else
            {
                Console.WriteLine("  → Ya existe. Verificando cambios...");
                var dirty = false;

                if (existing.Name.Value != leagueName)
                {
                    existing.UpdateName(new LeagueName(leagueName));
                    dirty = true;
                }
                if (existing.Description != description)
                {
                    existing.UpdateDescription(description);
                    dirty = true;
                }

                if (dirty)
                {
                    Console.WriteLine("  → Cambios detectados. Actualizando...");
                    await _repo.UpdateAsync(existing);
                }
                else
                {
                    Console.WriteLine("  → Sin cambios.");
                }
            }
        }
    }
}
