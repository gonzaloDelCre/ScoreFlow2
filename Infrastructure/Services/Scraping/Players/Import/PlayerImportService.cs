using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Enum;
using Domain.Ports.Players;
using Domain.Shared;
using Infrastructure.Services.Scraping.Players.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Players.Import
{
    public class PlayerImportService
    {
        private readonly PlayerScraperService _scraper;
        private readonly IPlayerRepository _repo;

        public PlayerImportService(PlayerScraperService scraper, IPlayerRepository repo)
        {
            _scraper = scraper;
            _repo = repo;
        }

        public async Task ImportByTeamIdAsync(int teamId)
        {
            var scrapedPlayers = await _scraper.GetPlayersByTeamIdAsync(teamId);
            foreach (var (name, age, position, goals) in scrapedPlayers)
            {
                var exists = await _repo.GetByNameAsync(name);
                if (exists != null) continue;

                var player = new Player(
                    new PlayerID(0),
                    new PlayerName(name),
                    Enum.TryParse<PlayerPosition>(position, out var pos) ? pos : PlayerPosition.LD,
                    new PlayerAge(age),
                    goals,
                    null,
                    DateTime.UtcNow,
                    new List<TeamPlayer>()
                );

                await _repo.AddAsync(player);
            }
        }
    }

}
