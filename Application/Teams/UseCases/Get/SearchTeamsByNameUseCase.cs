using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Ports.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class SearchTeamsByNameUseCase
    {
        private readonly ITeamRepository _repo;
        public SearchTeamsByNameUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<List<TeamResponseDTO>> ExecuteAsync(string partialName)
        {
            var list = await _repo.SearchByNameAsync(partialName);
            return list.Select(t => t.ToDTO()).ToList();
        }
    }
}
