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
    public class GetTopScorersUseCase
    {
        private readonly IPlayerRepository _repo;
        public GetTopScorersUseCase(IPlayerRepository repo) => _repo = repo;

        public async Task<List<PlayerResponseDTO>> ExecuteAsync(int topN)
        {
            var list = await _repo.GetTopScorersAsync(topN);
            return list.Select(p => p.ToDTO()).ToList();
        }
    }
}
