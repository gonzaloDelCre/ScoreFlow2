using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Services.Scraping.Standings.Services
{
    public class StandingsScraperService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "https://www.rfebm.com";

        public StandingsScraperService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/113.0.0.0 Safari/537.36"
            );
        }

        /// <summary>
        /// Devuelve una lista de tuplas con:
        /// (teamExternalId, points, played, won, drawn, lost, goalsFor, goalsAgainst, goalDiff)
        /// </summary>
        public async Task<List<(int TeamExternalId, int Points, int Played, int Won, int Drawn, int Lost, int GoalsFor, int GoalsAgainst, int GoalDiff)>>
            GetStandingsAsync(int competitionId, int leagueId)
        {
            var url = $"{BaseUrl}/competiciones/clasificacion.php?seleccion=0&id={competitionId}&id_ambito={leagueId}";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var result = new List<(int, int, int, int, int, int, int, int, int)>();
            var rows = doc.DocumentNode
                .SelectNodes("//table[contains(@class,'clasificacion')]/tbody/tr");
            if (rows == null) return result;

            foreach (var row in rows)
            {
                var cols = row.SelectNodes("td");
                if (cols == null || cols.Count < 13)
                    continue;

                // 1) Extraer ExternalID
                var link = cols[1].SelectSingleNode(".//a")?.GetAttributeValue("href", "");
                var qs = HttpUtility.ParseQueryString(new Uri($"{BaseUrl}/{link}").Query);
                if (!int.TryParse(qs["id_equipo"], out var extId))
                    continue;

                // 2) Parsear cada columna por separado
                if (!int.TryParse(cols[5].InnerText.Trim(), out var pts)) continue;
                if (!int.TryParse(cols[6].InnerText.Trim(), out var played)) continue;
                if (!int.TryParse(cols[7].InnerText.Trim(), out var won)) continue;
                if (!int.TryParse(cols[8].InnerText.Trim(), out var drawn)) continue;
                if (!int.TryParse(cols[9].InnerText.Trim(), out var lost)) continue;
                if (!int.TryParse(cols[10].InnerText.Trim(), out var gf)) continue;
                if (!int.TryParse(cols[11].InnerText.Trim(), out var ga)) continue;
                if (!int.TryParse(cols[12].InnerText.Trim(), out var gd)) continue;

                // 3) Añadir al resultado
                result.Add((extId, pts, played, won, drawn, lost, gf, ga, gd));
            }

            return result;
        }
    }
}
