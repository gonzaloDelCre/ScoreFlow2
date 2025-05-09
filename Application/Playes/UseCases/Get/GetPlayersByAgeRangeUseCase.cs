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
    public class GetPlayersByAgeRangeUseCase
    {
        private readonly IPlayerRepository _repo;
        public GetPlayersByAgeRangeUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<List<PlayerResponseDTO>> ExecuteAsync(int minAge, int maxAge)
        {
            var list = await _repo.GetByAgeRangeAsync(minAge, maxAge);
            return list.Select(p => p.ToDTO()).ToList();
        }
    }
}
