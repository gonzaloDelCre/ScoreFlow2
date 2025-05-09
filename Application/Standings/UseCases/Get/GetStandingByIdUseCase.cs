using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Domain.Entities.Standings;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetStandingByIdUseCase
    {
        private readonly IStandingRepository _repo;
        public GetStandingByIdUseCase(IStandingRepository repo) => _repo = repo;

        public async Task<StandingResponseDTO?> ExecuteAsync(int id)
        {
            var s = await _repo.GetByIdAsync(new StandingID(id));
            return s is null ? null : s.ToDTO();
        }
    }
}
