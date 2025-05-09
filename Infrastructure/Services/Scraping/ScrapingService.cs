using Infrastructure.Services.Scraping.Leagues.Services;
using Infrastructure.Services.Scraping.Players.Services;
using Infrastructure.Services.Scraping.Teams.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping
{
    public class ScrapingService
    {
        public TeamScraperService Teams { get; }
        public PlayerScraperService Players { get; }
        public LeagueScraperService Leagues { get; }

        public ScrapingService(HttpClient client)
        {
            Teams = new TeamScraperService(client);
            Players = new PlayerScraperService(client);
            Leagues = new LeagueScraperService(client);
        }
    }

}
