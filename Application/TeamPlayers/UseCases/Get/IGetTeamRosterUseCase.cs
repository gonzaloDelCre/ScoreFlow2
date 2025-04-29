using Application.TeamPlayers.DTOs;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Get
{
    public interface IGetTeamRosterUseCase
    {
        Task<TeamRosterDto> ExecuteAsync(TeamID teamId);
    }
}
