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
            var teams = await _scraper.GetTeamsAsync();
            Console.WriteLine($"📦 Encontrados {teams.Count} equipos.");

            foreach (var (extId, name, logo, category, stadium, club, coach) in teams)
            {
                Console.WriteLine($"– Procesando extId={extId} / {name}");
                var tid = new TeamID(extId);
                var existing = await _repo.GetByIdAsync(tid);

                if (existing == null)
                {
                    Console.WriteLine("   → Nuevo, añadiendo...");
                    var newTeam = new Team(
                        new TeamID(0),
                        new TeamName(name),
                        DateTime.UtcNow,
                        logo,
                        extId.ToString()
                    );
                    newTeam.SetCategory(category);
                    newTeam.SetStadium(stadium);
                    newTeam.SetClub(club);
                    await _repo.AddAsync(newTeam);
                    Console.WriteLine("   → Añadido.");
                }
                else
                {
                    Console.WriteLine("   → Ya existía, comprobando cambios...");
                    if (existing.ExternalID != extId.ToString()
                        || existing.Name.Value != name
                        || existing.Logo != logo)
                    {
                        Console.WriteLine("   → ¡Actualizando datos!");
                        existing.Update(new TeamName(name), logo, category, club, stadium);
                        existing.SetExternalID(extId.ToString());
                        await _repo.UpdateAsync(existing);
                        Console.WriteLine("   → Actualizado.");
                    }
                }
            }

            Console.WriteLine("✅ Import de equipos terminado.");
        }
    }
}
