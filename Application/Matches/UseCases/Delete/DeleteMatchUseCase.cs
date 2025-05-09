using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Ports.Matches;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Delete
{
    public class DeleteMatchUseCase
    {
        private readonly IMatchRepository _repo;

        public DeleteMatchUseCase(IMatchRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id) =>
            _repo.DeleteAsync(new MatchID(id));
    }
}
