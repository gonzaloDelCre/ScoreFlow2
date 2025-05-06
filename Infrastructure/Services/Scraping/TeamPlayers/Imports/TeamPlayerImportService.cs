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

namespace Infrastructure.Services.Scraping.TeamPlayers.Imports
{
    public class TeamPlayerImportService : ITeamPlayerImporter
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IPlayerRepository _playerRepo;
        private readonly ITeamPlayerRepository _repo;

        public TeamPlayerImportService(ITeamRepository teamRepo, IPlayerRepository playerRepo, ITeamPlayerRepository repo)
        {
            _teamRepo = teamRepo;
            _playerRepo = playerRepo;
            _repo = repo;
        }

        public async Task LinkPlayersToTeamAsync(int teamId)
        {
            var team = await _teamRepo.GetByIdAsync(new TeamID(teamId));
            if (team == null) return;

            var players = await _playerRepo.GetByTeamIdAsync(team.TeamID);
            foreach (var player in players)
            {
                var exists = await _repo.GetByIdsAsync(team.TeamID, player.PlayerID);
                if (exists == null)
                {
                    var tp = new TeamPlayer(team.TeamID, player.PlayerID, DateTime.UtcNow, null, team, player);
                    await _repo.AddAsync(tp);
                }
            }
        }
    }

}
