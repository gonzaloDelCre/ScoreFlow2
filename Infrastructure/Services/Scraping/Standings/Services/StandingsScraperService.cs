using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        private int ParseLeadingInt(string raw)
        {
            var m = Regex.Match(raw.Trim(), @"^\d+");
            if (!m.Success)
                throw new FormatException($"No leading integer in '{raw}'");
            return int.Parse(m.Value);
        }

        /// <summary>
        ///  Devuelve una lista de tuplas:
        ///  (ExternalId, Points, Played, Won, Drawn, Lost, GoalsFor, GoalsAgainst, GoalDiff)
        /// </summary>
        public async Task<List<(int ExternalId, int Points, int Played, int Won, int Drawn, int Lost, int GoalsFor, int GoalsAgainst, int GoalDiff)>>
            GetStandingsAsync(int competitionId)
        {
            var url = $"{BaseUrl}/competiciones/clasificacion.php?seleccion=0&id={competitionId}";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var result = new List<(int, int, int, int, int, int, int, int, int)>();
            var rows = doc.DocumentNode.SelectNodes("//table[contains(@class,'clasificacion')]/tbody/tr");
            if (rows == null)
                return result;

            foreach (var row in rows)
            {
                var cols = row.SelectNodes("td");
                if (cols == null || cols.Count < 13)
                    continue;

                // 1) Extraer ExternalId
                var link = cols[1].SelectSingleNode(".//a")
                           ?? cols[3].SelectSingleNode(".//a");
                if (link == null)
                    continue;

                var href = link.GetAttributeValue("href", "");
                var qs = System.Web.HttpUtility.ParseQueryString(new Uri($"{BaseUrl}/{href}").Query);
                if (!int.TryParse(qs["id_equipo"], out var extId))
                    continue;

                try
                {
                    // 2) Parsear solo el número líder de cada celda
                    var pts = ParseLeadingInt(cols[5].InnerText);
                    var played = ParseLeadingInt(cols[6].InnerText);
                    var won = ParseLeadingInt(cols[7].InnerText);
                    var drawn = ParseLeadingInt(cols[8].InnerText);
                    var lost = ParseLeadingInt(cols[9].InnerText);
                    var gf = ParseLeadingInt(cols[10].InnerText);
                    var ga = ParseLeadingInt(cols[11].InnerText);
                    var gd = ParseLeadingInt(cols[12].InnerText);

                    result.Add((extId, pts, played, won, drawn, lost, gf, ga, gd));
                }
                catch (FormatException)
                {
                    // Si alguna celda no empieza con dígitos, saltamos la fila
                    continue;
                }
            }

            return result;
        }
    }
}
