using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<StandingsScraperService> _logger;
        private const string BaseUrl = "https://www.rfebm.com";
        private const int RequestTimeoutSeconds = 30;

        public StandingsScraperService(
            HttpClient http,
            ILogger<StandingsScraperService> logger)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Configure HTTP client
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/113.0.0.0 Safari/537.36"
            );
            _http.Timeout = TimeSpan.FromSeconds(RequestTimeoutSeconds);
        }

        private int ParseLeadingInt(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new ArgumentException("Input string cannot be null or empty", nameof(raw));

            // Ahora aceptamos signo opcional
            var m = Regex.Match(raw.Trim(), @"^-?\d+");
            if (!m.Success)
                throw new FormatException($"No leading integer in '{raw}'");

            var val = int.Parse(m.Value);
            // Si es negativo, devolvemos 0
            return val < 0 ? 0 : val;
        }


        /// <summary>
        /// Devuelve una lista de tuplas:
        /// (ExternalId, Points, Played, Won, Drawn, Lost, GoalsFor, GoalsAgainst, GoalDiff)
        /// </summary>
        public async Task<List<(int ExternalId, int Points, int Played, int Won, int Drawn, int Lost, int GoalsFor, int GoalsAgainst, int GoalDiff)>>
            GetStandingsAsync(int competitionId)
        {
            if (competitionId <= 0)
                throw new ArgumentException("El ID de competición debe ser mayor que cero", nameof(competitionId));

            var url = $"{BaseUrl}/competiciones/clasificacion.php?seleccion=0&id={competitionId}";
            _logger.LogInformation("Obteniendo clasificación de URL: {Url}", url);

            string html;
            try
            {
                html = await _http.GetStringAsync(url);
                if (string.IsNullOrWhiteSpace(html))
                {
                    _logger.LogWarning("La respuesta HTTP está vacía para competitionId={CompetitionId}", competitionId);
                    return new List<(int, int, int, int, int, int, int, int, int)>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error HTTP al obtener clasificación para competitionId={CompetitionId}: {Message}",
                    competitionId, ex.Message);
                throw new ApplicationException($"Error al acceder a la clasificación: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError(ex, "Timeout al obtener clasificación para competitionId={CompetitionId}", competitionId);
                throw new ApplicationException("La solicitud de clasificación ha excedido el tiempo de espera", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desconocido al obtener clasificación para competitionId={CompetitionId}", competitionId);
                throw;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var result = new List<(int, int, int, int, int, int, int, int, int)>();

            var rows = doc.DocumentNode.SelectNodes("//table[contains(@class,'clasificacion')]/tbody/tr");
            if (rows == null)
            {
                _logger.LogWarning("No se encontraron filas de clasificación para competitionId={CompetitionId}", competitionId);
                return result;
            }

            _logger.LogInformation("Encontradas {RowCount} filas en la tabla de clasificación", rows.Count);
            int processedRows = 0;
            int skippedRows = 0;

            foreach (var row in rows)
            {
                var cols = row.SelectNodes("td");
                if (cols == null || cols.Count < 13)
                {
                    _logger.LogDebug("Fila sin suficientes columnas, saltando");
                    skippedRows++;
                    continue;
                }

                // Extraer ExternalId
                var link = cols[1].SelectSingleNode(".//a") ?? cols[3].SelectSingleNode(".//a");
                if (link == null)
                {
                    _logger.LogDebug("Fila sin enlace al equipo, saltando");
                    skippedRows++;
                    continue;
                }

                var href = link.GetAttributeValue("href", "");
                if (string.IsNullOrWhiteSpace(href))
                {
                    _logger.LogDebug("Enlace sin atributo href, saltando");
                    skippedRows++;
                    continue;
                }

                try
                {
                    var fullUrl = $"{BaseUrl}/{href}";
                    var uri = new Uri(fullUrl);
                    var qs = System.Web.HttpUtility.ParseQueryString(uri.Query);

                    if (!int.TryParse(qs["id_equipo"], out var extId))
                    {
                        _logger.LogDebug("No se pudo extraer id_equipo del enlace {Href}, saltando", href);
                        skippedRows++;
                        continue;
                    }

                    // Parsear los valores numéricos de cada celda
                    var pts = ParseLeadingInt(cols[5].InnerText);
                    var played = ParseLeadingInt(cols[6].InnerText);
                    var won = ParseLeadingInt(cols[7].InnerText);
                    var drawn = ParseLeadingInt(cols[8].InnerText);
                    var lost = ParseLeadingInt(cols[9].InnerText);
                    var gf = ParseLeadingInt(cols[10].InnerText);
                    var ga = ParseLeadingInt(cols[11].InnerText);
                    var gd = ParseLeadingInt(cols[12].InnerText);

                    // Validación adicional de los datos
                    if (played != won + drawn + lost)
                    {
                        _logger.LogWarning("Inconsistencia en partidos para equipo {ExternalId}: jugados={Played}, ganados={Won}, empatados={Drawn}, perdidos={Lost}",
                            extId, played, won, drawn, lost);
                    }

                    if (gd != gf - ga)
                    {
                        _logger.LogWarning("Inconsistencia en goles para equipo {ExternalId}: diferencia={GD}, a favor={GF}, en contra={GA}",
                            extId, gd, gf, ga);
                    }

                    result.Add((extId, pts, played, won, drawn, lost, gf, ga, gd));
                    processedRows++;
                }
                catch (FormatException ex)
                {
                    _logger.LogWarning(ex, "Error de formato al parsear datos de la fila");
                    skippedRows++;
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error inesperado al procesar fila de clasificación");
                    skippedRows++;
                    continue;
                }
            }

            _logger.LogInformation("Procesamiento completado: {ProcessedRows} filas procesadas, {SkippedRows} filas saltadas",
                processedRows, skippedRows);

            return result;
        }
    }
}
