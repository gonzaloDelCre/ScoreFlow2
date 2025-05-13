using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Ports.Leagues;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Matches.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Matches.Import
{
    public class MatchImportService
    {
        private readonly MatchScraperService _scraper;
        private readonly IMatchRepository _matchRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly ILeagueRepository _leagueRepo;
        private readonly ILogger<MatchImportService> _logger;

        public MatchImportService(
            MatchScraperService scraper,
            IMatchRepository matchRepo,
            ITeamRepository teamRepo,
            ILeagueRepository leagueRepo,
            ILogger<MatchImportService> logger)
        {
            _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
            _matchRepo = matchRepo ?? throw new ArgumentNullException(nameof(matchRepo));
            _teamRepo = teamRepo ?? throw new ArgumentNullException(nameof(teamRepo));
            _leagueRepo = leagueRepo ?? throw new ArgumentNullException(nameof(leagueRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Importa partidos para una liga específica de la base de datos
        /// </summary>
        /// <param name="leagueId">ID interno de la liga en la base de datos</param>
        /// <param name="competitionId">ID externo de la competición en RFEBM (opcional, si no se proporciona se usará el guardado en la descripción de la liga)</param>
        /// <exception cref="ArgumentException">Si la liga no existe</exception>
        /// <exception cref="InvalidOperationException">Si no se puede determinar el ID de competición</exception>
        public async Task ImportAsync(int leagueId, string competitionId = null)
        {
            _logger.LogInformation($"Iniciando importación de partidos para la liga ID {leagueId}...");

            // 1) Cargar dominio de la liga
            var leagueDomain = await _leagueRepo.GetByIdAsync(new LeagueID(leagueId));
            if (leagueDomain == null)
            {
                var errorMsg = $"Liga con ID {leagueId} no encontrada.";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg, nameof(leagueId));
            }

            // Si no se proporciona competitionId, intentar extraerlo de la descripción de la liga
            if (string.IsNullOrEmpty(competitionId))
            {
                // Buscamos el patrón "CompID=X" en la descripción
                var description = leagueDomain.Description ?? "";
                var compIdMatch = System.Text.RegularExpressions.Regex.Match(description, @"CompID=(\d+)");
                if (compIdMatch.Success)
                {
                    competitionId = compIdMatch.Groups[1].Value;
                    _logger.LogInformation($"Usando CompetitionId={competitionId} extraído de la descripción de la liga");
                }
                else
                {
                    var errorMsg = "No se pudo determinar el ID de competición. Asegúrate de que la liga tiene el formato correcto en su descripción (CompID=X).";
                    _logger.LogError(errorMsg);
                    throw new InvalidOperationException(errorMsg);
                }
            }

            // 2) Scrapeo de todas las jornadas
            _logger.LogInformation($"Scrapeando partidos para la competición ID {competitionId}...");
            var scraped = await _scraper.GetAllMatchesAsync(competitionId);
            _logger.LogInformation($"Scrapeados {scraped.Count} partidos en todas las jornadas.");

            // 3) Partidos ya existentes en BD
            var existing = (await _matchRepo.GetByLeagueIdAsync(leagueId)).ToList();
            _logger.LogInformation($"Encontrados {existing.Count} partidos existentes en BD para esta liga.");

            int created = 0;
            int updated = 0;
            int skipped = 0;

            // 4) Procesar cada partido scrapeado
            foreach (var m in scraped)
            {
                _logger.LogDebug($"Procesando: {m.LocalName} vs {m.VisitorName} (J{m.Jornada}) {m.Date:dd/MM/yyyy HH:mm}");

                // Asociar equipos por external ID
                var team1 = await _teamRepo.GetByExternalIdAsync(m.LocalId.ToString());
                var team2 = await _teamRepo.GetByExternalIdAsync(m.VisitorId.ToString());
                if (team1 == null || team2 == null)
                {
                    _logger.LogWarning($"Uno o ambos equipos no existen en BD: Local={m.LocalId}/{m.LocalName}, Visitante={m.VisitorId}/{m.VisitorName}. Omitiendo partido.");
                    skipped++;
                    continue;
                }

                // Buscar partido existente por equipos, fecha y jornada
                var found = existing.FirstOrDefault(x =>
                    x.Team1.TeamID.Value == team1.TeamID.Value &&
                    x.Team2.TeamID.Value == team2.TeamID.Value &&
                    Math.Abs((x.MatchDate - m.Date).TotalHours) < 24 && // +/- 24 horas de tolerancia
                    x.Jornada == m.Jornada);

                if (found == null)
                {
                    try
                    {
                        // Crear nuevo partido
                        _logger.LogInformation($"Creando nuevo partido: {m.LocalName} vs {m.VisitorName} (J{m.Jornada})");
                        var match = new Match(
                            new MatchID(1), // ID temporal
                            team1,
                            team2,
                            m.Date,
                            m.Status,
                            m.Location,
                            leagueDomain,
                            m.Jornada);

                        match.UpdateScore(m.Score1, m.Score2);
                        await _matchRepo.AddAsync(match);
                        created++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al crear partido: {m.LocalName} vs {m.VisitorName}. Omitiendo.");
                        skipped++;
                    }
                }
                else
                {
                    // Actualizar partido existente si hay cambios
                    var dirty = false;

                    if (found.ScoreTeam1 != m.Score1 || found.ScoreTeam2 != m.Score2)
                    {
                        found.UpdateScore(m.Score1, m.Score2);
                        dirty = true;
                    }

                    if (found.Status != m.Status)
                    {
                        found.Update(
                            found.Team1,
                            found.Team2,
                            found.MatchDate,
                            m.Status,
                            found.Location,
                            leagueDomain,
                            m.Jornada);
                        dirty = true;
                    }

                    if (dirty)
                    {
                        try
                        {
                            _logger.LogInformation($"Actualizando partido: {m.LocalName} vs {m.VisitorName} (J{m.Jornada})");
                            await _matchRepo.UpdateAsync(found);
                            updated++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error al actualizar partido: {m.LocalName} vs {m.VisitorName}. Omitiendo actualización.");
                        }
                    }
                    else
                    {
                        _logger.LogDebug($"Partido sin cambios: {m.LocalName} vs {m.VisitorName} (J{m.Jornada})");
                        skipped++;
                    }
                }
            }

            _logger.LogInformation($"Importación completada: {created} partidos creados, {updated} actualizados, {skipped} sin cambios.");
        }
    }
}