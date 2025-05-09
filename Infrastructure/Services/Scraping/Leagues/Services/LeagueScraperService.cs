using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Leagues.Services
{
    public class LeagueScraperService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "https://www.rfebm.com";
        private const string CompetitionUrl = BaseUrl + "/competiciones/competicion.php";
        private const string ClassificationUrl = BaseUrl + "/competiciones/clasificacion.php";

        public LeagueScraperService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/113.0.0.0 Safari/537.36"
            );
        }

        /// <summary>
        /// Obtiene la lista de ligas (categorías) para una competición específica
        /// </summary>
        public async Task<List<LeagueSummary>> GetAvailableLeaguesAsync(string competitionId)
        {
            // Descarga la página de cabecera (contiene selects)
            var html = await _http.GetStringAsync($"{CompetitionUrl}?seleccion=0&id={competitionId}&id_ambito=0");
            var doc = new HtmlDocument(); doc.LoadHtml(html);

            // Extrae temporada actual
            var seasonNode = doc.DocumentNode.SelectSingleNode("//select[@id='temporadas']/option[@selected]");
            var seasonId = seasonNode?.GetAttributeValue("value", "") ?? "";
            var seasonName = seasonNode?.InnerText.Trim() ?? "Temporada actual";

            // Extrae categorías disponibles
            var selectCat = doc.DocumentNode
                .SelectSingleNode("//div[contains(@class,'div_select_categoria')]//select[@id='categorias']");
            if (selectCat == null)
                throw new InvalidOperationException("No encontré el <select id='categorias'> en competicion.php");

            var optionNodes = selectCat.SelectNodes(".//option");
            if (optionNodes == null)
                throw new InvalidOperationException("El <select> de categorías está vacío");

            var leagues = new List<LeagueSummary>();
            foreach (var opt in optionNodes)
            {
                var id = opt.GetAttributeValue("value", "?");
                var name = opt.InnerText.Trim();
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
                    continue;

                leagues.Add(new LeagueSummary
                {
                    CompetitionId = competitionId,
                    SeasonId = seasonId,
                    SeasonName = seasonName,
                    CategoryId = id,
                    CategoryName = name
                });
            }
            return leagues;
        }

        /// <summary>
        /// Firma sin parámetro para compatibilidad: siempre lanza excepción
        /// </summary>
        public Task<List<LeagueSummary>> GetAvailableLeaguesAsync()
            => throw new NotSupportedException("Debe proporcionar competitionId a GetAvailableLeaguesAsync");

        /// <summary>
        /// Obtiene metadatos de la liga usando la página de cabecera y la de clasificación
        /// </summary>
        public async Task<LeagueMetadata> GetLeagueDetailsAsync(LeagueSummary summary)
        {
            // 1) Página de cabecera: competicion.php
            var headerUrl = $"{CompetitionUrl}?seleccion=0&id={summary.CompetitionId}&id_ambito=0";
            var headerHtml = await _http.GetStringAsync(headerUrl);
            var headerDoc = new HtmlDocument(); headerDoc.LoadHtml(headerHtml);

            // Season text
            var seasonNode = headerDoc.DocumentNode
                .SelectSingleNode($"//select[@id='temporadas']/option[@value='{summary.SeasonId}']")
                ?? headerDoc.DocumentNode.SelectSingleNode("//select[@id='temporadas']/option[@selected]");
            var seasonText = seasonNode?.InnerText.Trim() ?? summary.SeasonName;

            // Territorial select y hidden input
            var terrSelectNode = headerDoc.DocumentNode.SelectSingleNode("//select[@id='territorial']/option[@selected]");
            var terrInputNode = headerDoc.DocumentNode.SelectSingleNode("//input[@id='id_territorial']");
            var territorialId = terrInputNode?.GetAttributeValue("value", "0") ?? "0";
            var territorialText = terrSelectNode?.InnerText.Trim()
                                  ?? headerDoc.DocumentNode.SelectSingleNode($"//select[@id='territorial']/option[@value='{territorialId}']")?.InnerText.Trim()
                                  ?? "";

            // Competición (nombre)
            var compNode = headerDoc.DocumentNode.SelectSingleNode($"//select[@id='competiciones']/option[@value='{summary.CompetitionId}']")
                         ?? headerDoc.DocumentNode.SelectSingleNode("//select[@id='competiciones']/option[@selected]");
            var competitionName = compNode?.InnerText.Trim() ?? "";

            // Fase (id y nombre)
            var phaseNode = headerDoc.DocumentNode.SelectSingleNode("//select[@id='torneos']/option[@selected]")
                          ?? headerDoc.DocumentNode.SelectSingleNode("//select[@id='torneos']/option");
            var phaseId = phaseNode?.GetAttributeValue("value", "0") ?? "0";
            var phaseName = phaseNode?.InnerText.Trim() ?? "";

            // LeagueScopeId (ambito)
            var scopeInput = headerDoc.DocumentNode.SelectSingleNode("//input[@id='id_ambito']");
            var leagueScopeId = scopeInput?.GetAttributeValue("value", "0") ?? "0";

            // 2) Página de clasificación para datos específicos si los necesitas
            var classUrl = $"{ClassificationUrl}?seleccion=2&id_categoria={summary.CategoryId}&id_competicion={summary.CompetitionId}&id_torneo={phaseId}&id_temporada={summary.SeasonId}";
            var classHtml = await _http.GetStringAsync(classUrl);
            // var classDoc = new HtmlDocument(); classDoc.LoadHtml(classHtml);
            // Aquí podrías extraer otros datos si necesitas

            return new LeagueMetadata(
                summary.CompetitionId,
                leagueScopeId,
                summary.SeasonId,
                seasonText,
                territorialId,
                territorialText,
                summary.CategoryId,
                summary.CategoryName,
                competitionName,
                phaseId,
                phaseName
            );
        }

        public async Task<List<TeamStanding>> GetStandingsAsync(
            string categoryId,
            string competitionId,
            string phaseId,
            string seasonId = null)
        {
            if (string.IsNullOrEmpty(seasonId))
                seasonId = (await GetAvailableLeaguesAsync(competitionId)).First().SeasonId;

            var url = ClassificationUrl
                      + $"?seleccion=2&id_categoria={categoryId}"
                      + $"&id_competicion={competitionId}"
                      + $"&id_torneo={phaseId}"
                      + $"&id_temporada={seasonId}";
            var html = await _http.GetStringAsync(url);
            var doc = new HtmlDocument(); doc.LoadHtml(html);

            var standings = new List<TeamStanding>();
            var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class,'tabla_clasificacion')]");
            if (table == null) return standings;

            var rows = table.SelectNodes(".//tr[position()>1]");
            if (rows == null) return standings;

            foreach (var row in rows)
            {
                var cells = row.SelectNodes(".//td");
                if (cells == null || cells.Count < 10) continue;

                standings.Add(new TeamStanding
                {
                    Position = ParseInt(cells[0].InnerText),
                    TeamName = cells[1].InnerText.Trim(),
                    GamesPlayed = ParseInt(cells[2].InnerText),
                    Won = ParseInt(cells[3].InnerText),
                    Tied = ParseInt(cells[4].InnerText),
                    Lost = ParseInt(cells[5].InnerText),
                    GoalsFor = ParseInt(cells[6].InnerText),
                    GoalsAgainst = ParseInt(cells[7].InnerText),
                    GoalDifference = ParseInt(cells[8].InnerText),
                    Points = ParseInt(cells[9].InnerText)
                });
            }

            return standings;
        }

        private int ParseInt(string value) =>
            int.TryParse(value.Trim(), out var r) ? r : 0;
    }

    public class LeagueSummary
    {
        public string CompetitionId { get; set; }
        public string SeasonId { get; set; }
        public string SeasonName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public record LeagueMetadata(
        string CompetitionId,
        string LeagueScopeId,
        string SeasonId,
        string SeasonName,
        string TerritorialId,
        string TerritorialName,
        string CategoryId,
        string CategoryName,
        string CompetitionName,
        string PhaseId,
        string PhaseName
    );

    public class TeamStanding
    {
        public int Position { get; set; }
        public string TeamName { get; set; }
        public int GamesPlayed { get; set; }
        public int Won { get; set; }
        public int Tied { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
    }
}

