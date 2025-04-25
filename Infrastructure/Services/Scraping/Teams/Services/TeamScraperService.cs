using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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

        public async Task<List<(int Id, string Name, string Logo, string Category, string Stadium, string Club, string Coach)>> GetTeamsAsync()
        {
            var url = $"{BaseUrl}/competiciones/competicion.php?seleccion=0&id=1025342";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var logoDict = new Dictionary<int, string>();
            var cabeceraLogoNodes = doc.DocumentNode
                .SelectNodes("//div[contains(@class,'div_escudos_cabecera')]/a");

            if (cabeceraLogoNodes != null)
            {
                foreach (var node in cabeceraLogoNodes)
                {
                    var href = node.GetAttributeValue("href", "");
                    var qs = HttpUtility.ParseQueryString(new Uri($"{BaseUrl}/{href}").Query);
                    if (int.TryParse(qs["id_equipo"], out var id))
                    {
                        var img = node.SelectSingleNode(".//img");
                        if (img != null)
                        {
                            var src = img.GetAttributeValue("src", "");
                            logoDict[id] = src.StartsWith("http") ? src : $"{BaseUrl}/{src.TrimStart('/')}";
                        }
                    }
                }
            }

            var teams = new List<(int Id, string Name, string Logo, string Category, string Stadium, string Club, string Coach)>();
            var nameNodes = doc.DocumentNode
                .SelectNodes("//td[@class='p-t-20']/a");

            if (nameNodes == null)
            {
                Console.WriteLine("❌ No se encontraron nodos de nombres. Revisa el XPath.");
                return teams;
            }

            foreach (var node in nameNodes)
            {
                var href = node.GetAttributeValue("href", "");
                var qs = HttpUtility.ParseQueryString(new Uri($"{BaseUrl}/{href}").Query);
                if (int.TryParse(qs["id_equipo"], out var id))
                {
                    var name = node.InnerText.Trim();
                    logoDict.TryGetValue(id, out var logo);

                    var details = await GetTeamDetailsAsync(id);

                    teams.Add((id, name, logo ?? "", details.Category, details.Stadium, details.Club, null));
                }
            }

            return teams;
        }

        private async Task<(string Category, string Club, string Responsible, string Stadium)> GetTeamDetailsAsync(int id)
        {
            var url = $"{BaseUrl}/competiciones/equipo.php?seleccion=0&id_equipo={id}&id=1025342";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var categoryNode = doc.DocumentNode.SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'CATEGORÍA')]/following-sibling::div");
            var clubNode = doc.DocumentNode.SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'CLUB AL QUE PERTENECE')]/following-sibling::div");
            var responsibleNode = doc.DocumentNode.SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'RESPONSABLE')]/following-sibling::div");
            var stadiumNode = doc.DocumentNode.SelectSingleNode("//div[@class='col-md-4 cajadatos']/div[contains(text(),'PABELLÓN')]/following-sibling::div");

            string category = categoryNode?.InnerText.Trim() ?? "Categoría no disponible";
            string club = clubNode != null
                ? LimpiarNombreClub(clubNode.InnerText)
                : "Club no disponible";
            string responsible = responsibleNode?.InnerText.Trim() ?? "Responsable no disponible";
            string stadium = stadiumNode?.InnerText.Trim() ?? "Estadio no disponible";

            return (category, club, responsible, stadium);
        }
        private string LimpiarNombreClub(string raw)
        {
            var limpio = HttpUtility.HtmlDecode(raw)
                .Replace('\u00a0', ' ') 
                .Trim();

            var index = limpio.IndexOf("Datos del Club", StringComparison.OrdinalIgnoreCase);
            if (index > 0)
            {
                limpio = limpio.Substring(0, index).Trim();
            }

            return limpio;
        }


    }
}
