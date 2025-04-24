using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Players.Services
{
    public class PlayerScraperService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "https://www.rfebm.com";

        public PlayerScraperService(HttpClient http) => _http = http;

        public async Task<List<(string Name, int Age, string Position, int Goals)>> GetPlayersByTeamIdAsync(int teamId)
        {
            var url = $"{BaseUrl}/competiciones/equipo.php?seleccion=0&id_equipo={teamId}&id=1025342";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var rows = doc.DocumentNode.SelectNodes("//table[@id='tablaJugadores']/tbody/tr[position()>1]");
            var players = new List<(string, int, string, int)>();

            foreach (var row in rows)
            {
                var cols = row.SelectNodes("td");
                string name = cols[1].InnerText.Trim();
                int.TryParse(cols[2].InnerText.Trim(), out int age);
                string pos = cols[3].InnerText.Trim();
                int.TryParse(cols[4].InnerText.Trim(), out int goals);

                players.Add((name, age, pos, goals));
            }

            return players;
        }
    }

}
