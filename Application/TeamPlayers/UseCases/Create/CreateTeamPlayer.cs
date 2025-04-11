using Application.TeamPlayers.DTOs;
using Domain.Services;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Services.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Create
{
    public class CreateTeamPlayer
    {
        private readonly TeamPlayerService _teamPlayerService;

        public CreateTeamPlayer(TeamPlayerService teamPlayerService)
        {
            _teamPlayerService = teamPlayerService;
        }

        public async Task<TeamPlayerResponseDTO?> ExecuteAsync(TeamPlayerRequestDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var dummyTeam = new Domain.Entities.Teams.Team(
                new Domain.Shared.TeamID(request.TeamID),
                new Domain.Entities.Teams.TeamName("Equipo Dummy"),
                DateTime.UtcNow, 
                "" 
            );

            var dummyPlayer = new Domain.Entities.Players.Player(
                new Domain.Shared.PlayerID(request.PlayerID),
                new Domain.Entities.Players.PlayerName("Jugador Dummy"),
                Domain.Enum.PlayerPosition.LD,
                new Domain.Entities.Players.PlayerAge(0),
                0, 
                null, 
                DateTime.UtcNow,
                new List<Domain.Entities.TeamPlayers.TeamPlayer>()
            );

            var teamPlayer = request.ToDomain(dummyTeam, dummyPlayer);

            var result = await _teamPlayerService.AddAsync(teamPlayer);

            return result?.ToResponseDTO();
        }

    }
}
