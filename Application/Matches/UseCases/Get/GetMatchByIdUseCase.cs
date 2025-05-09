using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Ports.Matches;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Get
{
    public class GetMatchByIdUseCase
    {
        private readonly IMatchRepository _repo;

        public GetMatchByIdUseCase(IMatchRepository repo) => _repo = repo;

        public async Task<MatchResponseDTO?> ExecuteAsync(int id)
        {
            var match = await _repo.GetByIdAsync(new MatchID(id));
            return match is null
                ? null
                : match.ToDTO();
        }
    }
}
