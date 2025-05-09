using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Ports.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Get
{
    public class GetLeagueByNameUseCase
    {
        private readonly ILeagueRepository _repo;

        public GetLeagueByNameUseCase(ILeagueRepository repo) => _repo = repo;

        public async Task<LeagueResponseDTO?> ExecuteAsync(string name)
        {
            var l = await _repo.GetByNameAsync(name);
            return l is null ? null : l.ToDTO();
        }
    }
}
