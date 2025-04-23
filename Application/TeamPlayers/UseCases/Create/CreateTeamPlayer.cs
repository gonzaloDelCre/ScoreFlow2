using Application.TeamPlayers.DTOs;
using Domain.Services.TeamPlayers;
using Domain.Ports.Players;
using Application.TeamPlayers.Mappers;
using Domain.Ports.Teams;
using Domain.Shared;

namespace Application.TeamPlayers.UseCases.Create
{
    public class CreateTeamPlayer
    {
        private readonly TeamPlayerService _teamPlayerService;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public CreateTeamPlayer(
            TeamPlayerService teamPlayerService,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository)
        {
            _teamPlayerService = teamPlayerService;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public async Task<TeamPlayerResponseDTO?> ExecuteAsync(TeamPlayerRequestDTO request)
        {
            var team = await _teamRepository.GetByIdAsync(new TeamID(request.TeamID));
            var player = await _playerRepository.GetByIdAsync(new PlayerID(request.PlayerID));

            if (team == null || player == null)
                throw new InvalidOperationException("Equipo o jugador no encontrado.");

            var domain = request.ToDomain(team, player);
            var result = await _teamPlayerService.AddAsync(domain);

            return result?.ToResponseDTO();
        }
    }
}
