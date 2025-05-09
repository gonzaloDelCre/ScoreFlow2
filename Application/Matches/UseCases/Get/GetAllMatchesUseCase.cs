using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Ports.Matches;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Get
{
    public class GetAllMatchesUseCase
    {
        private readonly IMatchRepository _repo;

        public GetAllMatchesUseCase(IMatchRepository repo) => _repo = repo;

        public async Task<List<MatchResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(m => m.ToDTO()).ToList();
        }
    }
}
