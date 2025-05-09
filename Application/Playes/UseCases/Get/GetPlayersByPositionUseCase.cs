using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Enum;
using Domain.Ports.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Get
{
    public class GetPlayersByPositionUseCase
    {
        private readonly IPlayerRepository _repo;
        public GetPlayersByPositionUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<List<PlayerResponseDTO>> ExecuteAsync(PlayerPosition position)
        {
            var list = await _repo.GetByPositionAsync(position);
            return list.Select(p => p.ToDTO()).ToList();
        }
    }
}
