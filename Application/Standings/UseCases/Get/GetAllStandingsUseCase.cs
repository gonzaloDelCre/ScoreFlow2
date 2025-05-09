using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetAllStandingsUseCase
    {
        private readonly IStandingRepository _repo;
        public GetAllStandingsUseCase(IStandingRepository repo) => _repo = repo;

        public async Task<List<StandingResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(s => s.ToDTO()).ToList();
        }
    }
}
