using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Ports.Players;
using Domain.Ports.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Get
{
    public class GetPlayerByNameUseCase
    {
        private readonly IPlayerRepository _repo;
        public GetPlayerByNameUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<PlayerResponseDTO?> ExecuteAsync(string name)
        {
            var p = await _repo.GetByNameAsync(name);
            return p is null ? null : p.ToDTO();
        }
    }
}
