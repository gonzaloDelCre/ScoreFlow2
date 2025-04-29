using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Players.Services
{
    public class PlayerScraperService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "https://www.rfebm.com";

        public PlayerScraperService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/113.0.0.0 Safari/537.36"
            );
        }

        /// <summary>
        /// Scrapea la plantilla de jugadores de un equipo, dado su ExternalId.
        /// </summary>
        public async Task<List<(string Name, int Age, string Position, int Goals, int TeamExternalId, string PhotoUrl)>>
            GetPlayersByTeamExternalIdAsync(int teamExternalId)
        {
            try
            {
                var url = $"{BaseUrl}/competiciones/equipo.php?seleccion=0&id_equipo={teamExternalId}&id=1025342";
                var html = await _http.GetStringAsync(url);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var players = new List<(string, int, string, int, int, string)>();

                var table = doc.DocumentNode
                    .SelectSingleNode("//div[@id='plantilla']//table[contains(@class,'plantilla')]");
                if (table == null)
                {
                    Console.WriteLine($"❌ No se encontró la tabla de plantilla para el equipo {teamExternalId}.");
                    return players;
                }

                var rows = table.SelectNodes(".//tbody/tr[contains(@class,'baja_')]");
                if (rows == null)
                {
                    Console.WriteLine($"⚠️ No hay filas de jugadores para el equipo {teamExternalId}.");
                    return players;
                }

                foreach (var row in rows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols == null || cols.Count < 5)
                    {
                        Console.WriteLine($"⚠️ Fila inválida en plantilla del equipo {teamExternalId}, saltando...");
                        continue;
                    }

                    try
                    {
                        var name = cols[1].InnerText.Trim();
                        if (string.IsNullOrEmpty(name))
                            continue;

                        var position = cols[2].InnerText.Trim();
                        var ageText = cols[3].InnerText.Trim().Split(' ')[0];
                        var age = int.TryParse(ageText, out var a) ? a : 0;
                        var goalsText = cols[4].InnerText.Trim();
                        var goals = int.TryParse(goalsText, out var g) ? g : 0;

                        var styleAttr = cols[0].GetAttributeValue("style", "");
                        string photoUrl = ExtractPhotoUrlFromStyle(styleAttr);

                        players.Add((name, age, position, goals, teamExternalId, photoUrl));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚡ Error procesando fila de plantilla (team {teamExternalId}): {ex.Message}");
                    }
                }

                Console.WriteLine($"🚀 Jugadores encontrados para teamExtId={teamExternalId}: {players.Count}");
                return players;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al obtener la plantilla de jugadores para el equipo {teamExternalId}: {ex.Message}");
                return new List<(string, int, string, int, int, string)>();
            }
        }

        /// <summary>
        /// Extrae la URL de la foto desde el atributo style del primer td.
        /// </summary>
        private string ExtractPhotoUrlFromStyle(string styleAttr)
        {
            if (string.IsNullOrEmpty(styleAttr))
                return null;

            var start = styleAttr.IndexOf("url(");
            var end = styleAttr.IndexOf(")", start + 4);

            if (start >= 0 && end > start)
            {
                var urlPart = styleAttr.Substring(start + 4, end - start - 4)
                                       .Trim('\'', '"');

                if (urlPart.StartsWith("//"))
                    return $"https:{urlPart}";
                else if (urlPart.StartsWith("/"))
                    return $"https://balonmano.isquad.es{urlPart}";
                else
                    return urlPart;
            }

            return null;
        }
    }
}
