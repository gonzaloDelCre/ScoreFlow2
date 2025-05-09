using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Ports.Leagues;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Shared;

namespace Application.Matches.UseCases.Create
{
    public class CreateMatchUseCase
    {
        private readonly IMatchRepository _repo;
        private readonly ITeamRepository _teamRepo;
        private readonly ILeagueRepository _leagueRepo;    

        public CreateMatchUseCase(
            IMatchRepository repo,
            ITeamRepository teamRepo,
            ILeagueRepository leagueRepo)                
        {
            _repo = repo;
            _teamRepo = teamRepo;
            _leagueRepo = leagueRepo;
        }

        public async Task<MatchResponseDTO> ExecuteAsync(MatchRequestDTO dto)
        {
            var team1 = await _teamRepo.GetByIdAsync(new TeamID(dto.Team1ID));
            var team2 = await _teamRepo.GetByIdAsync(new TeamID(dto.Team2ID));
            var league = await _leagueRepo.GetByIdAsync(new LeagueID(dto.LeagueID)); 

            if (team1 == null || team2 == null || league == null)
                throw new ArgumentException("Equipo(s) o liga inválidos.");

            var domain = dto.ToDomain(team1, team2, league);
            var added = await _repo.AddAsync(domain);
            return added.ToDTO();
        }
    }
}
