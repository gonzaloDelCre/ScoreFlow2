using Domain.Entities.Leagues;
using Domain.Entities.Teams;
using Domain.Ports.Leagues;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Teams.Services;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Teams.Import
{
    public class TeamImportService : ITeamImporter
    {
        private readonly TeamScraperService _scraper;
        private readonly ITeamRepository _teamRepo;
        private readonly ILeagueRepository _leagueRepo;

        public TeamImportService(
            TeamScraperService scraper,
            ITeamRepository teamRepo,
            ILeagueRepository leagueRepo)
        {
            _scraper = scraper;
            _teamRepo = teamRepo;
            _leagueRepo = leagueRepo;
        }

        public async Task ImportAsync()
        {
            Console.WriteLine("🚀 Empezando import de equipos...");
            var teams = await _scraper.GetTeamsAsync();
            Console.WriteLine($"📦 Encontrados {teams.Count} equipos.");

            foreach (var (extId, name, logo, category, stadium, club, coach) in teams)
            {
                Console.WriteLine($"– Procesando extId={extId} / {name}");

                var teamID = new TeamID(extId);
                var existing = await _teamRepo.GetByIdAsync(teamID);

                // 1) Recuperar o crear persisiblemente la liga
                var leagueName = "Mi liga"; // o lo que necesites
                var league = await _leagueRepo.GetByNameAsync(leagueName);

                if (league == null)
                {
                    var newLeague = new League(
                        new LeagueID(1),                 // 0 para que SQL la genere
                        new LeagueName(leagueName),
                        "Descripción de la liga",
                        DateTime.UtcNow
                    );
                    league = await _leagueRepo.AddAsync(newLeague);
                    Console.WriteLine("   → Liga creada.");
                }

                if (existing == null)
                {
                    Console.WriteLine("   → Nuevo, añadiendo...");
                    var newTeam = new Team(
                        teamID,
                        new TeamName(name),
                        league,
                        logo,
                        DateTime.UtcNow,
                        extId.ToString()
                    );
                    newTeam.SetCategory(category);
                    newTeam.SetStadium(stadium);
                    newTeam.SetClub(club);

                    await _teamRepo.AddAsync(newTeam);
                    Console.WriteLine("   → Equipo añadido.");
                }
                else
                {
                    Console.WriteLine("   → Ya existía, comprobando cambios...");
                    if (existing.ExternalID != extId.ToString()
                        || existing.Name.Value != name
                        || existing.Logo != logo)
                    {
                        Console.WriteLine("   → ¡Actualizando equipo!");
                        existing.Update(new TeamName(name),
                                        logo, category, club, stadium);
                        existing.SetExternalID(extId.ToString());
                        await _teamRepo.UpdateAsync(existing);
                        Console.WriteLine("   → Equipo actualizado.");
                    }
                }
            }

            Console.WriteLine("✅ Import de equipos terminado.");
        }
    }
}