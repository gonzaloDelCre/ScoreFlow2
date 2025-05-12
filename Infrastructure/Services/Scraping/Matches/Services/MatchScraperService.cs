using Domain.Enum;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Shared;
using HtmlAgilityPack;
using Infrastructure.Persistence.Matches.Entities;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MatchScraperService> _logger;
        private const string BaseUrl = "https://www.rfebm.com";
        private const string CompetitionBase = "/competiciones/competicion.php";

        private readonly string _competitionId;
        private readonly int? _categoryId;
        private readonly int? _ambitId;

        /// <summary>
        /// Crea una instancia del servicio de scraping de partidos
        /// </summary>
        /// <param name="http">Cliente HTTP para las peticiones</param>
        /// <param name="competitionId">ID de la competición</param>
        /// <param name="categoryId">ID de categoría (opcional)</param>
        /// <param name="ambitId">ID de ámbito (opcional)</param>
        /// <param name="logger">Logger (opcional)</param>
        public MatchScraperService(
            HttpClient http,
            string competitionId,
            int? categoryId = null,
            int? ambitId = null,
            ILogger<MatchScraperService> logger = null)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _competitionId = competitionId ?? throw new ArgumentNullException(nameof(competitionId));
            _categoryId = categoryId;
            _ambitId = ambitId;
            _logger = logger;

            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            // Configurar el agente para simular un navegador
            _http.DefaultRequestHeaders.UserAgent.Clear();
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/113.0.0.0 Safari/537.36"
            );

            // Añadir otros headers si fuera necesario
            _http.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml");
            _http.DefaultRequestHeaders.AcceptLanguage.ParseAdd("es-ES,es;q=0.9");
        }

        private string BuildUrl(int? jornada = null)
        {
            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs["seleccion"] = "0";
            qs["id"] = _competitionId;

            if (jornada.HasValue)
            {
                qs["jornada"] = jornada.ToString();
            }

            if (_categoryId.HasValue)
            {
                qs["id_categoria"] = _categoryId.ToString();
            }

            if (_ambitId.HasValue)
            {
                qs["id_ambito"] = _ambitId.ToString();
            }

            return $"{BaseUrl}{CompetitionBase}?{qs}";
        }

        /// <summary>
        /// Obtiene todos los partidos de todas las jornadas para la competición configurada
        /// </summary>
        /// <returns>Lista de partidos extraídos</returns>
        public async Task<List<(int LocalId, int VisitorId, string LocalName, string VisitorName,
                              DateTime Date, string Location, int Score1, int Score2,
                              MatchStatus Status, int Jornada)>> GetAllMatchesAsync()
        {
            var result = new List<(int, int, string, string, DateTime, string, int, int, MatchStatus, int)>();

            try
            {
                // 1) Descargar página principal y leer jornadas
                _logger.LogDebug("Descargando página principal para extraer jornadas...");
                var mainHtml = await _http.GetStringAsync(BuildUrl());
                var mainDoc = new HtmlDocument();
                mainDoc.LoadHtml(mainHtml);

                var jornNodes = mainDoc.DocumentNode
                    .SelectNodes("//div[contains(@class,'lista_jornadas')]//a")
                    ?? Enumerable.Empty<HtmlNode>();

                // Filtrar solo números (descartar "TODAS", "TABLA")
                var jornadas = jornNodes
                    .Select(a => a.InnerText.Trim())
                    .Where(text => int.TryParse(text, out _))
                    .Select(int.Parse)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                if (jornadas.Count == 0)
                {
                    _logger.LogWarning("No se encontraron jornadas en la página. Verificar URL: {Url}", BuildUrl());
                    return result;
                }

                _logger.LogInformation("⚙️ Jornadas disponibles: {Jornadas}", string.Join(", ", jornadas));

                // 2) Para cada jornada, scrapear partidos
                foreach (var j in jornadas)
                {
                    _logger.LogInformation("🔍 → Jornada {Jornada}", j);

                    try
                    {
                        var journeyMatches = await GetMatchesForJourneyAsync(j);
                        result.AddRange(journeyMatches);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al procesar jornada {Jornada}: {Message}", j, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener partidos: {Message}", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Obtiene los partidos de una jornada específica
        /// </summary>
        private async Task<List<(int LocalId, int VisitorId, string LocalName, string VisitorName,
                               DateTime Date, string Location, int Score1, int Score2,
                               MatchStatus Status, int Jornada)>> GetMatchesForJourneyAsync(int jornada)
        {
            var result = new List<(int, int, string, string, DateTime, string, int, int, MatchStatus, int)>();

            var journeyUrl = BuildUrl(jornada);
            var pageHtml = await _http.GetStringAsync(journeyUrl);
            var pageDoc = new HtmlDocument();
            pageDoc.LoadHtml(pageHtml);

            var rows = pageDoc.DocumentNode
                .SelectNodes("//table[contains(@class,'table-striped')]/tbody/tr");

            if (rows == null)
            {
                _logger.LogWarning("   ⚠️ No hay filas en la tabla para la jornada {Jornada}", jornada);
                return result;
            }

            foreach (var row in rows)
            {
                try
                {
                    // Verificar que tengamos suficientes columnas
                    var cols = row.SelectNodes("td");
                    if (cols == null || cols.Count < 6)
                    {
                        _logger.LogWarning("Fila con columnas insuficientes.");
                        continue;
                    }

                    // Equipo 1 - Local
                    var href1 = cols[0].SelectSingleNode("a")?.GetAttributeValue("href", "");
                    if (string.IsNullOrEmpty(href1))
                    {
                        _logger.LogWarning("Enlace equipo local ausente.");
                        continue;
                    }

                    var q1 = HttpUtility.ParseQueryString(new Uri(BaseUrl + href1).Query)["id_equipo"];
                    if (!int.TryParse(q1, out var localId))
                    {
                        _logger.LogWarning("ID local inválido: {Id}", q1);
                        continue;
                    }

                    // Equipo 2 - Visitante
                    var href2 = cols[1].SelectSingleNode("a")?.GetAttributeValue("href", "");
                    if (string.IsNullOrEmpty(href2))
                    {
                        _logger.LogWarning("Enlace equipo visitante ausente.");
                        continue;
                    }

                    var q2 = HttpUtility.ParseQueryString(new Uri(BaseUrl + href2).Query)["id_equipo"];
                    if (!int.TryParse(q2, out var visitorId))
                    {
                        _logger.LogWarning("ID visitante inválido: {Id}", q2);
                        continue;
                    }

                    // Nombres de equipos
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
                    {
                        _logger.LogWarning("Fecha u hora faltante.");
                        continue;
                    }

                    var ds = dateDiv.InnerText.Trim();
                    var ts = timeDiv.InnerText.Trim();

                    // Usar DateTimeOffset para manejar correctamente la zona horaria
                    if (!DateTime.TryParseExact($"{ds} {ts}",
                            "dd/MM/yyyy HH:mm",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var date))
                    {
                        _logger.LogWarning("Fecha mal formateada: '{DateStr}'", $"{ds} {ts}");
                        continue;
                    }

                    // Lugar
                    var place = cols[5].SelectSingleNode("a")?.InnerText.Trim() ?? "";

                    // Estado
                    var stateTxt = row.SelectSingleNode("td[last()]//span")
                                      ?.InnerText.Trim().ToUpper() ?? "";

                    var status = DetermineMatchStatus(stateTxt);

                    // Añadir a resultados
                    result.Add((localId, visitorId, localName, visitorName,
                                date, place, s1, s2, status, jornada));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "   ⚠️ Fila omitida (J{Jornada}): {Message}", jornada, ex.Message);
                }
            }

            _logger.LogInformation("   📋 Extraídos {Count} partidos de la jornada {Jornada}", result.Count, jornada);
            return result;
        }

        /// <summary>
        /// Determina el estado del partido a partir del texto
        /// </summary>
        private MatchStatus DetermineMatchStatus(string statusText)
        {
            return statusText switch
            {
                "FINALIZADO" => MatchStatus.Finished,
                "DIRECTO" => MatchStatus.InProgress,
                "SUSPENDIDO" => MatchStatus.Scheduled,
                "APLAZADO" => MatchStatus.Aplazado,
                _ => MatchStatus.Scheduled
            };
        }
    }
}