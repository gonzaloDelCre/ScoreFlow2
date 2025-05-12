using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Shared;
using HtmlAgilityPack;
using Infrastructure.Services.Scraping.Leagues.Services;
using Infrastructure.Services.Scraping.Matches.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Leagues.Import
{
    public class LeagueImportService
    {
        private readonly LeagueScraperService _scraper;
        private readonly ILeagueRepository _leagueRepo;
        private readonly IMatchRepository _matchRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly ILogger<LeagueImportService> _logger;

        public LeagueImportService(
            LeagueScraperService scraper,
            ILeagueRepository leagueRepo,
            IMatchRepository matchRepo,
            ITeamRepository teamRepo,
            ILogger<LeagueImportService> logger)
        {
            _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
            _leagueRepo = leagueRepo ?? throw new ArgumentNullException(nameof(leagueRepo));
            _matchRepo = matchRepo ?? throw new ArgumentNullException(nameof(matchRepo));
            _teamRepo = teamRepo ?? throw new ArgumentNullException(nameof(teamRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Importa todas las ligas de una competición específica y sus partidos
        /// </summary>
        /// <param name="competitionId">ID de la competición (parámetro 'id' en la URL)</param>
        /// <param name="importMatches">Indica si se deben importar también los partidos de cada liga</param>
        public async Task ImportAsync(string competitionId, bool importMatches = true)
        {
            _logger.LogInformation("🚀 Empezando import de ligas para competición {CompetitionId}...", competitionId);

            try
            {
                var summaries = await _scraper.GetAvailableLeaguesAsync(competitionId);
                _logger.LogInformation("📦 Encontradas {Count} ligas.", summaries.Count);

                foreach (var summary in summaries)
                {
                    _logger.LogInformation("→ Procesando {CategoryName}...", summary.CategoryName);

                    try
                    {
                        var metadata = await _scraper.GetLeagueDetailsAsync(summary);
                        var league = await ImportLeagueFromMetadataAsync(metadata);

                        if (importMatches && league != null)
                        {
                            await ImportMatchesForLeagueAsync(league.LeagueID.Value, metadata.CompetitionId);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando liga {CategoryName}: {Message}",
                            summary.CategoryName, ex.Message);
                    }
                }

                _logger.LogInformation("✅ Import de ligas completado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el import de ligas: {Message}", ex.Message);
                throw;
            }
        }

        private async Task<League> ImportLeagueFromMetadataAsync(LeagueMetadata metadata)
        {
            var leagueName = $"{metadata.CategoryName} - {metadata.CompetitionName} ({metadata.SeasonName})";
            var description = $"Federación: {metadata.TerritorialName}, CompID={metadata.CompetitionId}, FaseID={metadata.PhaseId}";
            var existing = await _leagueRepo.GetByNameAsync(leagueName);

            if (existing == null)
            {
                _logger.LogInformation("  → Nueva liga. Insertando...");
                var league = new League(
                    new LeagueID(1), // ID temporal, se actualizará al insertar
                    new LeagueName(leagueName),
                    description,
                    DateTime.UtcNow
                );
                return await _leagueRepo.AddAsync(league);
            }
            else
            {
                _logger.LogInformation("  → Ya existe. Verificando cambios...");
                var dirty = false;

                if (existing.Name.Value != leagueName)
                {
                    existing.UpdateName(new LeagueName(leagueName));
                    dirty = true;
                }

                if (existing.Description != description)
                {
                    existing.UpdateDescription(description);
                    dirty = true;
                }

                if (dirty)
                {
                    _logger.LogInformation("  → Cambios detectados. Actualizando...");
                    await _leagueRepo.UpdateAsync(existing);
                }
                else
                {
                    _logger.LogInformation("  → Sin cambios.");
                }

                return existing;
            }
        }

        /// <summary>
        /// Importa todos los partidos de una liga específica
        /// </summary>
        private async Task ImportMatchesForLeagueAsync(int leagueId, string competitionId)
        {
            _logger.LogInformation("📊 Iniciando importación de partidos para liga ID {LeagueId}...", leagueId);

            try
            {
                // Crear un scraper para esta liga específica
                var matchScraper = new MatchScraperService(
                    new HttpClient(),
                    competitionId  // Pasamos el ID de competición para construir las URLs
                );

                // Obtener la liga
                var leagueDomain = await _leagueRepo.GetByIdAsync(new LeagueID(leagueId));
                if (leagueDomain == null)
                {
                    _logger.LogWarning("❌ Liga con ID {LeagueId} no encontrada. Abortando import de partidos.", leagueId);
                    return;
                }

                // Obtener partidos de esta liga
                var scraped = await matchScraper.GetAllMatchesAsync();
                _logger.LogInformation("📦 Scrapeados {Count} partidos en todas las jornadas.", scraped.Count);

                // Obtener partidos existentes en BD para esta liga
                var existing = (await _matchRepo.GetByLeagueIdAsync(leagueId)).ToList();

                // Procesar cada partido
                int created = 0;
                int updated = 0;
                int skipped = 0;

                foreach (var m in scraped)
                {
                    _logger.LogDebug("– {Local} vs {Visitor} (J{Jornada}) {Date}",
                        m.LocalName, m.VisitorName, m.Jornada, m.Date.ToString("dd/MM/yyyy HH:mm"));

                    // Asociar equipos por external ID
                    var team1 = await _teamRepo.GetByExternalIdAsync(m.LocalId.ToString());
                    var team2 = await _teamRepo.GetByExternalIdAsync(m.VisitorId.ToString());

                    if (team1 == null || team2 == null)
                    {
                        _logger.LogWarning("   ⚠️ Uno o ambos equipos no existen en BD, omitiendo...");
                        skipped++;
                        continue;
                    }

                    // Buscar partido existente por equipos, fecha y jornada
                    var found = existing.FirstOrDefault(x =>
                        x.Team1.TeamID.Value == team1.TeamID.Value &&
                        x.Team2.TeamID.Value == team2.TeamID.Value &&
                        x.MatchDate == m.Date &&
                        x.Jornada == m.Jornada);

                    if (found == null)
                    {
                        // Crear nuevo partido
                        _logger.LogInformation("   → Nuevo partido, creando...");

                        try
                        {
                            var match = new Domain.Entities.Matches.Match(
                                new MatchID(1), 
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
                            _logger.LogInformation("   → Añadido.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error creando partido: {Message}", ex.Message);
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
                            _logger.LogInformation("   → Actualizando partido...");
                            await _matchRepo.UpdateAsync(found);
                            updated++;
                            _logger.LogInformation("   → Actualizado.");
                        }
                        else
                        {
                            _logger.LogDebug("   → Sin cambios.");
                        }
                    }
                }

                _logger.LogInformation("✅ Import de partidos completado. Creados: {Created}, Actualizados: {Updated}, Omitidos: {Skipped}",
                    created, updated, skipped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la importación de partidos: {Message}", ex.Message);
            }
        }
    }
}
