using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Ports.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Get
{
    public class GetPlayersByTeamUseCase
    {
        private readonly IPlayerRepository _repo;
        public GetPlayersByTeamUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<List<PlayerResponseDTO>> ExecuteAsync(int teamId)
        {
            var list = await _repo.GetByTeamIdAsync(teamId);
            return list.Select(p => p.ToDTO()).ToList();
        }
    }
}
