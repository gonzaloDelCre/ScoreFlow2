using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Teams;
using Infrastructure.Services.Scraping.Teams.Import;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeamsFromScraperUseCase
    {
        private readonly TeamImportService _importer;

        public CreateTeamsFromScraperUseCase(TeamImportService importer)
        {
            _importer = importer;
        }

        public async Task ExecuteAsync()
        {
            await _importer.ImportAsync();
        }
    }
}
