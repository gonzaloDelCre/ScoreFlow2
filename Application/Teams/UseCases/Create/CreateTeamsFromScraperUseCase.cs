using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Teams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeamsFromScraperUseCase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly TeamScraperService _teamScraperService;

        public CreateTeamsFromScraperUseCase(ITeamRepository teamRepository, TeamScraperService teamScraperService)
        {
            _teamRepository = teamRepository;
            _teamScraperService = teamScraperService;
        }

        public async Task<List<TeamResponseDTO>> ExecuteAsync()
        {
            // Obtener equipos del scraper
            var teamsFromScraper = await _teamScraperService.ScrapeTeamsAsync();
            var createdTeams = new List<TeamResponseDTO>();

            // Crear equipos en la base de datos
            foreach (var teamDTO in teamsFromScraper)
            {
                var team = new Team(
                    new TeamID(0),  // El ID se asigna en la base de datos
                    new TeamName(teamDTO.Name),
                    null,  // Aquí podrías asignar el entrenador si lo tienes disponible
                    DateTime.UtcNow,
                    teamDTO.Logo
                );

                var createdTeam = await _teamRepository.AddAsync(team);

                // Si necesitas asociar jugadores al equipo, habría que hacerlo aquí
                // dependiendo de cómo está configurada tu entidad Team y relaciones

                createdTeams.Add(new TeamMapper().MapToDTO(createdTeam));
            }

            return createdTeams;
        }
    }
}
