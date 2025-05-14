using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Ports.Leagues;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Shared;

namespace Application.Matches.UseCases.Update
{
    public class UpdateMatchUseCase
    {
        private readonly IMatchRepository _repo;
        private readonly ITeamRepository _teamRepo;
        private readonly ILeagueRepository _leagueRepo;  

        public UpdateMatchUseCase(
            IMatchRepository repo,
            ITeamRepository teamRepo,
            ILeagueRepository leagueRepo)              
        {
            _repo = repo;
            _teamRepo = teamRepo;
            _leagueRepo = leagueRepo;
        }

        public async Task<MatchResponseDTO?> ExecuteAsync(MatchRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar.");

            var id = new MatchID(dto.ID.Value);
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null) return null;

            var team1 = await _teamRepo.GetByIdAsync(new TeamID(dto.Team1ID));
            var team2 = await _teamRepo.GetByIdAsync(new TeamID(dto.Team2ID));
            var league = await _leagueRepo.GetByIdAsync(new LeagueID(dto.LeagueID));

            if (team1 == null || team2 == null || league == null)
                throw new ArgumentException("Equipo(s) o liga inválidos.");

            existing.Update(
                team1: team1,
                team2: team2,
                matchDate: dto.MatchDate,
                status: dto.Status,
                location: dto.Location,
                league: league,          
                jornada: dto.Jornada        
            );
            existing.UpdateScore(dto.ScoreTeam1, dto.ScoreTeam2);

            await _repo.UpdateAsync(existing);
            var updated = await _repo.GetByIdAsync(id);
            return updated?.ToDTO();
        }
    }
}
