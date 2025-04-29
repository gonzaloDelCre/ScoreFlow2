using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Ports.TeamPlayers;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamRosterUseCase : IGetTeamRosterUseCase
    {
        private readonly ITeamPlayerRepository _tpRepo;

        public GetTeamRosterUseCase(ITeamPlayerRepository tpRepo)
            => _tpRepo = tpRepo;

        public async Task<TeamRosterDto> ExecuteAsync(TeamID teamId)
        {
            var rels = await _tpRepo.GetByTeamIdAsync(teamId);
            return rels.ToRosterDto();
        }
    }
}
