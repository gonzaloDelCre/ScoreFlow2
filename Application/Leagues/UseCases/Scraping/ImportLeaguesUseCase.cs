using Application.Leagues.DTOs;
using Domain.Ports.Leagues;
using Domain.Shared;
using Infrastructure.Services.Scraping.Leagues.Import;
using Infrastructure.Services.Scraping.Leagues.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Scraping
{
    public class ImportLeaguesUseCase
    {
        private readonly LeagueImportService _service;

        public ImportLeaguesUseCase(LeagueImportService service)
        {
            _service = service;
        }

        public Task ExecuteAsync(string competitionId)
        {
            return _service.ImportAsync(competitionId);
        }
    }
}


