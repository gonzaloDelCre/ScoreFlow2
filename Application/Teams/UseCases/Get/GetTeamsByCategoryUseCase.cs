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
    public class GetTeamsByCategoryUseCase
    {
        private readonly ITeamRepository _repo;
        public GetTeamsByCategoryUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<List<TeamResponseDTO>> ExecuteAsync(string category)
        {
            var list = await _repo.GetByCategoryAsync(category);
            return list.Select(t => t.ToDTO()).ToList();
        }
    }
}
