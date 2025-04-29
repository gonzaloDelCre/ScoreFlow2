using Application.TeamPlayers.DTOs;
using Domain.Services;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Services.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Update
{
    public class UpdateTeamPlayer
    {
        private readonly TeamPlayerService _teamPlayerService;

        public UpdateTeamPlayer(TeamPlayerService teamPlayerService)
        {
            _teamPlayerService = teamPlayerService;
        }

        public async Task ExecuteAsync(TeamPlayerRequestDTO dto, int teamId, int playerId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var dummyTeam = new Domain.Entities.Teams.Team(
                new Domain.Shared.TeamID(teamId),
                new Domain.Entities.Teams.TeamName("Dummy"),
                DateTime.UtcNow, 
                ""); 

            var dummyPlayer = new Domain.Entities.Players.Player(
                new Domain.Shared.PlayerID(playerId),
                new Domain.Entities.Players.PlayerName("Dummy"),
                Domain.Enum.PlayerPosition.JUGADOR,
                new Domain.Entities.Players.PlayerAge(0),
                0,
                null, 
                DateTime.UtcNow,
                new List<Domain.Entities.TeamPlayers.TeamPlayer>()
            );

            var teamPlayer = dto.ToDomain(dummyTeam, dummyPlayer);

            await _teamPlayerService.UpdateAsync(teamPlayer);
        }

    }
}
