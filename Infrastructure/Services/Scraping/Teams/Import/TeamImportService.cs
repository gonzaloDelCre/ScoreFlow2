using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Teams.Services;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Teams.Import
{
    public class TeamImportService
    {
        private readonly TeamScraperService _scraper;
        private readonly ITeamRepository _repo;

        public TeamImportService(TeamScraperService scraper, ITeamRepository repo)
        {
            _scraper = scraper;
            _repo = repo;
        }

        public async Task ImportAsync()
        {
            Console.WriteLine("🚀 Empezando import de equipos...");
            var scraped = await _scraper.GetTeamsAsync();
            Console.WriteLine($"📦 Encontrados {scraped.Count} equipos.");

            foreach (var (extId, name, logo, category, stadium, club, coach) in scraped)
            {
                Console.WriteLine($"– Procesando ExternalID={extId} / {name}");
                var externalId = extId.ToString();

                // Ahora buscamos por ExternalID
                var existing = await _repo.GetByExternalIdAsync(externalId);

                if (existing == null)
                {
                    Console.WriteLine("   → Nuevo, añadiendo...");
                    // Creamos un Team con ID provisional 0; el repositorio asignará el real
                    var team = new Team(
                        new TeamID(0),
                        new TeamName(name),
                        new LogoUrl(logo),
                        DateTime.UtcNow,
                        category,
                        club,
                        stadium,
                        externalId
                    );

                    await _repo.AddAsync(team);
                    Console.WriteLine("   → Añadido.");
                }
                else
                {
                    Console.WriteLine("   → Ya existía, comprobando cambios...");
                    var dirty = false;

                    if (existing.Name.Value != name) { existing.UpdateName(new TeamName(name)); dirty = true; }
                    if (existing.Logo.Value != logo) { existing.UpdateLogo(new LogoUrl(logo)); dirty = true; }
                    if (existing.Category != category) { existing.SetCategory(category); dirty = true; }
                    if (existing.Club != club) { existing.SetClub(club); dirty = true; }
                    if (existing.Stadium != stadium) { existing.SetStadium(stadium); dirty = true; }
                    if (existing.ExternalID != externalId)
                    {
                        existing.SetExternalID(externalId);
                        dirty = true;
                    }

                    if (dirty)
                    {
                        Console.WriteLine("   → ¡Actualizando datos!");
                        await _repo.UpdateAsync(existing);
                        Console.WriteLine("   → Actualizado.");
                    }
                    else
                    {
                        Console.WriteLine("   → Sin cambios.");
                    }
                }
            }

            Console.WriteLine("✅ Import de equipos terminado.");
        }
    }
}
