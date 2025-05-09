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
    public class GetTeamByExternalIdUseCase
    {
        private readonly ITeamRepository _repo;
        public GetTeamByExternalIdUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<TeamResponseDTO?> ExecuteAsync(string externalId)
        {
            var t = await _repo.GetByExternalIdAsync(externalId);
            return t is null ? null : t.ToDTO();
        }
    }
}
