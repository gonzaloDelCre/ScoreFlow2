using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Teams.Services;

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

            foreach (var (id, name, logo, category, stadium, club, coach) in teams)
            {
                Console.WriteLine($"– Procesando equipo {id} / {name}");
                var tid = new TeamID(id);
                var existing = await _repo.GetByIdAsync(tid);

                if (existing == null)
                {
                    Console.WriteLine("   → Nuevo, añadiendo...");
                    var newTeam = new Team(tid, new TeamName(name), DateTime.UtcNow, logo);
                    newTeam.SetCategory(category != "Categoría no disponible" ? category : null);
                    newTeam.SetStadium(stadium != "Estadio no disponible" ? stadium : null);
                    newTeam.SetClub(club != "Club no disponible" ? club : null); // ⬅️ nuevo campo
                    await _repo.AddAsync(newTeam);
                }
                else
                {
                    Console.WriteLine("   → Ya existía, comprobando cambios...");
                    if (existing.Name.Value != name || existing.Logo != logo)
                    {
                        Console.WriteLine("   → ¡Datos cambiados! Actualizando...");
                        existing.Update(new TeamName(name), logo);
                        existing.SetCategory(category != "Categoría no disponible" ? category : null);
                        existing.SetStadium(stadium != "Estadio no disponible" ? stadium : null);
                        existing.SetClub(club != "Club no disponible" ? club : null); // ⬅️ actualización del nuevo campo
                        await _repo.UpdateAsync(existing);
                    }
                }
            }

            Console.WriteLine("✅ Import terminado.");
        }
    }

}
