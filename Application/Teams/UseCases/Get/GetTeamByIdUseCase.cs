using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class GetTeamByIdUseCase
    {
        private readonly ITeamRepository _repo;
        public GetTeamByIdUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<TeamResponseDTO?> ExecuteAsync(int id)
        {
            var t = await _repo.GetByIdAsync(new TeamID(id));
            return t is null ? null : t.ToDTO();
        }
    }
}
