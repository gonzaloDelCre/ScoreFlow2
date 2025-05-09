using Domain.Enum;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Shared;
using HtmlAgilityPack;
using Infrastructure.Persistence.Matches.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Services.Scraping.Matches.Services
{
    public class MatchScraperService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "https://www.rfebm.com";
        // id_categoria e id_ambito según tu competición
        private const string CompetitionBase = "/competiciones/competicion.php";
        private const int CategoryId = 200018;
        private const int AmbitId = 0;
        private readonly int _leagueId = 1025342; 

        public MatchScraperService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/113.0.0.0 Safari/537.36"
            );
        }

        private string BuildUrl(int? jornada = null)
        {
            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs["seleccion"] = "0";
            qs["id"] = _leagueId.ToString();    // <-- aquí
            if (jornada.HasValue)
                qs["jornada"] = jornada.ToString();
            // opcionalmente: qs["id_ambito"] = AmbitId.ToString();
            return $"{BaseUrl}{CompetitionBase}?{qs}";
        }


        public async Task<List<(int LocalId, int VisitorId, string LocalName, string VisitorName,
                                DateTime Date, string Location, int Score1, int Score2,
                                MatchStatus Status, int Jornada)>> GetAllMatchesAsync()
        {
            var result = new List<(int, int, string, string, DateTime, string, int, int, MatchStatus, int)>();

            // 1) Descargar página principal y leer jornadas
            var mainHtml = await _http.GetStringAsync(BuildUrl());
            var mainDoc = new HtmlDocument();
            mainDoc.LoadHtml(mainHtml);

            var jornNodes = mainDoc.DocumentNode
                .SelectNodes("//div[contains(@class,'lista_jornadas')]//a")
                ?? Enumerable.Empty<HtmlNode>();

            // Filtrar solo números (descartar “TODAS”, “TABLA”)
            var jornadas = jornNodes
                .Select(a => a.InnerText.Trim())
                .Where(text => int.TryParse(text, out _))
                .Select(int.Parse)
                .Distinct()
                .OrderBy(x => x);

            Console.WriteLine($"⚙️ Jornadas disponibles: {string.Join(", ", jornadas)}");

            // 2) Para cada jornada, scrapear partidos
            foreach (var j in jornadas)
            {
                Console.WriteLine($"🔍 → Jornada {j}");
                var pageHtml = await _http.GetStringAsync(BuildUrl(j));
                var pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(pageHtml);

                var rows = pageDoc.DocumentNode
                    .SelectNodes("//table[contains(@class,'table-striped')]/tbody/tr");
                if (rows == null)
                {
                    Console.WriteLine("   ⚠️ No hay filas en la tabla para esta jornada.");
                    continue;
                }

                foreach (var row in rows)
                {
                    try
                    {
                        var cols = row.SelectNodes("td");
                        if (cols == null || cols.Count < 6)
                            throw new InvalidOperationException("Columnas insuficientes.");

                        // Equipo 1
                        var href1 = cols[0].SelectSingleNode("a")?.GetAttributeValue("href", "");
                        if (string.IsNullOrEmpty(href1))
                            throw new InvalidOperationException("Enlace equipo local ausente.");

                        var q1 = HttpUtility.ParseQueryString(
                                    new Uri(BaseUrl + href1).Query)["id_equipo"];
                        if (!int.TryParse(q1, out var localId))
                            throw new InvalidOperationException("ID local inválido.");

                        // Equipo 2
                        var href2 = cols[1].SelectSingleNode("a")?.GetAttributeValue("href", "");
                        if (string.IsNullOrEmpty(href2))
                            throw new InvalidOperationException("Enlace equipo visitante ausente.");

                        var q2 = HttpUtility.ParseQueryString(
                                    new Uri(BaseUrl + href2).Query)["id_equipo"];
                        if (!int.TryParse(q2, out var visitorId))
                            throw new InvalidOperationException("ID visitante inválido.");

                        // Nombres
                        var names = cols[2].InnerText.Split('-', 2);
                        var localName = names[0].Trim();
                        var visitorName = names.Length > 1 ? names[1].Trim() : "";

                        // Marcador (puede estar vacío)
                        var scores = cols[3].InnerText.Trim()
                                             .Split('-', StringSplitOptions.RemoveEmptyEntries);
                        int.TryParse(scores.ElementAtOrDefault(0), out var s1);
                        int.TryParse(scores.ElementAtOrDefault(1), out var s2);

                        // Fecha y hora
                        var dateDiv = cols[4].SelectSingleNode(".//div[@class='negrita']");
                        var timeDiv = cols[4].SelectSingleNode("div[2]");
                        if (dateDiv == null || timeDiv == null)
                            throw new InvalidOperationException("Fecha u hora faltante.");

                        var ds = dateDiv.InnerText.Trim();
                        var ts = timeDiv.InnerText.Trim();
                        if (!DateTime.TryParseExact($"{ds} {ts}",
                                "dd/MM/yyyy HH:mm",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var date))
                        {
                            throw new InvalidOperationException($"Fecha mal formateada: '{ds} {ts}'");
                        }

                        // Lugar
                        var place = cols[5].SelectSingleNode("a")?.InnerText.Trim() ?? "";

                        // Estado
                        var stateTxt = row.SelectSingleNode("td[last()]//span")
                                          ?.InnerText.Trim().ToUpper() ?? "";
                        var status = stateTxt switch
                        {
                            "FINALIZADO" => MatchStatus.Finished,
                            "DIRECTO" => MatchStatus.InProgress,
                            _ => MatchStatus.Scheduled
                        };

                        result.Add((localId, visitorId, localName, visitorName,
                                    date, place, s1, s2, status, j));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   ⚠️ Fila omitida (J{j}): {ex.Message}");
                    }
                }
            }

            return result;
        }
    }
}
