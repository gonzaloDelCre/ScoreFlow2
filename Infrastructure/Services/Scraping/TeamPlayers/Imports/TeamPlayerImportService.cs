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
    public class TeamPlayerImportService
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IPlayerRepository _playerRepo;
        private readonly ITeamPlayerRepository _teamPlayerRepo;

        public TeamPlayerImportService(
            ITeamRepository teamRepo,
            IPlayerRepository playerRepo,
            ITeamPlayerRepository teamPlayerRepo)
        {
            _teamRepo = teamRepo;
            _playerRepo = playerRepo;
            _teamPlayerRepo = teamPlayerRepo;
        }

        /// <summary>
        /// Vincula todos los jugadores existentes a un equipo,
        /// creando registros en TeamPlayers para los que no estén ya.
        /// </summary>
        public async Task LinkPlayersToTeamAsync(int teamId)
        {
            // 1) Obtiene el equipo de dominio
            var team = await _teamRepo.GetByIdAsync(new TeamID(teamId));
            if (team == null)
            {
                Console.WriteLine($"⚠️ Equipo {teamId} no encontrado.");
                return;
            }

            // 2) Obtiene todos los jugadores que ya estén asignados a ese equipo
            var assigned = await _teamPlayerRepo.GetByTeamIdAsync(team.TeamID);
            var assignedIds = new HashSet<PlayerID>(assigned.Select(tp => tp.PlayerID));

            // 3) Obtiene desde tu otro repositorio todos los jugadores actuales del equipo
            var players = await _playerRepo.GetByTeamIdAsync(team.TeamID.Value);

            foreach (var player in players)
            {
                // 4) Si este jugador aún no está en TeamPlayers, lo creamos
                if (!assignedIds.Contains(player.PlayerID))
                {
                    Console.WriteLine($"– Vinculando jugador {player.PlayerID.Value} al equipo {team.TeamID.Value}...");
                    var tp = new TeamPlayer(
                        team.TeamID,
                        player.PlayerID,
                        new JoinedAt(DateTime.UtcNow),
                        roleInTeam: null
                    );

                    await _teamPlayerRepo.AddAsync(tp);
                    Console.WriteLine("   → Creado TeamPlayer.");
                }
            }

            Console.WriteLine("✅ Vinculación de jugadores completada.");
        }
    }
}
