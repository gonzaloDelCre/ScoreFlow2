using Domain.Entities.TeamPlayers;
using Domain.Ports.Players;
using Domain.Ports.TeamPlayers;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Scraping
{
    public class LinkPlayersToTeamUseCase
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IPlayerRepository _playerRepo;
        private readonly ITeamPlayerRepository _teamPlayerRepo;

        public LinkPlayersToTeamUseCase(
            ITeamRepository teamRepo,
            IPlayerRepository playerRepo,
            ITeamPlayerRepository teamPlayerRepo)
        {
            _teamRepo = teamRepo;
            _playerRepo = playerRepo;
            _teamPlayerRepo = teamPlayerRepo;
        }

        public async Task<int> ExecuteAsync(int teamId)
        {
            var team = await _teamRepo.GetByIdAsync(new TeamID(teamId));
            if (team == null)
            {
                throw new ArgumentException($"Equipo {teamId} no encontrado.");
            }

            var assigned = await _teamPlayerRepo.GetByTeamIdAsync(team.TeamID);
            var assignedIds = new HashSet<PlayerID>(assigned.Select(tp => tp.PlayerID));

            var players = await _playerRepo.GetByTeamIdAsync(team.TeamID.Value);

            int count = 0;

            foreach (var player in players)
            {
                if (!assignedIds.Contains(player.PlayerID))
                {
                    var tp = new TeamPlayer(
                        team.TeamID,
                        player.PlayerID,
                        new JoinedAt(DateTime.UtcNow),
                        roleInTeam: null
                    );
                    await _teamPlayerRepo.AddAsync(tp);
                    count++;
                }
            }

            return count;
        }
    }
}
