using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;

namespace Infrastructure.Services.Scraping.Teams.Services
{
    public class TeamScraperService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "https://www.rfebm.com";

        public TeamScraperService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/113.0.0.0 Safari/537.36"
            );
        }

        /// <summary>
        /// Scrapea la lista de equipos desde la tabla de partidos,
        /// extrayendo su ExternalID y nombre desde la 3ª columna.
        /// </summary>
        public async Task<List<(int ExternalId, string Name, string Logo, string Category, string Stadium, string Club, string Coach)>> GetTeamsAsync()
        {
            var url = $"{BaseUrl}/competiciones/competicion.php?seleccion=0&id=1025342";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 1) Logos como antes
            var logoDict = new Dictionary<int, string>();
            var logoNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'div_escudos_cabecera')]/a");
            if (logoNodes != null)
            {
                foreach (var node in logoNodes)
                {
                    var qs = HttpUtility.ParseQueryString(new Uri($"{BaseUrl}/{node.GetAttributeValue("href", "")}").Query);
                    if (int.TryParse(qs["id_equipo"], out var id))
                    {
                        var img = node.SelectSingleNode(".//img")?.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(img))
                            logoDict[id] = img.StartsWith("http") ? img : $"{BaseUrl}/{img.TrimStart('/')}";
                    }
                }
            }

            var teams = new List<(int, string, string, string, string, string, string)>();
            var seen = new HashSet<int>();

            // 2) Seleccionamos los anchors de la 3ª columna de cada fila
            var nameAnchors = doc.DocumentNode
                .SelectNodes("//table[contains(@class,'table-striped')]/tbody/tr/td[3]/a");

            if (nameAnchors == null)
                return teams;

            foreach (var anchor in nameAnchors)
            {
                // Extraer ExternalId
                var qs = HttpUtility.ParseQueryString(new Uri($"{BaseUrl}/{anchor.GetAttributeValue("href", "")}").Query);
                if (!int.TryParse(qs["id_equipo"], out var extId))
                    continue;

                if (!seen.Add(extId))
                    continue; // ya procesado

                // Nombre limpio
                var name = anchor.InnerText.Trim();
                if (string.IsNullOrEmpty(name))
                    continue;

                // Logo y detalles
                logoDict.TryGetValue(extId, out var logo);
                var (cat, club, resp, std) = await GetTeamDetailsAsync(extId);

                teams.Add((extId, name, logo ?? "", cat, std, club, resp));
            }

            Console.WriteLine($"🚀 Equipos encontrados: {teams.Count}");
            return teams;
        }

        private async Task<(string Category, string Club, string Responsible, string Stadium)> GetTeamDetailsAsync(int id)
        {
            var url = $"{BaseUrl}/competiciones/equipo.php?seleccion=0&id_equipo={id}&id=1025342";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            string category = doc.DocumentNode
                .SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'CATEGORÍA')]/following-sibling::div")
                ?.InnerText.Trim() ?? "Categoría no disponible";

            string club = doc.DocumentNode
                .SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'CLUB AL QUE PERTENECE')]/following-sibling::div")
                ?.InnerText.Trim() ?? "Club no disponible";

            string responsible = doc.DocumentNode
                .SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'RESPONSABLE')]/following-sibling::div")
                ?.InnerText.Trim() ?? "Responsable no disponible";

            string stadium = doc.DocumentNode
                .SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'PABELLÓN')]/following-sibling::div")
                ?.InnerText.Trim() ?? "Estadio no disponible";

            return (category, club, responsible, stadium);
        }
    }
}
