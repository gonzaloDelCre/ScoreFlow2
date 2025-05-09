using Application.Teams.DTOs;
using Application.Teams.Mapper;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using System;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeamUseCase
    {
        private readonly ITeamRepository _repo;
        public CreateTeamUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<TeamResponseDTO> ExecuteAsync(TeamRequestDTO dto)
        {
            var team = dto.ToDomain();
            var created = await _repo.AddAsync(team);
            return created.ToDTO();
        }
    }
}
