using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeamsFromScraperUseCase
    {
        private readonly ITeamImporter _importer;

        public CreateTeamsFromScraperUseCase(ITeamImporter importer)
        {
            _importer = importer;
        }

        public async Task ExecuteAsync()
        {
            await _importer.ImportAsync();
        }
    }
}
